using Application.Features.Orders.Queries.GetListWithPage;
using Application.Services;
using Infrastructure.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;



public class GetListOrderQuery : IRequest<PaginatedList<GetListOrderListItemDto>>
{
    public int PageNumber { get; }
    public int PageSize { get; }

    public GetListOrderQuery(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
    }
    public class GetListOrderQueryHandler : IRequestHandler<GetListOrderQuery, PaginatedList<GetListOrderListItemDto>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetListOrderQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<PaginatedList<GetListOrderListItemDto>> Handle(GetListOrderQuery request, CancellationToken cancellationToken)
        {
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            var orders = await _orderRepository.GetListAsync(

                include: query => query.Include(o => o.OrderEquipments).ThenInclude(oe => oe.Equipment),
                enableTracking: false
            );
            var orders2 = orders.OrderBy(x => x.CreatedAt);

            var paginatedOrders = await PaginatedList<GetListOrderListItemDto>.CreateAsync(
      orders2.AsQueryable().Select(order => new GetListOrderListItemDto
      {
          Id = order.Id,
          Description = order.Description,
          CreatedAt = order.CreatedAt,
          UpdatedAt = order.UpdatedAt,
          Price = order.Price,
          Equipments = order.OrderEquipments.Select(oe => oe.Equipment).Select(e => new EquipmentDto
          {
              Id = e.Id,
              Name = e.Name,
              Amount = e.Amount,
              Price = e.Price
          }).ToList()
      }),
      pageNumber,
      pageSize
  );

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };


            var json = JsonSerializer.Serialize(paginatedOrders, options);


            return paginatedOrders;
        }

    }
}


