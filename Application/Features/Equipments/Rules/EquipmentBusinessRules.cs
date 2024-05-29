using Application.Services;
using Domain.Entities;
using Infrastructure.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Equipments.Rules
{
    public class EquipmentBusinessRules
    {
        private readonly IEquipmentRepository _equipmentRepository;

        public EquipmentBusinessRules(IEquipmentRepository equipmentRepository)
        {
            _equipmentRepository = equipmentRepository;
        }

        public async Task EquipmentNameCanNotBeDuplicatedWhenInserted(string name)
        {
            var result = await _equipmentRepository.GetAsync(x => x.Name.ToLower() == name.ToLower());
            if (result != null)
                throw new BusinessException("An equipment with the same name already exists.");
        }

        public void EquipmentIdShouldExistWhenSelected(Equipment? equipment)
        {
            if (equipment == null)
                throw new BusinessException("Equipment not exists.");
        }
    }
}
