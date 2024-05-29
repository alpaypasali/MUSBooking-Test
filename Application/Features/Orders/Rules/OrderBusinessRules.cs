using Application.Features.Orders.Commands.Create;
using Application.Services;
using Domain.Entities;
using Infrastructure.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Rules
{
    public class OrderBusinessRules
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEquipmentRepository _equipmentRepository;

        public OrderBusinessRules(IOrderRepository orderRepository, IEquipmentRepository equipmentRepository)
        {
            _orderRepository = orderRepository;
            _equipmentRepository = equipmentRepository;
        }

        public async Task CheckEquipmentAmount(List<CreatedOrderCommand.OrderEquipmentDto> orderEquipments)
        {
            foreach (var orderEquipmentDto in orderEquipments)
            {
                var equipment = await _equipmentRepository.GetByIdAsync(orderEquipmentDto.EquipmentId);
                EquipmentIdShouldExistWhenSelected(equipment);
                if (equipment.Amount < orderEquipmentDto.Quantity)
                {
                    throw new Exception($"Not enough stock available for equipment with ID {orderEquipmentDto.EquipmentId}.");
                }
            }
        }

        public void EquipmentIdShouldExistWhenSelected(Equipment? equipment)
        {
            if (equipment == null)
                throw new BusinessException("Equipment not exists.");
        }

        public void OrderShouldExistWhenSelected(Order? order)
        {
            if (order == null)
                throw new BusinessException("Order not exists.");
        }
        public void OrderEquipmentsShouldExistWhenSelected(List<OrderEquipment>? orderEquipment)
        {
            if (orderEquipment.Count == 0)
                throw new BusinessException("OrderEquipment not exists.");
        }

    }
}
