namespace SiteCalculations.Models
{
    public interface IBaseBuilding
    {
        string StageName { get; }
        string BuildingName { get; }
        double ConstructionArea { get; }
        double PlotArea { get; }
    }
}
