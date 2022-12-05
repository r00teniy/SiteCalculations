using Newtonsoft.Json;
using System;
using System.Globalization;

namespace SiteCalculations.Models
{
    public class ParkingReqModel
    {
        public string Name { get; set; }
        public double LongParkingReqPeople { get; set; }
        public double LongParkingReqApartments { get; set; }
        public double LongParkingReqSqm { get; set; }
        public double ShortParkingReqSqm { get; set; }
        public double GuestParkingReqPeople { get; set; }
        public double GuestParkingReqApartments { get; set; }
        public double GuestParkingReqSqm { get; set; }
        public double SchoolParkingReq { get; set; }
        public double KindergartenParkingReq { get; set; }
        public double HospitalParkingReq { get; set; }

        public ParkingModel CalculateParking(string name, double[] parameters)
        {
            var parkLong = Convert.ToInt32(Math.Ceiling(LongParkingReqPeople * parameters[0] + LongParkingReqApartments * parameters[1]+ LongParkingReqSqm * parameters[0]));
            var parkGuest = Convert.ToInt32(Math.Ceiling(GuestParkingReqPeople * parameters[0] + GuestParkingReqApartments * parameters[1] + GuestParkingReqSqm * parameters[0]));
            var parkShort = Convert.ToInt32(Math.Ceiling(ShortParkingReqSqm * parameters[3]));
            parkShort += Convert.ToInt32(Math.Ceiling(SchoolParkingReq * parameters[4]));
            parkShort += Convert.ToInt32(Math.Ceiling(KindergartenParkingReq * parameters[5]));
            parkShort += Convert.ToInt32(Math.Ceiling(HospitalParkingReq * parameters[6]));
            ParkingModel parking = new ParkingModel(name, parkLong, parkShort, parkGuest);
            return parking;
        }

        public ParkingReqModel(string[] parameters)
        {
            LongParkingReqPeople = Convert.ToDouble(parameters[0], CultureInfo.InvariantCulture);
            LongParkingReqApartments = Convert.ToDouble(parameters[1], CultureInfo.InvariantCulture);
            LongParkingReqSqm = Convert.ToDouble(parameters[2], CultureInfo.InvariantCulture);
            ShortParkingReqSqm = Convert.ToDouble(parameters[3], CultureInfo.InvariantCulture);
            GuestParkingReqPeople = Convert.ToDouble(parameters[4], CultureInfo.InvariantCulture);
            GuestParkingReqApartments = Convert.ToDouble(parameters[5], CultureInfo.InvariantCulture);
            GuestParkingReqSqm = Convert.ToDouble(parameters[6], CultureInfo.InvariantCulture);
            SchoolParkingReq = Convert.ToDouble(parameters[7], CultureInfo.InvariantCulture);
            KindergartenParkingReq = Convert.ToDouble(parameters[8], CultureInfo.InvariantCulture);
            HospitalParkingReq = Convert.ToDouble(parameters[9], CultureInfo.InvariantCulture);
            Name = parameters[10];
        }
        [JsonConstructor]
        public ParkingReqModel()
        {
            
        }
    }
}
