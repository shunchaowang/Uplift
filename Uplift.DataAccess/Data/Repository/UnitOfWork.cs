using System;
using System.Collections.Generic;
using System.Text;
using Uplift.DataAccess.Data.Repository.Interface;

namespace Uplift.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        public ICategoryRepository Category { get; private set; }
        public IFrequencyRepository Frequency { get; private set; }
        public IServiceRepository Service { get; private set; }
        public IUserRepository User { get; private set; }
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            Category = new CategoryRepository(dbContext);
            Frequency = new FrequencyRepository(dbContext);
            Service = new ServiceRepository(dbContext);
            User = new UserRepository(dbContext);
        }


        public void Dispose()
        {
            dbContext.Dispose();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
