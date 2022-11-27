namespace SiteCalculations.Models
{
    public class CityModel
    {
        public string CityName { get; set; }
        public string Documents { get; set; }
        public double CityLatitude { get; set; }
        public double SqMPerPerson { get; set; }
        public IAreaReq AreaReq { get; set; } 
        public IParking Parking { get; set; } // need detailing implementation
        
        public CityModel(string cityName, string documents, double cityLatitude, double sqMPerPerson, IParking parking, IAreaReq areaReq)
        {
            CityName = cityName;
            Documents = documents;
            CityLatitude = cityLatitude;
            SqMPerPerson = sqMPerPerson;
            AreaReq = areaReq;
            Parking = parking;
        }
    }
}
