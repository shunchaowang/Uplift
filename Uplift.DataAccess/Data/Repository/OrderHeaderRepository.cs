using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Uplift.DataAccess.Data.Repository.Interface;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext dbContext;
        public OrderHeaderRepository(ApplicationDbContext dbContext) : base(dbContext) => this.dbContext = dbContext;

        public void ChangeOrderStatus(int orderHeaderId, string status)
        {
            var objFromDb = dbContext.OrderHeader.FirstOrDefault(o => o.Id == orderHeaderId);
            objFromDb.Status = status;
            dbContext.SaveChanges();
        }
    }
}
