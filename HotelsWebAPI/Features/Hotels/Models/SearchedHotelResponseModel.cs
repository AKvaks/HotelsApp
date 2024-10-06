namespace HotelsWebAPI.Features.Hotels.Models
{
    public class SearchedHotelResponseModel
    {
        public int Id { get; set; }
        public required string HotelName { get; set; }
        public decimal Price { get; set; }
        public double Distance { get; set; }
    }
}
