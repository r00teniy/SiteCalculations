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
        public double BuildingPartPercent { get; set; }
        public int TotalResidents { get; set; }
        public int TotalNumberOfApartments { get; set; }
        public double TotalApartmentArea { get; set; }
        public double TotalCommerceArea { get; set; }
        // Requirements part
        // Areas
        public double TotalChildAreaReq { get; set; }
        public double TotalSportAreaReq { get; set; }
        public double TotalRestAreaReq { get; set; }
        public double TotalUtilityAreaReq { get; set; }
        public double TotalTrashAreaReq { get; set; }
        public double TotalDogsAreaReq { get; set; }
        public double TotalAreaReq { get; set; }
        public double TotalGreeneryAreaReq { get; set; }
        // Existing part
        public double TotalChildAreaEx { get; set; }
        public double TotalSportAreaEx { get; set; }
        public double TotalRestAreaEx { get; set; }
        public double TotalUtilityAreaEx { get; set; }
        public double TotalDogsAreaEx { get; set; }
        public double TotalTrashAreaEx { get; set; }
        public double TotalAreaEx { get; set; }
        public double TotalGreeneryAreaEx { get; set; }
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
