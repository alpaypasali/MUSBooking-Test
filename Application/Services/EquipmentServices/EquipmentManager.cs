using Domain.Entities;
using Infrastructure.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.EquipmentServices
{
    public class EquipmentManager : IEquipmentService
    {
        private readonly IEquipmentRepository _equipmentRepository;

        public EquipmentManager(IEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        public async Task DecreaseEquipmentQuantityAsync(int equipmentId, int quantity)
        {
            var equipment = await GetEquipmentByIdAsync(equipmentId);
            CheckIfSufficientStock(equipment, quantity);

            equipment.Amount -= quantity;

            await _equipmentRepository.UpdateAsync(equipment);
        }

        public async Task IncreaseEquipmentQuantityAsync(int equipmentId, int quantity)
        {
            var equipment = await GetEquipmentByIdAsync(equipmentId);

            equipment.Amount += quantity;

            await _equipmentRepository.UpdateAsync(equipment);
        }


        private async Task<Equipment> GetEquipmentByIdAsync(int equipmentId)
        {
            var equipment = await _equipmentRepository.GetByIdAsync(equipmentId);
            if (equipment == null)
            {
                throw new BusinessException($"Equipment with ID {equipmentId} not found.");
            }

            return equipment;
        }

        private void CheckIfSufficientStock(Equipment equipment, int quantity)
        {
            if (equipment.Amount < quantity)
            {
                throw new BusinessException("Insufficient stock.");
            }
        }
    }
}
