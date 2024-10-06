using HotelsWebAPI.DAL;
using HotelsWebAPI.Entities;
using HotelsWebAPI.Features.Hotels.Models;
using HotelsWebAPI.Models;
using HotelsWebAPI.Utilities;
using Microsoft.EntityFrameworkCore;

namespace HotelsWebAPI.Services.HotelService
{
    public class HotelService : IHotelService
    {
        //Repository pattern complicates things, but because of that ApplicationDbContext is injected
        //only here (in one place), which allows easy database switching/changing  
        private readonly ApplicationDbContext _dbContext;
        public HotelService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        //In real applications all Console.WriteLine would be some kind of logger library that would 
        //write exceptions into a file or some outside monitoring service like Application Insights 
        public async Task<int?> AddHotelAsync(string HotelName, decimal Price, double Latitude, double Longitude, CancellationToken cancellationToken)
        {
            var location = GeoUtils.CreatePoint(Latitude, Longitude);
            var hotelToAdd = new Hotel
            {
                HotelName = HotelName,
                Price = Price,
                Location = location
            };

            try
            {
                _dbContext.Hotels.Add(hotelToAdd);
                await _dbContext.SaveChangesAsync(cancellationToken);

                return hotelToAdd.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<int?> DeleteHotelAsync(int Id, CancellationToken CancellationToken)
        {
            try
            {
                var hotelToDelete = await _dbContext.Hotels.FindAsync(Id);
                if (hotelToDelete == null)
                {
                    return -1;
                }

                _dbContext.Hotels.Remove(hotelToDelete);
                await _dbContext.SaveChangesAsync(CancellationToken);

                return hotelToDelete.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<int?> EditHotelAsync(int Id, string HotelName, decimal Price, double Latitude, double Longitude, CancellationToken CancellationToken)
        {
            try
            {
                var hotelToEdit = await _dbContext.Hotels.FindAsync(Id);
                if (hotelToEdit == null)
                {
                    return -1;
                }

                var location = GeoUtils.CreatePoint(Latitude, Longitude);
                hotelToEdit.HotelName = HotelName;
                hotelToEdit.Price = Price;
                hotelToEdit.Location = location;

                _dbContext.Hotels.Update(hotelToEdit);
                await _dbContext.SaveChangesAsync(CancellationToken);

                return hotelToEdit.Id;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<HotelResponseModel>?> GetAllHotelsAsync(CancellationToken CancellationToken)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<HotelResponseModel?> GetHotelByIdAsync(int Id, CancellationToken CancellationToken)
        {
            try
            {
                var result = await _dbContext.Hotels.FindAsync(Id);
                if (result == null) return new HotelResponseModel { Id = -1, HotelName = "Hotel" };

                return new HotelResponseModel
                {
                    Id = result.Id,
                    HotelName = result.HotelName,
                    Price = result.Price,
                    Latitude = result.Location.X,
                    Longitude = result.Location.Y,
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<PagedResponse<SearchedHotelResponseModel>?> GetHotelsByDistanceAsync(double Latitude, double Longitude, int? PageNumber, int? PageSize, CancellationToken CancellationToken)
        {
            var currentLocation = GeoUtils.CreatePoint(Latitude, Longitude);

            var pageNumber = 1;
            if (PageNumber.HasValue && PageNumber.Value > 1) pageNumber = PageNumber.Value;

            var pageSize = 10;
            if (PageSize.HasValue && PageSize > 0) pageSize = PageSize.Value;

            var hotelsQuery = _dbContext.Hotels
                .Select(h => new SearchedHotelResponseModel
                {
                    Id = h.Id,
                    HotelName = h.HotelName,
                    Price = h.Price,
                    Distance = h.Location.Distance(currentLocation)
                })
                .OrderBy(h => h.Price)
                .ThenBy(h => h.Distance)
                .AsQueryable();

            try
            {
                var hotelsSearchedTotalItems = await hotelsQuery.CountAsync(CancellationToken);
                var hotelSearched = await hotelsQuery
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync(CancellationToken);

                return new PagedResponse<SearchedHotelResponseModel>(hotelSearched, hotelsSearchedTotalItems, pageNumber, pageSize);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
