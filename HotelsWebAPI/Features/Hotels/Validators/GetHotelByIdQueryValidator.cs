using FluentValidation;
using HotelsWebAPI.Features.Hotels.Queries;

namespace HotelsWebAPI.Features.Hotels.Validators
{
    public class GetHotelByIdQueryValidator : AbstractValidator<GetHotelByIdQuery>
    {
        public GetHotelByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        }
    }
}
