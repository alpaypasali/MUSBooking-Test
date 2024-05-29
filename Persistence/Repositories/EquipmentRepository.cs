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
    public class EquipmentRepository : GenericRepository<BaseDbContext, Equipment>, IEquipmentRepository
    {
        public EquipmentRepository(BaseDbContext context) : base(context)
        {
        }
    }
}
