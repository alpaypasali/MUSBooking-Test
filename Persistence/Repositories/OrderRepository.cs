using Application.Services;
using Domain.Entities;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class OrderRepository : GenericRepository<BaseDbContext, Order>, IOrderRepository
    {
        public OrderRepository(BaseDbContext context) : base(context)
        {
        }
    }
}
