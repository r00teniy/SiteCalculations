using Autodesk.AutoCAD.Geometry;
using System;

namespace SiteCalculations.Models
{
    public class HospitalModel : IBaseBuilding
    {
        public string StageName { get; private set; }
        public string Name { get; private set; }
        public double TotalConstructionArea { get; private set; }
        public string NumberOfFloors { get; private set; }
        public int NumberOfPatientsPerDay { get; private set; }
        public double PlotArea { get; private set; }
        public string PlotNumber { get; private set; }
        public double PlotRequired { get; private set; }
        public ParkingModel TotalParkingReq { get; private set; }
        public ParkingModel TotalParkingEx { get; private set; }
        public Point3d MidPoint { get; private set; }
        public HospitalModel(CityModel city, string[] parameters, ZoneBorderModel plot, ParkingModel exParking, Point3d midPoint)
        {
            StageName = parameters[0];
            Name = parameters[1];
            TotalConstructionArea = Convert.ToDouble(parameters[2]);
            NumberOfFloors = parameters[3];
            NumberOfPatientsPerDay = Convert.ToInt32(parameters[4]);
            PlotArea = Math.Round(plot.Area, 2);
            PlotNumber = plot.PlotNumber;
            PlotRequired = Convert.ToDouble(parameters[5]);
            TotalParkingReq = city.Parking.CalculateParking(Name, new double[] { 0, 0, 0, 0, 0, 0, 0, 0,NumberOfPatientsPerDay, 0, 0 });
            TotalParkingEx = exParking;
            MidPoint = midPoint;
        }
    }
}
