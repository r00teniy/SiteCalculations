namespace SiteCalculations.Models
{
    abstract public class BaseBigAreaModel
    {
        public CityModel City { get; set;}
        public ParkingModel TotalParkingReq { get; set;}
        public ParkingModel TotalParkingEx { get; set; }
        public string Name { get; set; }
        public double TotalConstructionArea { get; set; }
        public double PlotArea { get; set; }
        public string NumberOfFloors { get; set; }
        public double BuildingPartPercent { get; set; }
        public int TotalResidents { get; set; }
        public int TotalNumberOfApartments { get; set; }
        public double TotalApartmentArea { get; set; }
        public double TotalCommerceArea { get; set; }
        public string NUmberOfFloors { get; set; }
        //Amenities
        public AmenitiesModel AmenitiesReq { get; set; }
        public AmenitiesModel AmenitiesEx { get; set; }
        //Social req
        public double SchoolsReq { get; set; }
        public double KindergartensReq { get; set; }
        public double HospitalsReq { get; set; }
        //Social Ex
        public double SchoolsEx { get; set; }
        public double KindergartensEx { get; set; }
        public double HospitalsEx { get; set; }
    }
}
