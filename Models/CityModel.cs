using Newtonsoft.Json;

namespace SiteCalculations.Models
{
    public class CityModel
    {
        public string CityName { get; set; }
        public double CityLatitude { get; set; }
        public double SqMPerPerson { get; set; }
        public AmenitiesReqModel AreaReq { get; set; } 
        public ParkingReqModel Parking { get; set; } // need more detailing implementation for supermarkets and such?
        public double SchoolsReq { get; set; }
        public double KindergartensReq { get; set; }
        public double HospitalsReq { get; set; }

        public CityModel(string cityName, double cityLatitude, double sqMPerPerson, double schoolsReq, double kindergartensReq, double hospitalsReq, ParkingReqModel parking, AmenitiesReqModel amenitiesReq)
        {
            CityName = cityName;
            CityLatitude = cityLatitude;
            SqMPerPerson = sqMPerPerson;
            AreaReq = amenitiesReq;
            Parking = parking;
            HospitalsReq = hospitalsReq;
            SchoolsReq = schoolsReq;
            KindergartensReq = kindergartensReq;
        }
        [JsonConstructor]
        public CityModel()
        {

        }
    }
}
