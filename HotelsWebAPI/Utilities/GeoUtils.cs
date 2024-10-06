using NetTopologySuite.Geometries;

namespace HotelsWebAPI.Utilities
{
    public static class GeoUtils
    {
        //I know that geolocation with only latitude and longitude is less precise than a geolocation 
        //with added height, but for the purpose of this assignment I decided that it's precise enough
        public static Point CreatePoint(double latitude, double longitude)
        {
            //SRID (spatial reference system id) is set to 4326,
            //because that is the spatial reference system used by Google maps
            return new Point(latitude, longitude) { SRID = 4326 };
        }
    }
}
