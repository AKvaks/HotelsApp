using HotelsWebAPI.DAL;
using HotelsWebAPI.Features.Hotels.Models;
using MediatR;

namespace HotelsWebAPI.Features.Hotels.Queries
{
    public record GetHotelByIdQuery(int Id) : IRequest<HotelResponseModel?>;

    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, HotelResponseModel?>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetHotelByIdQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HotelResponseModel?> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _dbContext.Hotels.FindAsync(request.Id);

            if (result == null) return null;

            return new HotelResponseModel
            {
                Id = result.Id,
                HotelName = result.HotelName,
                Price = result.Price,
                Latitude = result.Location.X,
                Longitude = result.Location.Y,
            };
        }
    }
}
