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
        protected IGraphProvider GraphProvider { get; }

        protected IGraphUpdater GraphUpdater { get; }

        private List<Statement> _toAdd { get; }

        private List<Statement> _toDelete { get; }

        public GraphContext()
        {
            GraphProvider = new GraphProvider<SparqlBGPEvaluator>();
            GraphUpdater = new GraphUpdater();
            _toAdd = new List<Statement>();
            _toDelete = new List<Statement>();
        }

        public void Add(Resource resource)
        {
            throw new NotImplementedException();
        }

        public void Append(Statement statement)
        {
            _toAdd.Add(statement);
            _toDelete.Remove(statement);
        }

        public IQueryableGraph Read<T>()
        {
            return new QueryableGraph(GraphProvider, new GraphExpression(new Variable(typeof(T))));
        }

        public void Remove(Statement statement)
        {
            _toDelete.Add(statement);
            _toAdd.Remove(statement);
        }

        public void SaveChanges()
        {
            GraphUpdater.Delete(_toDelete);
            GraphUpdater.Insert(_toAdd);

            _toDelete.Clear();
            _toAdd.Clear();
        }
    }
}
