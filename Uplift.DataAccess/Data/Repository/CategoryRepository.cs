using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uplift.DataAccess.Data.Repository.Interface;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;
        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
        public IEnumerable<SelectListItem> GetCategoryListForDropDown()
        {
            return dbContext.Category.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }) ;
        }

        public void Update(Category category)
        {
            var objFromDb = dbContext.Category.FirstOrDefault(s => s.Id == category.Id);
            objFromDb.Name = category.Name;
            objFromDb.DisplayOrder = category.DisplayOrder;
            dbContext.SaveChanges();
        }
    }
}
