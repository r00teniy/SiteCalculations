using Autodesk.AutoCAD.DatabaseServices;
using System;

namespace SiteCalculations.Models
{
    public class PlotBorderModel
    {
        public string PlotNumber { get; private set; }
        public double Area { get; private set; }
        public Polyline Polyline { get; private set; }
        public PlotBorderModel(string number, Polyline pl)
        {
            PlotNumber = number;
            Area = pl.Area;
            Polyline = pl;
        }
    }
}
