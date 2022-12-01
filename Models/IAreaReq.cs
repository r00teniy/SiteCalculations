namespace SiteCalculations.Models
{
    public interface IAreaReq
    {
        double ChildReq { get; set; }
        double SportReq { get; set; }
        double RestReq { get; set; }
        double UtilityReq { get; set; }
        double TrashReq { get; set; }
        double DogsReq { get; set; }
        double TotalReq { get; set; }
        double GreeneryReq { get; set; }
        AmenitiesModel CalculateReqArea(string name, int numberOfPeople, double ApartmentArea, int numberOfApartments);
    }
}
