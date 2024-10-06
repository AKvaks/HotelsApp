using HotelsWebAPI.DAL;
using MediatR;

namespace HotelsWebAPI.Features.Hotels.Commands
{
    public record DeleteHotelCommand(int Id) : IRequest<int>;

    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, int>
    {
        private readonly ApplicationDbContext _dbContext;

        public DeleteHotelCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            var hotelToDelete = await _dbContext.Hotels.FindAsync(request.Id);
            if (hotelToDelete == null)
            {
                return -1;
                // return NotFound
            }

            _dbContext.Hotels.Remove(hotelToDelete);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return hotelToDelete.Id;
        }
    }
}
