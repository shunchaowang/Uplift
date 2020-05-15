using System;
using System.Collections.Generic;
using System.Text;

namespace Uplift.DataAccess.Data.Repository.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository Category { get; }
        IFrequencyRepository Frequency { get; }
        IServiceRepository Service { get; }
        IUserRepository User { get; }

        void Save();
    }
}
