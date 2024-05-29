
using Application.Features.Orders.Queries.GetListWithPage;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Orders.Commands.Profiles
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Order, GetListDto>()
                 .ForMember(dest => dest.OrderEquipments, opt => opt.MapFrom(src => src.OrderEquipments))
                 .ReverseMap();

            CreateMap<OrderEquipment, GetListDto.OrderEquipmentDto>()
                .ForMember(dest => dest.EquipmentName, opt => opt.MapFrom(src => src.Equipment.Name))
                .ReverseMap();

            CreateMap<Paginate<Order>, GetListResponse<GetListDto>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        }
    }
}
