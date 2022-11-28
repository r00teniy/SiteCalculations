namespace SiteCalculations.Models
{
    public interface IParking
    {
        double LongParkingReq { get; set; }
        double ShortParkingReq { get; set; }
        double GuestParkingReq { get; set; }
        ParkingModel CalculateParking(string name, double[] parameters);
    }
}
