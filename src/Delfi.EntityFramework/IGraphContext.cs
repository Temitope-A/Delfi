using Delfi.QueryProvider.RDF;
using Sparql.Algebra.Trees;
using System;

namespace Delfi.EntityFramework
{
    public interface IGraphContext
    {
        IQueryableGraph<T> Read<T>();

        void Add(Resource resource);

        void Append(LabelledTreeNode<Resource, Resource> graph);

        void Remove(LabelledTreeNode<Resource, Resource> graph);

        void SaveChanges();
    }
}
