using System;

namespace SiteCalculations.Models
{
    internal class ParkingFromPeopleModel : IParking
    {
        public double LongParkingReq { get; set; }
        public double ShortParkingReq { get; set; }
        public double GuestParkingReq { get; set; }
        public double SchoolParkingReq { get; set; }
        public double KindergartenParkingReq { get; set; }
        public double HospitalParkingReq { get; set; }

        public ParkingModel CalculateParking(string name, double[] parameters)
        {
            var parkLong = Convert.ToInt32(Math.Ceiling(LongParkingReq * parameters[0]));
            var parkGuest = Convert.ToInt32(Math.Ceiling(GuestParkingReq * parameters[0]));
            var parkShort = Convert.ToInt32(Math.Ceiling(ShortParkingReq * parameters[3]));
            parkShort += Convert.ToInt32(Math.Ceiling(GuestParkingReq * parameters[4]));
            parkShort += Convert.ToInt32(Math.Ceiling(GuestParkingReq * parameters[5]));
            parkShort += Convert.ToInt32(Math.Ceiling(GuestParkingReq * parameters[6]));
            ParkingModel parking = new ParkingModel(name, parkLong, parkShort, parkGuest);
            return parking;
        }

        public ParkingFromPeopleModel(double longParkingReq, double shortParkingReq, double guestParkingReq, double schoolParkingReq, double kindergartenParkingReq, double hospitalParkingReq)
        {
            LongParkingReq = longParkingReq;
            ShortParkingReq = shortParkingReq;
            GuestParkingReq = guestParkingReq;
            SchoolParkingReq = schoolParkingReq;
            KindergartenParkingReq = kindergartenParkingReq;
            HospitalParkingReq = hospitalParkingReq;
        }
    }
}
