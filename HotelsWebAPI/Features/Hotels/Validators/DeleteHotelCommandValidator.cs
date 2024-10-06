using FluentValidation;
using HotelsWebAPI.Features.Hotels.Commands;

namespace HotelsWebAPI.Features.Hotels.Validators
{
    public class DeleteHotelCommandValidator : AbstractValidator<DeleteHotelCommand>
    {
        public DeleteHotelCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        }
    }
}