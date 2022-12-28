using Autodesk.AutoCAD.DatabaseServices;

namespace SiteCalculations.Models
{
    public class ZoneBorderModel
    {
        public string Name { get; private set; }
        public double Area { get; private set; }
        public string PlotNumber { get; set; }
        public Polyline Polyline { get; private set; }
        public ZoneBorderModel(string name, Polyline pl)
        {
            Name = name;
            Area = pl.Area;
            Polyline = pl;
            
        }
    }
}
