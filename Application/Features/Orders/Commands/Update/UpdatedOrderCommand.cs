using Application.Features.Orders.Rules;
using Application.Services;
using Domain.Entities;
using Infrastructure.NewFolder;
using MediatR;



namespace Application.Features.Orders.Commands.Update;
public class UpdatedOrderCommand : IRequest<Unit>
{
    public int Id { get; set; }
    public List<OrderEquipmentDto> OrderEquipments { get; set; }

    public class OrderEquipmentDto
    {
        public int EquipmentId { get; set; }
        public int Quantity { get; set; }
    }


    public class UpdatedOrderCommandHandler : IRequestHandler<UpdatedOrderCommand, Unit>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IOrderEquipmentRepository _orderEquipmentRepository;
        private readonly OrderBusinessRules _rules;

        public UpdatedOrderCommandHandler(IOrderRepository orderRepository, IEquipmentRepository equipmentRepository, IOrderEquipmentRepository orderEquipmentRepository, OrderBusinessRules rules)
        {
            _orderRepository = orderRepository;
            _equipmentRepository = equipmentRepository;
            _orderEquipmentRepository = orderEquipmentRepository;
            _rules = rules;
        }

        public async Task<Unit> Handle(UpdatedOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _orderRepository.GetAsync(x => x.Id == request.Id);
                _rules.OrderShouldExistWhenSelected(order);

                var totalPriceChange = CalculateTotalPriceChange(order, request.OrderEquipments);

                await UpdateOrderEquipments(order, request.OrderEquipments);
                await UpdateEquipmentAmount(request.OrderEquipments);

                order.Price += totalPriceChange;
                order.UpdatedAt = DateTime.UtcNow;

                await _orderRepository.UpdateAsync(order);

                return Unit.Value;
            }
          
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        private decimal CalculateTotalPriceChange(Order order, List<OrderEquipmentDto> orderEquipments)
        {
            decimal totalPriceChange = 0;

            foreach (var orderEquipmentDto in orderEquipments)
            {
                var existingOrderEquipment = order.OrderEquipments.FirstOrDefault(oe => oe.EquipmentId == orderEquipmentDto.EquipmentId);

                if (existingOrderEquipment != null)
                {
                    int quantityChange = orderEquipmentDto.Quantity - existingOrderEquipment.Quantity;
                    var equipment = _equipmentRepository.GetByIdAsync(orderEquipmentDto.EquipmentId).Result;

                    totalPriceChange += equipment.Price * quantityChange;
                }
                else
                {
                    var equipment = _equipmentRepository.GetByIdAsync(orderEquipmentDto.EquipmentId).Result;
                    totalPriceChange += equipment.Price * orderEquipmentDto.Quantity;
                }
            }

            return totalPriceChange;
        }

        private async Task UpdateOrderEquipments(Order order, List<OrderEquipmentDto> orderEquipments)
        {
            foreach (var orderEquipmentDto in orderEquipments)
            {
                var existingOrderEquipment = order.OrderEquipments.FirstOrDefault(oe => oe.EquipmentId == orderEquipmentDto.EquipmentId);

                if (existingOrderEquipment != null)
                {
                    existingOrderEquipment.Quantity = orderEquipmentDto.Quantity;
                    await _orderEquipmentRepository.UpdateAsync(existingOrderEquipment);
                }
                else
                {
                    var newOrderEquipment = new OrderEquipment
                    {
                        EquipmentId = orderEquipmentDto.EquipmentId,
                        OrderId = order.Id,
                        Quantity = orderEquipmentDto.Quantity,
                    };
                    await _orderEquipmentRepository.AddAsync(newOrderEquipment);
                }
            }
        }

        private async Task UpdateEquipmentAmount(List<OrderEquipmentDto> orderEquipments)
        {
            foreach (var orderEquipmentDto in orderEquipments)
            {
                var equipment = await _equipmentRepository.GetByIdAsync(orderEquipmentDto.EquipmentId);
                equipment.Amount -= orderEquipmentDto.Quantity;
                await _equipmentRepository.UpdateAsync(equipment);
            }
        }
    }
}
