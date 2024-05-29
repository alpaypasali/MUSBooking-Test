using Application.Features.Orders.Rules;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Create
{
    public class CreatedOrderCommand : IRequest<Unit>
    {
        public string Description { get; set; }
        public List<OrderEquipmentDto> OrderEquipments { get; set; }

        public class OrderEquipmentDto
        {
            public int EquipmentId { get; set; }
            public int Quantity { get; set; }
        }

        public class CreatedOrderCommandHandler : IRequestHandler<CreatedOrderCommand, Unit>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IEquipmentRepository _equipmentRepository;
            private readonly IOrderEquipmentRepository _orderEquipmentRepository;
            private readonly OrderBusinessRules _rules;
            private readonly IMapper _mapper;

            public CreatedOrderCommandHandler(IOrderRepository orderRepository, IEquipmentRepository equipmentRepository, IMapper mapper, IOrderEquipmentRepository orderEquipmentRepository, OrderBusinessRules rules)
            {
                _orderRepository = orderRepository;
                _equipmentRepository = equipmentRepository;
                _mapper = mapper;
                _orderEquipmentRepository = orderEquipmentRepository;
                _rules = rules;
            }

            public async Task<Unit> Handle(CreatedOrderCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var order = new Order
                    {
                        Description = request.Description,
                        CreatedAt = DateTime.UtcNow,
                        Price = 0
                    };
                    await _rules.CheckEquipmentAmount(request.OrderEquipments);
                    foreach (var orderEquipmentDto in request.OrderEquipments)
                    {
                        var equipment = await _equipmentRepository.GetByIdAsync(orderEquipmentDto.EquipmentId);
                        order.Price += equipment.Price * orderEquipmentDto.Quantity;
                        if (equipment != null)
                        {
                            equipment.Amount -= orderEquipmentDto.Quantity;
                            await _equipmentRepository.UpdateAsync(equipment);
                        }

                    }

                    Order createdOrder = await _orderRepository.AddAsync(order);

                    foreach (var orderEquipmentDto in request.OrderEquipments)
                    {
                        var orderEquipment = new OrderEquipment
                        {
                            EquipmentId = orderEquipmentDto.EquipmentId,
                            OrderId = createdOrder.Id,
                            Quantity = orderEquipmentDto.Quantity,
                        };

                        await _orderEquipmentRepository.AddAsync(orderEquipment);
                    }
                }
                catch (Exception ex)
                {
              
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }

                return Unit.Value;
            }
        }
    }
}
