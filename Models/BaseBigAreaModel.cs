namespace SiteCalculations.Models
{
    abstract public class BaseBigAreaModel
    {
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
        // Parking
        public int TotalLongParkingReq { get; set; }
        public int TotalShortParkingReq { get; set; }
        public int TotalGuestParkingReq { get; set; }
        // Existing part
        //Areas
        public double TotalChildAreaEx { get; set; }
        public double TotalSportAreaEx { get; set; }
        public double TotalRestAreaEx { get; set; }
        public double TotalUtilityAreaEx { get; set; }
        public double TotalDogsAreaEx { get; set; }
        public double TotalTrashAreaEx { get; set; }
        public double TotalAreaEx { get; set; }
        public double TotalGreeneryAreaEx { get; set; }
        // Parking
        public int TotalLongParkingEx { get; set; }
        public int TotalShortParkingEx { get; set; }
        public int TotalGuestParkingEx { get; set; }
    }
}
