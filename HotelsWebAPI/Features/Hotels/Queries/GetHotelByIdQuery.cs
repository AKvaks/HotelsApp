using HotelsWebAPI.Features.Hotels.Models;
using HotelsWebAPI.Models;
using HotelsWebAPI.Services.HotelService;
using MediatR;

namespace HotelsWebAPI.Features.Hotels.Queries
{
    public record GetHotelByIdQuery(int Id) : IRequest<BaseResponse<HotelResponseModel>>;

    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, BaseResponse<HotelResponseModel>>
    {
        private readonly IHotelService _hotelService;

        public GetHotelByIdQueryHandler(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task<BaseResponse<HotelResponseModel>> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _hotelService.GetHotelByIdAsync(request.Id, cancellationToken);

            if (result == null) return new BaseResponse<HotelResponseModel> { StatusCode = 500, Message = "Error during communication with database!" };
            if (result.Id == -1) return new BaseResponse<HotelResponseModel> { StatusCode = 404, Message = $"Hotel with Id {request.Id} not found!" };

            return new BaseResponse<HotelResponseModel> { Value = result, StatusCode = 200, Message = "Hotel retrived successfully!" };
        }
    }
}
