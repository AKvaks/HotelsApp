using HotelsWebAPI.DAL;
using HotelsWebAPI.Features.Hotels.Models;
using HotelsWebAPI.Utilities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace HotelsWebAPI.Features.Hotels.Queries
{
    public record GetHotelsByDistanceQuery(double Latitude, double Longitude, int PageNumber, int PageSize) : IRequest<List<SearchedHotelResponseModel>>;

    public class GetHotelsByDistanceQueryHandler : IRequestHandler<GetHotelsByDistanceQuery, List<SearchedHotelResponseModel>>
    {
        private readonly ApplicationDbContext _dbContext;

        public GetHotelsByDistanceQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SearchedHotelResponseModel>> Handle(GetHotelsByDistanceQuery request, CancellationToken cancellationToken)
        {
            var currentLocation = GeoUtils.CreatePoint(request.Latitude, request.Longitude);

            var pageNumber = request.PageNumber;
            if (pageNumber < 1) pageNumber = 1;

            var pageSize = request.PageSize;
            if (pageSize <= 0) pageSize = 10;

            var hotels = await _dbContext.Hotels
                .Select(h => new SearchedHotelResponseModel
                {
                    Id = h.Id,
                    HotelName = h.HotelName,
                    Price = h.Price,
                    Distance = h.Location.Distance(currentLocation)
                })
                .OrderBy(h => h.Price)
                .ThenBy(h => h.Distance)
                .ToListAsync(cancellationToken);

            return hotels;
        }
    }
}
