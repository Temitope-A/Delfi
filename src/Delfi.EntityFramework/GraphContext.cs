using Delfi.QueryProvider;
using Delfi.QueryProvider.Evaluators;
using Delfi.QueryProvider.RDF;
using System.Collections.Generic;
using System;

namespace Delfi.EntityFramework
{
    /// <summary>
    /// A simple read and write context
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

        private List<Statement> _toAdd { get; }

        private List<Statement> _toDelete { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public GraphContext()
        {
            GraphProvider = new GraphProvider<SparqlBGPEvaluator>(Configuration.Instance.QueryEndpoint);
            GraphWriter = new GraphWriter(Configuration.Instance.UpdateEndpoint);
            _toAdd = new List<Statement>();
            _toDelete = new List<Statement>();
        }

        public void Add(Resource resource)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Append a statement
        /// </summary>
        /// <param name="statement"></param>
        public void Append(Statement statement)
        {
            _toAdd.Add(statement);
            _toDelete.Remove(statement);
        }

        /// <summary>
        /// Returns a queryable graph
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryableGraph Read<T>()
        {
            return new QueryableGraph(GraphProvider, new GraphExpression(new Variable(typeof(T))));
        }

        /// <summary>
        /// Removes a statement
        /// </summary>
        /// <param name="statement"></param>
        public void Remove(Statement statement)
        {
            _toDelete.Add(statement);
            _toAdd.Remove(statement);
        }

        /// <summary>
        /// Write additions and removal
        /// </summary>
        public void SaveChanges()
        {
            GraphWriter.Delete(_toDelete);
            GraphWriter.Insert(_toAdd);

            _toDelete.Clear();
            _toAdd.Clear();
        }
    }
}
