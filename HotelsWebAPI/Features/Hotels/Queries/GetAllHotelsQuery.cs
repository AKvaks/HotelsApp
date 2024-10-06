using HotelsWebAPI.Features.Hotels.Models;
using HotelsWebAPI.Models;
using HotelsWebAPI.Services.HotelService;
using MediatR;

namespace HotelsWebAPI.Features.Hotels.Queries
{
    public record GetAllHotelsQuery : IRequest<BaseResponse<List<HotelResponseModel>>>;

    public class GetAllHotelsQueryHandler : IRequestHandler<GetAllHotelsQuery, BaseResponse<List<HotelResponseModel>>>
    {
        private readonly IHotelService _hotelService;

        public GetAllHotelsQueryHandler(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task<BaseResponse<List<HotelResponseModel>>> Handle(GetAllHotelsQuery request, CancellationToken cancellationToken)
        {
            var result = await _hotelService.GetAllHotelsAsync(cancellationToken);
            if (result == null) return new BaseResponse<List<HotelResponseModel>> { StatusCode = 500, Message = "Error during communication with database!" };

            return new BaseResponse<List<HotelResponseModel>> { Value = result, StatusCode = 200, Message = "Hotel list successfully retrieved." };
        }
    }
}
