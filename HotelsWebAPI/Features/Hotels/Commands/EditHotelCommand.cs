using HotelsWebAPI.Models;
using HotelsWebAPI.Services.HotelService;
using MediatR;

namespace HotelsWebAPI.Features.Hotels.Commands
{
    public record EditHotelCommand(int Id, string HotelName, decimal Price, double Latitude, double Longitude) : IRequest<BaseResponse<int>>;

    public class EditHotelCommandHandler : IRequestHandler<EditHotelCommand, BaseResponse<int>>
    {
        private readonly IHotelService _hotelService;

        public EditHotelCommandHandler(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task<BaseResponse<int>> Handle(EditHotelCommand request, CancellationToken cancellationToken)
        {
            var hotelId = await _hotelService.EditHotelAsync(request.Id, request.HotelName, request.Price, request.Latitude, request.Longitude, cancellationToken);
            if (hotelId == null) return new BaseResponse<int> { StatusCode = 500, Message = "Error during communication with database!" };
            if (hotelId == -1) return new BaseResponse<int> { StatusCode = 404, Message = $"Hotel with Id {request.Id} not found!" };

            return new BaseResponse<int> { StatusCode = 200, Message = $"Hotel was successfuly updated!" };
        }
    }
}
