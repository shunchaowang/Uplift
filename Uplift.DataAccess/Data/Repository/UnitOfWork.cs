using System;
using System.Collections.Generic;
using System.Text;
using Uplift.DataAccess.Data.Repository.Interface;

namespace Uplift.DataAccess.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            Category = new CategoryRepository(this.dbContext);
        }

        public ICategoryRepository Category { get; private set; }

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
