using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Paging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Queries.GetListWithPage
{
    public class GetListOrderQuery:IRequest<GetListResponse<GetListDto>>
    {
        public PageRequest PageRequest {  get; set; }

        public class GetlistOrderQueryHandler : IRequestHandler<GetListOrderQuery, GetListResponse<GetListDto>>
        {
            private readonly IOrderRepository _orderRepository;
            private readonly IMapper _mapper;

            public GetlistOrderQueryHandler(IOrderRepository orderRepository, IMapper mapper)
            {
                _orderRepository = orderRepository;
                _mapper = mapper;
            }

            public async Task<GetListResponse<GetListDto>> Handle(GetListOrderQuery request, CancellationToken cancellationToken)
            {
                Paginate<Order> orders = await _orderRepository.GetListAsync(
                  include: queryable => queryable.Include(o => o.OrderEquipments).ThenInclude(oe => oe.Equipment),
                 index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                
                cancellationToken:cancellationToken,
                 orderBy: queryable => queryable.OrderBy(o => o.CreatedAt));

                var mappedBrandListModel = _mapper.Map<GetListResponse<GetListDto>>(orders);

                return mappedBrandListModel;

            }
        }
    }
}
