using Delfi.QueryProvider.RDF;
using System.Collections.Generic;

namespace Delfi.EntityFramework
{
    public interface IGraphWriter
    {
        void Insert(IEnumerable<Statement> statements);

        void Delete(IEnumerable<Statement> statements);
    }
}
