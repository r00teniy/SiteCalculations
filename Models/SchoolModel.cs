using System;

namespace SiteCalculations.Models
{
    public class SchoolModel : IBaseBuilding
    {
        public string StageName { get; private set; }
        public string Name { get; private set; }
        public double ConstructionArea { get; private set; }
        public int NumberOfFloors { get; private set; }
        public int NumberOfStudents { get; private set; }
        public double PlotArea { get; private set; }
        public double PlotRequired { get; private set; }
        public ParkingModel TotalParkingReq { get; private set; }
        public ParkingModel TotalParkingEx { get; private set; }
        public SchoolModel(CityModel city, string[] parameters, double plotArea, ParkingModel exParking)
        {
            StageName = parameters[0];
            Name = parameters[1];
            ConstructionArea = Convert.ToDouble(parameters[2]);
            NumberOfFloors = Convert.ToInt32(parameters[3]);
            NumberOfStudents = Convert.ToInt32(parameters[4]);
            PlotArea = plotArea;
            PlotRequired = Convert.ToDouble(parameters[5]);
            TotalParkingReq = city.Parking.CalculateParking(Name, new double[] { 0, 0, 0, 0, NumberOfStudents, 0, 0 });
            TotalParkingEx = exParking;
        }
    }
}
