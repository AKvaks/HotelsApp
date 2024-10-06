using FluentValidation;
using HotelsWebAPI.Features.Hotels.Queries;

namespace HotelsWebAPI.Features.Hotels.Validators
{
    public class GetHotelsByDistanceQueryValidator : AbstractValidator<GetHotelsByDistanceQuery>
    {
        public GetHotelsByDistanceQueryValidator()
        {
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
        }
    }
}