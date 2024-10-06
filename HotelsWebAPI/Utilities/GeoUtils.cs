using NetTopologySuite.Geometries;

namespace HotelsWebAPI.Utilities
{
    public static class GeoUtils
    {
        public static Point CreatePoint(double latitude, double longitude)
        {
            //SRID (spatial reference system id) is set to 4326
            //because that is the spatial reference system used by Google maps
            return new Point(latitude, longitude) { SRID = 4326 };
        }
    }
}
