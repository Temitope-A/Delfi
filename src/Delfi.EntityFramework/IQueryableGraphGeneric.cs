using Delfi.QueryProvider;
using Delfi.QueryProvider.RDF;
using System.Collections.Generic;

namespace Delfi.EntityFramework
{
    /// <summary>
    /// An IQueryableGraph that supports generics
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQueryableGraph<T> : IQueryableGraph, IEnumerable<T>
    {
        IQueryableGraph<T> Expand<Y>(Resource property);

        IQueryableGraph<T> Require<Y>(Resource property);

        IQueryableGraph<Y> Select<Y>();

        IQueryableGraph<Y> Select<Y>(Resource property);
    }
}
