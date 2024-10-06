using HotelsWebAPI.DAL;
using HotelsWebAPI.Features.Hotels.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelsWebAPI.Features.Hotels.Queries
{
    public record GetAllHotelsQuery : IRequest<List<HotelResponseModel>>;

    public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, List<HotelResponseModel>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetAllHotelsQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<HotelResponseModel>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Hotels.Select(x => new HotelResponseModel
            {
                Id = x.Id,
                HotelName = x.HotelName,
                Price = x.Price,
                Latitude = x.Location.X,
                Longitude = x.Location.Y,
            }).ToListAsync();
        }
    }
}
