using HotelsWebAPI.DAL;
using HotelsWebAPI.Utilities;
using MediatR;

namespace HotelsWebAPI.Features.Hotels.Commands
{
    public record EditHotelCommand(int Id, string HotelName, decimal Price, double Latitude, double Longitude) : IRequest<int>;

    public class EditHotelCommandHandler : IRequestHandler<EditHotelCommand, int>
    {
        private readonly ApplicationDbContext _dbContext;

        public EditHotelCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(EditHotelCommand request, CancellationToken cancellationToken)
        {
            var hotelToEdit = await _dbContext.Hotels.FindAsync(request.Id);
            if (hotelToEdit == null)
            {
                return -1;
                // return NotFound
            }

            var location = GeoUtils.CreatePoint(request.Latitude, request.Longitude);

            hotelToEdit.HotelName = request.HotelName;
            hotelToEdit.Price = request.Price;
            hotelToEdit.Location = location;

            _dbContext.Hotels.Update(hotelToEdit);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return hotelToEdit.Id;
        }
    }
}
