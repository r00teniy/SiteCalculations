using System;

namespace SiteCalculations.Models
{
    public class BuildingBorderModel
    {
        public string Name { get; private set; }
        public double Area { get; private set; }
        public string PlotNumber { get; private set; }
        public BuildingBorderModel(string name, double area, string number = "")
        {
            Name = name;
            Area = area;
            PlotNumber = number;
        }
    }
}
