using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetListWithPage
{
    public class GetListOrderListItemDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public decimal Price { get; set; }
        public List<EquipmentDto> Equipments { get; set; }
    }
    public class EquipmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}
