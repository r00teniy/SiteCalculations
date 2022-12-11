using Autodesk.AutoCAD.Geometry;
using System;

namespace SiteCalculations.Models
{
    internal class ParkingBuildingModel : IBaseBuilding
    {
        public string StageName { get; private set; }
        public string Name { get; private set; }
        public double TotalConstructionArea { get; private set; }
        public string NumberOfFloors { get; private set; }
        public int MaxNumberOfParkingSpaces { get; private set; }
        public double TotalCommerceArea { get { return CommerceArea + OfficeArea + StoreArea; } }
        public double PlotArea { get; private set; }
        public string PlotNumber { get; private set; }
        public double CommerceArea { get; private set; }
        public double OfficeArea { get; private set; }
        public double StoreArea { get; private set; }
        public ParkingModel TotalParkingReq { get; private set; }
        public ParkingModel TotalParkingEx { get; private set; }
        public Point3d MidPoint { get; private set; }
        public ParkingBuildingModel(CityModel city, string[] parameters, BuildingBorderModel plot, ParkingModel exParking, Point3d midPoint)
        {
            StageName = parameters[0];
            Name = parameters[1];
            TotalConstructionArea = Convert.ToDouble(parameters[2]);
            PlotArea = Math.Round(plot.Area, 2);
            PlotNumber = plot.PlotNumber;
            NumberOfFloors = parameters[3];
            MaxNumberOfParkingSpaces = Convert.ToInt32(parameters[4]);
            CommerceArea = Convert.ToDouble(parameters[5]);
            OfficeArea= Convert.ToDouble(parameters[6]);
            StoreArea= Convert.ToDouble(parameters[7]);
            TotalParkingReq = city.Parking.CalculateParking(Name, new double[] { 0, 0, 0, CommerceArea, OfficeArea, StoreArea, 0, 0, 0, 0, 0 });
            TotalParkingEx = exParking;
            MidPoint = midPoint;
        }
    }
}
