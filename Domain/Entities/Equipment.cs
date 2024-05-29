using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Equipment
    {
        private int _amount;

        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount
        {
            get => _amount;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "Amount cannot be less than 0.");
                }
                _amount = value;
            }
        }
        public decimal Price { get; set; }

        public virtual ICollection<OrderEquipment> OrderEquipments { get; set; }

        public Equipment()
        {
            OrderEquipments = new HashSet<OrderEquipment>();
        }

        public Equipment(int id, string name, int amount, decimal price)
        {
            Id = id;
            Name = name;
            Amount = amount;
            Price = price;
        }
    }
}
