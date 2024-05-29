using Application.Features.Orders.Rules;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Delete
{   public  class DeletedOrderCommand:IRequest<Unit>
    {
        public int Id { get; set; }

        public class DeletedOrderCommandHandler : IRequestHandler<DeletedOrderCommand, Unit>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IEquipmentRepository _equipmentRepository;
            private readonly IOrderEquipmentRepository _orderEquipmentRepository;
            private readonly OrderBusinessRules _rules;
         

            public DeletedOrderCommandHandler(IOrderRepository orderRepository, IEquipmentRepository equipmentRepository, IOrderEquipmentRepository orderEquipmentRepository, OrderBusinessRules rules)
            {
                _orderRepository = orderRepository;
                _equipmentRepository = equipmentRepository;
                _orderEquipmentRepository = orderEquipmentRepository;
                _rules = rules;
              
            }
            public async Task<Unit> Handle(DeletedOrderCommand request, CancellationToken cancellationToken)
            {
                try
                {
                   
                    Order? order = await _orderRepository.GetAsync(x => x.Id == request.Id);
                    _rules.OrderShouldExistWhenSelected(order);

              
                    List<OrderEquipment> orderEquipments = await _orderEquipmentRepository.GetListAsync(oe => oe.OrderId == order.Id);
                    _rules.OrderEquipmentsShouldExistWhenSelected(orderEquipments);

               
                    foreach (var orderEquipment in orderEquipments)
                    {
                        var equipment = await _equipmentRepository.GetByIdAsync(orderEquipment.EquipmentId);
                        _rules.EquipmentIdShouldExistWhenSelected(equipment);
                        if (equipment != null)
                        {
                            equipment.Amount += orderEquipment.Quantity;
                            await _equipmentRepository.UpdateAsync(equipment);
                        }
                        await _orderEquipmentRepository.DeleteAsync(orderEquipment);
                    }

                    await _orderRepository.DeleteAsync(order);
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
