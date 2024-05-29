using Application.Features.Orders.Rules;
using Application.Services;
using Application.Services.EquipmentServices;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Update
{
    public class UpdateOrderCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public List<EquipmentOrderUpdateDto> Equipments { get; set; } = new List<EquipmentOrderUpdateDto>();

        public class EquipmentOrderUpdateDto
        {
            public int EquipmentId { get; set; }
            public int Quantity { get; set; }
        }

        public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Unit>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IEquipmentService _equipmentService;
            private readonly OrderBusinessRules _orderBusinessRules;

            public UpdateOrderCommandHandler(IOrderRepository orderRepository,IEquipmentService equipmentService, OrderBusinessRules orderBusinessRules)
            {
                _orderRepository = orderRepository;
                _equipmentService = equipmentService;
                _orderBusinessRules = orderBusinessRules;
            }

            public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
            {
          
                var existingOrder = await _orderRepository.GetAsync(
                    o => o.Id == request.Id,
                    queryable => queryable.Include(o => o.OrderEquipments).ThenInclude(oe => oe.Equipment)
                );

             
                _orderBusinessRules.OrderShouldExistWhenSelected(existingOrder);

          
                existingOrder.Description = request.Description;
                existingOrder.UpdatedAt = DateTime.UtcNow;

                // Обновление существующих связей OrderEquipments
                foreach (var existingOrderEquipment in existingOrder.OrderEquipments.ToList())
                {
                    var equipmentDto = request.Equipments.FirstOrDefault(e => e.EquipmentId == existingOrderEquipment.EquipmentId);
                    if (equipmentDto != null)
                    {
                        if (equipmentDto.Quantity != existingOrderEquipment.Quantity)
                        {
                         
                            int quantityDifference = equipmentDto.Quantity - existingOrderEquipment.Quantity;

                            if (quantityDifference > 0)
                            {
                               
                                await _equipmentService.DecreaseEquipmentQuantityAsync(existingOrderEquipment.EquipmentId, quantityDifference);
                            }
                            else
                            {
                              
                                await _equipmentService.IncreaseEquipmentQuantityAsync(existingOrderEquipment.EquipmentId, -quantityDifference);
                            }

                           
                            existingOrderEquipment.Quantity = equipmentDto.Quantity;
                        }
                    }
                    else
                    {
                        // Если оборудование удалено, добавить его на склад и удалить связь
                        await _equipmentService.IncreaseEquipmentQuantityAsync(existingOrderEquipment.EquipmentId, existingOrderEquipment.Quantity);
                        existingOrder.OrderEquipments.Remove(existingOrderEquipment);
                    }
                }

                // Добавление новых связей OrderEquipments
                foreach (var equipmentDto in request.Equipments)
                {
                    if (!existingOrder.OrderEquipments.Any(oe => oe.EquipmentId == equipmentDto.EquipmentId))
                    {
                     
                        await _equipmentService.DecreaseEquipmentQuantityAsync(equipmentDto.EquipmentId, equipmentDto.Quantity);

                        var orderEquipment = new OrderEquipment
                        {
                            OrderId = existingOrder.Id,
                            EquipmentId = equipmentDto.EquipmentId,
                            Quantity = equipmentDto.Quantity
                        };
                        existingOrder.OrderEquipments.Add(orderEquipment);
                    }
                }

        
                existingOrder.Price = existingOrder.OrderEquipments.Sum(oe => oe.Equipment.Price * oe.Quantity);

              
                await _orderRepository.UpdateAsync(existingOrder);

                return Unit.Value;
            }
        }
    }
}
