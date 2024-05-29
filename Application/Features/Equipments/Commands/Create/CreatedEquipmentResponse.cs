using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Equipments.Commands.Create
{
    public class CreatedEquipmentResponse
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}
