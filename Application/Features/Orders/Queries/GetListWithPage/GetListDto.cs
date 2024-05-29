using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetListWithPage
{
    public class GetListDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public decimal Price { get; set; }
        public ICollection<OrderEquipmentDto> OrderEquipments { get; set; }

        public class OrderEquipmentDto
        {
            public int EquipmentId { get; set; }
            public string EquipmentName { get; set; }
            public int Quantity { get; set; }
        }
    }
}
