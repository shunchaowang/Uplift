using System;
using System.Collections.Generic;
using System.Text;

namespace Uplift.DataAccess.Data.Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }

        void Save();
    }
}
