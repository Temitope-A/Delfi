using Delfi.QueryProvider;
using System;
using System.Collections.Generic;
using Delfi.QueryProvider.RDF;
using Sparql.Algebra.Trees;
using Sparql.Algebra.RDF;

namespace Delfi.EntityFramework
{
    /// <summary>
    /// Base implementation of the IQueryableGraph &lt;T&gt;, an IQueryableGraph that supports generics
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class QueryableGraph<T> : QueryableGraph, IQueryableGraph<T>
    {
        /// <summary>
        /// Type graph
        /// </summary>
        protected LabelledTreeNode<Type, Resource> TypeGraph { get; }

        /// <summary>
        /// Initializes a new instance of a QueryableGraph &lt;T&gt; class
        /// </summary>
        /// <param name="graphProvider"></param>
        /// <param name="graphExpression"></param>
        public QueryableGraph(IGraphProvider graphProvider, GraphExpression graphExpression = null) : base(graphProvider, graphExpression)
        {
            TypeGraph = new LabelledTreeNode<Type, Resource>(typeof(T));
        }

        /// <summary>
        /// Initializes a new instance of a QueryableGraph &lt;T&gt; class, for internal usage
        /// </summary>
        /// <param name="graphProvider"></param>
        /// <param name="graphExpression"></param>
        /// <param name="typeGraph"></param>
        private QueryableGraph(IGraphProvider graphProvider, GraphExpression graphExpression, LabelledTreeNode<Type, Resource> typeGraph) : base(graphProvider, graphExpression)
        {
            TypeGraph = typeGraph;
        }

        public new IQueryableGraph<T> Expand<Y>(Resource property)
        {
            TypeGraph.AddChild(property, typeof(Y));

            var joinedQuery = Select<Y>();
            var query = Expand(property, joinedQuery);
            return new QueryableGraph<T>(GraphProvider, query.GraphExpression, TypeGraph);
        }

        public new IQueryableGraph<T> Require<Y>(Resource property)
        {
            TypeGraph.AddChild(property, typeof(Y));

            var joinedQuery = Select<Y>();
            var query = Require(property, joinedQuery);
            return new QueryableGraph<T>(GraphProvider, query.GraphExpression, TypeGraph);
        }

        public IQueryableGraph<Y> Select<Y>()
        {
            var objectGraph = CreateObjectGraph(typeof(Y));
            return new QueryableGraph<Y>(GraphProvider, new GraphExpression(objectGraph));
        }

        public IQueryableGraph<Y> Select<Y>(Resource property)
        {
            var objectGraph = CreateObjectGraph(typeof(Y));
            throw new NotImplementedException();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private LabelledTreeNode<object, Term> CreateObjectGraph(Type ojjectType)
        {
            throw new NotImplementedException();
        }
    }
}
