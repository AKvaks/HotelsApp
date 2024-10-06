using HotelsWebAPI.Features.Hotels.Models;
using HotelsWebAPI.Models;

namespace HotelsWebAPI.Services.HotelService
{
    public interface IHotelService
    {
        Task<int?> AddHotelAsync(string HotelName, decimal Price, double Latitude, double Longitude, CancellationToken CancellationToken);
        Task<int?> EditHotelAsync(int Id, string HotelName, decimal Price, double Latitude, double Longitude, CancellationToken CancellationToken);
        Task<int?> DeleteHotelAsync(int Id, CancellationToken CancellationToken);
        Task<HotelResponseModel?> GetHotelByIdAsync(int Id, CancellationToken CancellationToken);
        Task<List<HotelResponseModel>?> GetAllHotelsAsync(CancellationToken CancellationToken);
        Task<PagedResponse<SearchedHotelResponseModel>?> GetHotelsByDistanceAsync(double Latitude, double Longitude, int? PageNumber, int? PageSize, CancellationToken CancellationToken);
    }
}
