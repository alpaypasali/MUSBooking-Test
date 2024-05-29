using Application.Features.Equipments.Commands.Create;
using Application.Features.Equipments.Rules;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Equipments.Commands.Update
{
    public class UpdatedEquipmentCommand : IRequest<UpdatedEquipmentResponse>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }

        public class UpdatedEquipmentCommandHandler : IRequestHandler<UpdatedEquipmentCommand, UpdatedEquipmentResponse>
        {
            private readonly IEquipmentRepository _equipmentRepository;
            private readonly IMapper _mapper;
            private readonly IValidator<UpdatedEquipmentCommand> _validator;
            private readonly EquipmentBusinessRules _equipmentBusinessRules;

            public UpdatedEquipmentCommandHandler(IEquipmentRepository equipmentRepository, IMapper mapper, IValidator<UpdatedEquipmentCommand> validator, EquipmentBusinessRules equipmentBusinessRules)
            {
                _equipmentRepository = equipmentRepository;
                _mapper = mapper;
                _validator = validator;
                _equipmentBusinessRules = equipmentBusinessRules;
            }

            public async Task<UpdatedEquipmentResponse> Handle(UpdatedEquipmentCommand request, CancellationToken cancellationToken)
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                Equipment? equipment = await _equipmentRepository.GetAsync(x => x.Id == request.Id);
                _equipmentBusinessRules.EquipmentIdShouldExistWhenSelected(equipment);

                _mapper.Map(request, equipment);
                await _equipmentBusinessRules.EquipmentNameCanNotBeDuplicatedWhenInserted(equipment.Name);

                Equipment updatedEquipment = await _equipmentRepository.UpdateAsync(equipment);
                UpdatedEquipmentResponse? response = _mapper.Map<UpdatedEquipmentResponse>(updatedEquipment);
                return response;
            }
        }
    }
}
