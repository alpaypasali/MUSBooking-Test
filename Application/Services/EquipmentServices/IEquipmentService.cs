using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.EquipmentServices
{
    public interface IEquipmentService
    {
        Task DecreaseEquipmentQuantityAsync(int equipmentId, int quantity);
        Task IncreaseEquipmentQuantityAsync(int equipmentId, int quantity);
    }

}
