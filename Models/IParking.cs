namespace SiteCalculations.Models
{
    public interface IParking
    {
        double LongParkingReq { get; set; }
        double ShortParkingReq { get; set; }
        double GuestParkingReq { get; set; }
        int[] CalculateParking(int numberOfPeople, double apartmentArea, int numberOfApartments, double commerceArea);
    }
}
