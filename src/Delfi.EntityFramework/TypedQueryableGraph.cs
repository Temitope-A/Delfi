using Delfi.QueryProvider;
using System;
using System.Reflection;
using Delfi.EntityFramework.Attributes;
using Delfi.QueryProvider.RDF;
using Delfi.QueryProvider.StandardNamespaces;
using Sparql.Algebra.Trees;
using Sparql.Algebra.RDF;
using System.Linq;
using System.Collections;
using Delfi.EntityFramework.Extensions;

namespace Delfi.EntityFramework
{
    /// <summary>
    /// Base implementation of the TypedQueryableGraph, an IQueryableGraph that supports typed expansions
    /// </summary>
    public class TypedQueryableGraph : QueryableGraph, ITypedQueryableGraph
    {
        /// <summary>
        /// Type graph
        /// </summary>
        public LabelledTreeNode<Type, Term> TypeGraph { get; }

        /// <summary>
        /// Initializes a new instance of a QueryableGraph &lt;T&gt; class
        /// </summary>
        /// <param name="graphProvider"></param>
        internal TypedQueryableGraph(IGraphProvider graphProvider) : base(graphProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of a QueryableGraph &lt;T&gt; class, for internal usage
        /// </summary>
        /// <param name="graphProvider"></param>
        /// <param name="graphExpression"></param>
        /// <param name="typeGraph"></param>
        private TypedQueryableGraph(IGraphProvider graphProvider, GraphExpression graphExpression, LabelledTreeNode<Type, Term> typeGraph) : base(graphProvider, graphExpression)
        {
            TypeGraph = typeGraph;
        }

        /// <summary>
        /// Expands the graph expression along the specified property.
        /// The new graph expression will bind a given context if and only if the old expression binds,
        /// while the new addition is allowed not to bind
        /// </summary>
        /// <typeparam name="Y">type of expansion</typeparam>
        /// <param name="property">property along which to expand</param>
        /// <returns>a TypedQueryableGraph</returns>
        public ITypedQueryableGraph Expand<Y>(Resource property) where Y : Resource
        {
            var expansion = Select<Y>();
            var typeGraph = TypeGraph.Copy().AddChild(property, expansion.TypeGraph);
            var query = Expand(property, expansion);

            return new TypedQueryableGraph(GraphProvider, query.GraphExpression, typeGraph);
        }

        /// <summary>
        /// Expands the graph expression along the specified property.
        /// The new graph expression will bind a given context if and only if both the old expression and the addition bind 
        /// </summary>
        /// <typeparam name="Y">type of expansion</typeparam>
        /// <param name="property">property along which to expand</param>
        /// <returns>a TypedQueryableGraph</returns>
        public ITypedQueryableGraph Require<Y>(Resource property) where Y : Resource
        {
            var expansion = Select<Y>();
            var typeGraph =TypeGraph.Copy().AddChild(property, expansion.TypeGraph);
            var query = Require(property, expansion);

            return new TypedQueryableGraph(GraphProvider, query.GraphExpression, typeGraph);
        }

        /// <summary>
        /// Expands the graph expression along the specified property, but returns only the terminal type
        /// The new graph expression will bind a given context if and only if both the old expression and the addition bind 
        /// </summary>
        /// <typeparam name="Y">type of query</typeparam>
        /// <param name="property">property along which to expand</param>
        /// <returns>a TypedQueryableGraph</returns>
        public ITypedQueryableGraph Select<Y>(Resource property) where Y : Resource
        {
            var expansion = Select<Y>();
            var query = Select(property, expansion);
            return new TypedQueryableGraph(GraphProvider, query.GraphExpression, new LabelledTreeNode<Type, Term>(typeof(Y)));
        }

        /// <summary>
        /// Start of a query, returns objects of the specified type
        /// </summary>
        /// <typeparam name="Y">type of query</typeparam>
        /// <returns>a TypedQueryableGraph</returns>
        public ITypedQueryableGraph Select<Y>() where Y : Resource
        {
            var objectGraph = ConvertTypeToGraph(typeof(Y));
            return new TypedQueryableGraph(GraphProvider, new GraphExpression(objectGraph), new LabelledTreeNode<Type, Term>(typeof(Y)));
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection of results.
        /// </summary>
        /// <returns>
        /// An System.Collections.IEnumerator object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach ( var graph in GraphProvider.Execute(GraphExpression) )
            {
                yield return TypifyGraph(graph, TypeGraph);
            }
        }

        /// <summary>
        /// Create a graph of typed objects using the type graph as a model
        /// </summary>
        private LabelledTreeNode<object, Term> TypifyGraph(LabelledTreeNode<object, Term> objectGraph, LabelledTreeNode<Type, Term> typeGraph)
        {
            var obj = ConvertGraphToTypedObject(objectGraph, typeGraph.Value);
            var typedGraph = new LabelledTreeNode<object, Term>(obj);

            foreach (var outEdge in typeGraph.Children)
            {
                var edge = outEdge.Edge;
                var childTypeGraph = outEdge.TerminalNode;
                var childObjectGraphs = objectGraph.Descend(edge);

                foreach (var childObjectGraph in childObjectGraphs)
                {
                    typedGraph.AddChild(edge, TypifyGraph(childObjectGraph, childTypeGraph));
                }
            }
            return typedGraph;
        }

        /// <summary>
        /// Reflection based recursive method to populate a type with graph node values
        /// </summary>
        private object ConvertGraphToTypedObject(LabelledTreeNode<object, Term> objectGraph, Type type)
        {
            if (objectGraph.Children.Count == 0)
            {
                if (type.GetTypeInfo().IsAssignableFrom(objectGraph.Value.GetType()))
                {
                    return objectGraph.Value;
                }
                else
                {
                    throw new InvalidCastException(string.Format("Cannot cast a {0} to a {1}", objectGraph.Value.GetType(), type));
                }
            }

            //here we use the fact that expansion types inherit from Resource
            var obj = Activator.CreateInstance(type, new[] { objectGraph.Value});

            foreach (var member in type.GetRuntimeProperties())
            {
                var memberAttribute = member.GetCustomAttribute<PropertyBindAttribute>();
                if (memberAttribute != null)
                {
                    var memberObjectGraphCollection = objectGraph.Descend(memberAttribute.Property);

                    if (member.PropertyType.IsListType())
                    {
                        var typedList = Activator.CreateInstance(member.PropertyType);
                        var addMethod = member.PropertyType.GetRuntimeMethod("Add", new Type[] {member.PropertyType.GenericTypeArguments[0]});

                        foreach (var memberObjectGraph in memberObjectGraphCollection)
                        {
                            addMethod.Invoke(typedList, new[] { ConvertGraphToTypedObject(memberObjectGraph, member.PropertyType.GenericTypeArguments[0]) });
                        }
                        member.SetValue(obj, typedList);
                    }
                    else
                    {
                        LabelledTreeNode<object, Term> memberObjectGraph = null;
                        try
                        {
                            memberObjectGraph = memberObjectGraphCollection.Single();
                        }
                        catch (Exception)
                        {
                            throw new InvalidOperationException(string.Format("The type {0} declared a non collecton property {1} but more than one node matched", type, memberAttribute.Property));
                        }
                        member.SetValue(obj, ConvertGraphToTypedObject(memberObjectGraph, member.PropertyType));
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// Recursively create a query model for a particular type
        /// </summary>
        private LabelledTreeNode<object, Term> ConvertTypeToGraph(Type objectType)
        {
            var graph = new LabelledTreeNode<object, Term>(new Variable());

            var resourceAttribute = objectType.GetTypeInfo().GetCustomAttribute<EntityBindAttribute>();
            if (resourceAttribute != null)
            {
                graph.AddChild(new Rdf("type"), resourceAttribute.Type);
            }

            foreach (var member in objectType.GetRuntimeProperties())
            {
                var memberAttribute = member.GetCustomAttribute<PropertyBindAttribute>();
                if (memberAttribute != null)
                {
                    Type relevantType;
                    if (member.PropertyType.IsListType())
                    {
                        relevantType = member.PropertyType.GenericTypeArguments[0];
                    }
                    else
                    {
                        relevantType = member.PropertyType;
                    }
                    var memberGraph = ConvertTypeToGraph(relevantType);
                    graph.AddChild(memberAttribute.Property, memberGraph);
                }
            }

            return graph;
        }
    }
}
