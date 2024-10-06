using FluentValidation;
using HotelsWebAPI.Features.Hotels.Commands;

namespace HotelsWebAPI.Features.Hotels.Validators
{
    public class EditHotelCommandValidator : AbstractValidator<EditHotelCommand>
    {
        public EditHotelCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.HotelName).NotEmpty();
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        }
    }
}