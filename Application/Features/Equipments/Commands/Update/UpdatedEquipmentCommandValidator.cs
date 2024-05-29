using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Equipments.Commands.Update
{
    public class UpdatedEquipmentCommandValidator : AbstractValidator<UpdatedEquipmentCommand>
    {
        public UpdatedEquipmentCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Amount).GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal to 0.");
            RuleFor(x => x.Price).GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.");
        }
    }
}
