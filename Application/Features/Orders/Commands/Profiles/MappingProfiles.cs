using Application.Features.Orders.Queries.GetListWithPage;
using AutoMapper;
using Domain.Entities;
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
            CreateMap<Order, GetListOrderListItemDto>()
                .ForMember(dest => dest.Equipments, opt => opt.MapFrom(src => src.OrderEquipments.Select(oe => oe.Equipment).ToList())).ReverseMap();
        }
    }
}
