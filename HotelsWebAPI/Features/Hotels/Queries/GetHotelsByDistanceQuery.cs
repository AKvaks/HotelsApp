using HotelsWebAPI.Features.Hotels.Models;
using HotelsWebAPI.Models;
using HotelsWebAPI.Services.HotelService;
using MediatR;

namespace HotelsWebAPI.Features.Hotels.Queries
{
    public record GetHotelsByDistanceQuery(double Latitude, double Longitude, int? PageNumber, int? PageSize) : IRequest<BaseResponse<PagedResponse<SearchedHotelResponseModel>?>>;

    public class GetHotelsByDistanceQueryHandler : IRequestHandler<GetHotelsByDistanceQuery, BaseResponse<PagedResponse<SearchedHotelResponseModel>?>>
    {
        private readonly IHotelService _hotelService;

        public GetHotelsByDistanceQueryHandler(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task<BaseResponse<PagedResponse<SearchedHotelResponseModel>?>> Handle(GetHotelsByDistanceQuery request, CancellationToken cancellationToken)
        {
            var result = await _hotelService.GetHotelsByDistanceAsync(request.Latitude, request.Longitude, request.PageNumber, request.PageSize, cancellationToken);
            if (result == null) return new BaseResponse<PagedResponse<SearchedHotelResponseModel>?> { StatusCode = 500, Message = "Error during communication with database!" };

            return new BaseResponse<PagedResponse<SearchedHotelResponseModel>?> { Value = result, StatusCode = 200, Message = "Searched hotels retrived successfully!" };
        }
    }
}
