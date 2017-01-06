using Delfi.EntityFramework.Attributes;
using Delfi.EntityFramework.Extensions;
using Delfi.QueryProvider;
using Delfi.QueryProvider.Evaluators;
using Delfi.QueryProvider.RDF;
using Delfi.QueryProvider.StandardNamespaces;
using Sparql.Algebra.RDF;
using Sparql.Algebra.Trees;
using System;
using System.Reflection;

namespace Delfi.EntityFramework
{
    /// <summary>
    /// A simple read and write context with no im memory persistence
    /// </summary>
    public class GraphContext : IGraphContext
    {
        /// <summary>
        /// Graph Provider
        /// </summary>
        protected IGraphProvider GraphProvider { get; }

        /// <summary>
        /// Graph Writer
        /// </summary>
        protected IGraphWriter GraphWriter { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GraphContext()
        {
            GraphProvider = new GraphProvider<SparqlBgpEvaluator>(Configuration.Instance.QueryEndpoint);
            GraphWriter = new GraphWriter(Configuration.Instance.UpdateEndpoint);
        }

        /// <summary>
        /// Returns a queryable graph
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>a TypedQueryableGraph</returns>
        public ITypedQueryableGraph Select<T>() where T : Resource
        {
            return (new TypedQueryableGraph(GraphProvider)).Select<T>();
        }

        /// <summary>
        /// Adds a typed object to the graph
        /// </summary>
        public void Add(Resource resource)
        {
            var graph = CreateObjectGraph(resource);
            Append(graph);
        }

        /// <summary>
        /// Appends a graph
        /// </summary>
        public void Append(LabelledTreeNode<object, Term> graph)
        {
            GraphWriter.Insert(graph);
        }

        /// <summary>
        /// Removes a graph
        /// </summary>
        public void Remove(LabelledTreeNode<object, Term> graph)
        {
            GraphWriter.Delete(graph);
        }

        private LabelledTreeNode<object, Term> CreateObjectGraph(Resource resource)
        {
            var graph = new LabelledTreeNode<object, Term>(new Resource(resource.Id));
            var objectType = resource.GetType();

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
                        var countMember = member.PropertyType.GetRuntimeProperty("Count");
                        var removeMethod = member.PropertyType.GetRuntimeMethod("Remove", new Type[] { });

                        while ((int)countMember.GetValue(member.GetValue(resource)) > 0)
                        {
                            var obj = removeMethod.Invoke(member.GetValue(resource), new object[] { });
                            AddChildToGraph(graph, obj, relevantType, memberAttribute.Property);
                        }
                    }
                    else
                    {
                        relevantType = member.PropertyType;
                        var obj = member.GetValue(resource);
                        AddChildToGraph(graph, obj, relevantType, memberAttribute.Property);
                    }
                }
            }
            return graph;
        }

        private void AddChildToGraph(LabelledTreeNode<object, Term> graph, object obj, Type objType, Resource property)
        {
            if (typeof(Resource).GetTypeInfo().IsAssignableFrom(objType))
            {
                var memberGraph = CreateObjectGraph((Resource)obj);
                graph.AddChild(property, memberGraph);
            }
            else
            {
                var value = obj;
                graph.AddChild(property, value);
            }
        }
    }
}
