using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uplift.DataAccess.Data;
using Uplift.Models;
using Uplift.Utility;

namespace Uplift.DataAccess.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DbInitializer(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public void initialize()
        {
            try
            {
                if (dbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    dbContext.Database.Migrate();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            if (dbContext.Roles.Any(r => r.Name == SD.Admin)) return;
            roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();

            if (dbContext.Roles.Any(r => r.Name == SD.Manager)) return;
            roleManager.CreateAsync(new IdentityRole(SD.Manager)).GetAwaiter().GetResult();

            // seed an admin user
            userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@me.com",
                Email = "admin@me.com",
                EmailConfirmed = true,
                Name = "Admin Admin"
            }, "Password1!").GetAwaiter().GetResult();

            ApplicationUser user = dbContext.ApplicationUser.Where(u => u.Email == "admin@me.com").FirstOrDefault();
            userManager.AddToRoleAsync(user, SD.Admin).GetAwaiter().GetResult();
        }
    }
}
