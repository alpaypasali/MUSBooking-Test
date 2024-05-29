using Application.Features.Orders.Rules;
using Application.Services;
using Application.Services.EquipmentServices;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Delete
{
    public class DeleteOrderCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public class DeleteCommandHandler : IRequestHandler<DeleteOrderCommand, Unit>
        {

            private readonly IOrderRepository _orderRepository;

            private readonly IEquipmentService _equipmentService;
            private readonly OrderBusinessRules _orderBusinessRules;

            public DeleteCommandHandler(IOrderRepository orderRepository, IEquipmentService equipmentService, OrderBusinessRules orderBusinessRules)
            {
                _orderRepository = orderRepository;

                _equipmentService = equipmentService;
                _orderBusinessRules = orderBusinessRules;
            }

            public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
            {
                var order = await _orderRepository.GetAsync(
              o => o.Id == request.Id,
              queryable => queryable.Include(o => o.OrderEquipments).ThenInclude(oe => oe.Equipment)
                     );

                foreach (var equipmentDto in order.OrderEquipments)
                {
                    await _equipmentService.IncreaseEquipmentQuantityAsync(equipmentDto.EquipmentId, equipmentDto.Quantity);
                }

                await _orderRepository.DeleteAsync(order);

                return Unit.Value;
            }
        }
    }
}
