using HotelsWebAPI.Models;
using HotelsWebAPI.Services.HotelService;
using MediatR;

namespace HotelsWebAPI.Features.Hotels.Commands
{
    public record AddHotelCommand(string HotelName, decimal Price, double Latitude, double Longitude) : IRequest<BaseResponse<int>>;

    public class AddHotelCommandHandler : IRequestHandler<AddHotelCommand, BaseResponse<int>>
    {
        private readonly IHotelService _hotelService;

        public AddHotelCommandHandler(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task<BaseResponse<int>> Handle(AddHotelCommand request, CancellationToken cancellationToken)
        {
            var hotelId = await _hotelService.AddHotelAsync(request.HotelName, request.Price, request.Latitude, request.Longitude, cancellationToken);
            if (hotelId == null) return new BaseResponse<int> { StatusCode = 500, Message = "Error during communication with database!" };

            return new BaseResponse<int> { Value = hotelId.Value, StatusCode = 201, Message = $"Hotel was successfuly added! HotelId is {hotelId.Value}." };
        }
    }
}
