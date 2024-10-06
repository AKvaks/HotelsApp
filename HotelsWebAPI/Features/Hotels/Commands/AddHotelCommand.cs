using HotelsWebAPI.DAL;
using HotelsWebAPI.Entities;
using HotelsWebAPI.Utilities;
using MediatR;

namespace HotelsWebAPI.Features.Hotels.Commands
{
    public record AddHotelCommand(string HotelName, decimal Price, double Latitude, double Longitude) : IRequest<int>;

    public class AddHotelCommandHandler : IRequestHandler<AddHotelCommand, int>
    {
        private readonly ApplicationDbContext _dbContext;

        public AddHotelCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(AddHotelCommand request, CancellationToken cancellationToken)
        {
            var location = GeoUtils.CreatePoint(request.Latitude, request.Longitude);

            var hotel = new Hotel
            {
                HotelName = request.HotelName,
                Price = request.Price,
                Location = location
            };

            _dbContext.Hotels.Add(hotel);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return hotel.Id;
        }
    }
}
