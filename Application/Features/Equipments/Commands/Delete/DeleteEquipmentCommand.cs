using Application.Features.Equipments.Commands.Create;
using Application.Features.Equipments.Rules;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Equipments.Commands.Delete
{
    public class DeleteEquipmentCommand:IRequest<Unit>
    {
        public int Id { get; set; }

        public class DeleteEquipmentCommandHandler : IRequestHandler<DeleteEquipmentCommand, Unit>
        {
            private readonly IEquipmentRepository _equipmentRepository;
            private readonly IMapper _mapper;
         
            private readonly EquipmentBusinessRules _equipmentBusinessRules;

            public DeleteEquipmentCommandHandler(IEquipmentRepository equipmentRepository, IMapper mapper, EquipmentBusinessRules equipmentBusinessRules)
            {
                _equipmentRepository = equipmentRepository;
                _mapper = mapper;
           
                _equipmentBusinessRules = equipmentBusinessRules;
            }

            public async Task<Unit> Handle(DeleteEquipmentCommand request, CancellationToken cancellationToken)
            {
                
                Equipment? equipment = await _equipmentRepository.GetAsync(x=>x.Id == request.Id);
               _equipmentBusinessRules.EquipmentIdShouldExistWhenSelected(equipment);

                await _equipmentRepository.DeleteAsync(equipment);

                return Unit.Value;

            }
        }
    }
}
