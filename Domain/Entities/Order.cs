using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public decimal Price { get; set; }

     
        public virtual ICollection<OrderEquipment> OrderEquipments { get; set; } = new List<OrderEquipment>();
     

        public Order()
        {
            OrderEquipments = new HashSet<OrderEquipment>();
          
          
        }

        public Order(int id , string description )
        {
            Id = id;    
            Description = description;  
     
            
        }
       
    }
}
