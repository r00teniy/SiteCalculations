namespace SiteCalculations.Models
{
    public class EntityBorderModel
    {
        public string Name { get; set; }
        public double Area { get; set; }

        public EntityBorderModel(string name, double area)
        {
            Name = name;
            Area = area;
        }
    }
}
