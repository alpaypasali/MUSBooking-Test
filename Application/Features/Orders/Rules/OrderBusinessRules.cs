using Application.Services;
using Domain.Entities;
using Infrastructure.Exceptions;
using Infrastructure.NewFolder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Rules
{
    public class OrderBusinessRules:BaseBusinessRules
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEquipmentRepository _equipmentRepository;

        public OrderBusinessRules(IOrderRepository orderRepository, IEquipmentRepository equipmentRepository)
        {
            _orderRepository = orderRepository;
            _equipmentRepository = equipmentRepository;
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
