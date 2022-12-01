using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SiteCalculations.Models
{
    internal class ParkingBuildingModel : IBaseBuilding
    {
        public string StageName { get; private set; }
        public string Name { get; private set; }
        public double TotalConstructionArea { get; private set; }
        public int NumberOfFloors { get; private set; }
        public int MaxNumberOfParkingSpaces { get; private set; }
        public double PlotArea { get; private set; }
        public double CommerceArea { get; private set; }
        public ParkingModel TotalParkingReq { get; private set; }
        public ParkingModel TotalParkingEx { get; private set; }
        public Point3d MidPoint { get; private set; }
        public ParkingBuildingModel(CityModel city, string[] parameters, double plotArea, ParkingModel exParking, Point3d midPoint)
        {
            StageName = parameters[0];
            Name = parameters[1];
            TotalConstructionArea = Convert.ToDouble(parameters[2]);
            PlotArea = Math.Round(plotArea, 2);
            NumberOfFloors = Convert.ToInt32(parameters[3]);
            MaxNumberOfParkingSpaces = Convert.ToInt32(parameters[4]);
            CommerceArea = Convert.ToDouble(parameters[5]);
            TotalParkingReq = city.Parking.CalculateParking(Name, new double[] { 0, 0, 0, CommerceArea, 0, 0, 0});
            TotalParkingEx = exParking;
            MidPoint = midPoint;
        }
    }
}
