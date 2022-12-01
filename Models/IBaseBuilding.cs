using Autodesk.AutoCAD.Geometry;

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
        Point3d MidPoint { get; }
    }
}
