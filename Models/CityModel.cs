namespace SiteCalculations.Models
{
    public class CityModel
    {
        public string CityName { get; set; }
        public double CityLatitude { get; set; }
        public double SqMPerPerson { get; set; }
        public IAreaReq AreaReq { get; set; } 
        public IParking Parking { get; set; } // need detailing implementation
        public double SchoolsReq { get; set; }
        public double KindergartensReq { get; set; }
        public double HospitalsReq { get; set; }

        public CityModel(string cityName, double cityLatitude, double sqMPerPerson, double schoolsReq, double kindergartensReq, double hospitalsReq, IParking parking, IAreaReq areaReq)
        {
            CityName = cityName;
            CityLatitude = cityLatitude;
            SqMPerPerson = sqMPerPerson;
            AreaReq = areaReq;
            Parking = parking;
            HospitalsReq = hospitalsReq;
            SchoolsReq = schoolsReq;
            KindergartensReq = kindergartensReq;
        }
    }
}
