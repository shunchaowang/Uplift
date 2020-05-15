using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uplift.DataAccess.Data.Repository.Interface;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly ApplicationDbContext dbContext;

        public UserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public void LockUser(string userId)
        {
            var objFromDb = dbContext.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
            dbContext.SaveChanges();
        }

        public void UnlockUser(string userId)
        {
            var objFromDb = dbContext.ApplicationUser.FirstOrDefault(u => u.Id == userId);
            objFromDb.LockoutEnd = DateTime.Now;
            dbContext.SaveChanges();
        }
    }
}
