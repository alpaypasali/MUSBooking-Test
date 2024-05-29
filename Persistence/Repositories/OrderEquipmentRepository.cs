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
    public class OrderEquipmentRepository : GenericRepository<BaseDbContext, OrderEquipment>, IOrderEquipmentRepository
    {
        public OrderEquipmentRepository(BaseDbContext context) : base(context)
        {
        }
    }
}
