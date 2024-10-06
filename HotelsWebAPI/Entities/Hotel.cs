using NetTopologySuite.Geometries;

namespace HotelsWebAPI.Entities
{
    public class Hotel
    {
        //In a real application, Guid would be used instead of int for Id type,
        //but to make update and delete methods simpler and easier to use I used int
        public int Id { get; set; }
        public required string HotelName { get; set; }
        public required decimal Price { get; set; }
        public required Point Location { get; set; }
    }
}
