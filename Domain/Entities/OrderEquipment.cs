using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderEquipment
    {
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int EquipmentId { get; set; }
        public Equipment Equipment { get; set; }
        public int Quantity { get; set; }

        public OrderEquipment()
        {
            
        }
        public OrderEquipment(int orderId , int equipmentId , int quantity)
        {

            OrderId = orderId;
            EquipmentId = equipmentId;
            Quantity = quantity;
        }
    }
}
