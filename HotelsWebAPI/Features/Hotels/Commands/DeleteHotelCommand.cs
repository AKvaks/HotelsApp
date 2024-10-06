using HotelsWebAPI.Models;
using HotelsWebAPI.Services.HotelService;
using MediatR;

namespace HotelsWebAPI.Features.Hotels.Commands
{
    public record DeleteHotelCommand(int Id) : IRequest<BaseResponse<int>>;

    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, BaseResponse<int>>
    {
        private readonly IHotelService _hotelService;

        public DeleteHotelCommandHandler(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        public async Task<BaseResponse<int>> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            var hotelId = await _hotelService.DeleteHotelAsync(request.Id, cancellationToken);
            if (hotelId == null) return new BaseResponse<int> { StatusCode = 500, Message = "Error during communication with database!" };
            if (hotelId == -1) return new BaseResponse<int> { StatusCode = 404, Message = $"Hotel with Id {request.Id} not found!" };

            return new BaseResponse<int> { Value = hotelId.Value, StatusCode = 200, Message = $"Hotel was successfuly deleted!" };
        }
    }
}
