namespace POIApp.Common
{
    public class PointOfInterest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Image { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
