using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Uplift.DataAccess.Data.Repository.Interface;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class FrequencyRepository : Repository<Frequency>, IFrequencyRepository
    {
        private readonly ApplicationDbContext dbContext;

        public FrequencyRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public IEnumerable<SelectListItem> GetFrequencyListForDropDown()
        {
            return dbContext.Frequency.Select(i => new SelectListItem()
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }) ;
        }

        public void Update(Frequency frequency)
        {
            var objFromDb = dbContext.Frequency.FirstOrDefault(i => i.Id == frequency.Id);
            objFromDb.Name = frequency.Name;
            objFromDb.Count = frequency.Count;
            dbContext.SaveChanges();
        }
    }
}
