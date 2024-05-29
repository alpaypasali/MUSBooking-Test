using Application.Features.Orders.Rules;
using Application.Services;
using Application.Services.EquipmentServices;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Create
{
    public class CreateOrderCommand :IRequest<Unit>
    {
        public string Description { get; set; }
        public List<EquipmentOrderDto> Equipments { get; set; } = new List<EquipmentOrderDto>();
        public class EquipmentOrderDto
        {
            public int EquipmentId { get; set; }
            public int Quantity { get; set; }
        }

        public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Unit>
        {

            private readonly IOrderRepository _orderRepository;
            private readonly OrderBusinessRules _orderBusinessRules;
            private readonly IEquipmentRepository _equipmentRepository;
            private readonly IEquipmentService _equipmentService;

            public CreateOrderCommandHandler(IOrderRepository orderRepository, OrderBusinessRules orderBusinessRules, IEquipmentRepository equipmentRepository, IEquipmentService equipmentService)
            {
                _orderRepository = orderRepository;
                _orderBusinessRules = orderBusinessRules;
                _equipmentRepository = equipmentRepository;
                _equipmentService = equipmentService;
            }

            public async Task<Unit> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
            {
                Order order = new Order
                {

                    Description = request.Description,
                    CreatedAt = DateTime.UtcNow,
                };
                foreach (var equipmentDto in request.Equipments)
                {
                    Equipment? equipment =await _equipmentRepository.GetByIdAsync(equipmentDto.EquipmentId);
                    _orderBusinessRules.EquipmentIdShouldExistWhenSelected(equipment);
                  
                   await _equipmentService.DecreaseEquipmentQuantityAsync(equipmentDto.EquipmentId , equipmentDto.Quantity);
                    order.Price += equipment.Price * equipmentDto.Quantity;

                    var orderEquipment = new OrderEquipment
                    {
                        OrderId = order.Id, 
                        EquipmentId = equipmentDto.EquipmentId,
                        Quantity = equipmentDto.Quantity,
                        Equipment = equipment,
                        Order = order
                    };

                    order.OrderEquipments.Add(orderEquipment);
                }

                   await _orderRepository.AddAsync(order);


                return Unit.Value;

            }
        }
    }
}
