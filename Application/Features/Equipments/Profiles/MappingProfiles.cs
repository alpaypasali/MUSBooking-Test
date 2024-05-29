using Application.Features.Equipments.Commands.Create;
using Application.Features.Equipments.Commands.Update;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Equipments.Profiles
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            CreateMap<Equipment, CreatedEquipmentResponse>().ReverseMap();
            CreateMap<Equipment, CreatedEquipmentCommand>().ReverseMap();

            CreateMap<Equipment, UpdatedEquipmentResponse>().ReverseMap();
            CreateMap<Equipment, UpdatedEquipmentCommand>().ReverseMap();
        }
    }
}
