using System;
using System.Collections.Generic;
using System.Text;
using Uplift.DataAccess.Data.Repository.Interface;
using Uplift.Models;

namespace Uplift.DataAccess.Data.Repository
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext dbContext;
        public OrderDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }
    }
}
