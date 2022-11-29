namespace SiteCalculations.Models
{
    public interface IBaseBuilding
    {
        string StageName { get; }
        string Name { get; }
        double TotalConstructionArea { get; }
        double PlotArea { get; }
        ParkingModel TotalParkingReq { get; }
        ParkingModel TotalParkingEx { get; }
    }
}
