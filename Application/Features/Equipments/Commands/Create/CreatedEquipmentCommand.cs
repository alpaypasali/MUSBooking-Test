using Application.Features.Equipments.Rules;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Equipments.Commands.Create
{
    public class CreatedEquipmentCommand: IRequest<CreatedEquipmentResponse>
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }


        public class CreatedEquipmentCommandHandler : IRequestHandler<CreatedEquipmentCommand, CreatedEquipmentResponse>
        {
            private readonly IEquipmentRepository _equipmentRepository;
            private readonly IMapper _mapper;
   
            private readonly EquipmentBusinessRules _equipmentBusinessRules;

            public CreatedEquipmentCommandHandler(IEquipmentRepository equipmentRepository, IMapper mapper, EquipmentBusinessRules equipmentBusinessRules)
            {
                _equipmentRepository = equipmentRepository;
                _mapper = mapper;
                _equipmentBusinessRules = equipmentBusinessRules;
            }

            public async Task<CreatedEquipmentResponse> Handle(CreatedEquipmentCommand request, CancellationToken cancellationToken)
            {
              
                await _equipmentBusinessRules.EquipmentNameCanNotBeDuplicatedWhenInserted(request.Name);

                Equipment equipment = _mapper.Map<Equipment>(request);

                await _equipmentRepository.AddAsync(equipment);
                CreatedEquipmentResponse response = _mapper.Map<CreatedEquipmentResponse>(equipment);
                return response;
            }
        }
    }
}
