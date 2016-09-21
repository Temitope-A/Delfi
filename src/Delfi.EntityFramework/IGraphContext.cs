using Delfi.QueryProvider;
using Delfi.QueryProvider.RDF;

namespace Delfi.EntityFramework
{
    public interface IGraphContext
    {
        void Add(Resource resource);

        void Append(Statement statement);

        IQueryableGraph Read<T>();      

        void Remove(Statement statement);

        void SaveChanges();
    }
}
