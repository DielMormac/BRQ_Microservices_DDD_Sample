using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Transaction.API.Interfaces
{
    /// <summary>
    /// Provides Handlers operations.
    /// </summary>
    /// <typeparam name="T">Entity to be handled.</typeparam>
    public interface IHandler<T>
    {
        /// <summary>
        /// Handle entity.
        /// </summary>
        /// <returns>Entity handled.</returns>
        T Handle(T entity);
    }
}
