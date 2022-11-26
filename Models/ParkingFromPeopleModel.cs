using System;

namespace SiteCalculations.Models
{
    internal class ParkingFromPeopleModel : IParking
    {
        public double LongParkingReq { get; set; }
        public double ShortParkingReq { get; set; }
        public double GuestParkingReq { get; set; }

        public int[] CalculateParking(int numberOfPeople, double apartmentArea, int numberOfApartmernts, double commerceArea)
        {
            int[] values = new int[3];
            values[0] = Convert.ToInt32(Math.Ceiling(LongParkingReq * numberOfPeople));
            values[1] = Convert.ToInt32(Math.Ceiling(ShortParkingReq * numberOfPeople));
            values[2] = Convert.ToInt32(Math.Ceiling(GuestParkingReq * numberOfPeople));
            return values;
        }
    }
}
