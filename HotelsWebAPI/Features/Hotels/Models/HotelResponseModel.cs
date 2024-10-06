namespace HotelsWebAPI.Features.Hotels.Models
{
    public class HotelResponseModel
    {
        public int Id { get; set; }
        public required string HotelName { get; set; }
        public decimal Price { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
