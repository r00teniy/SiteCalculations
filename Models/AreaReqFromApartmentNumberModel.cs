using System.Xml.Linq;

namespace SiteCalculations.Models
{
    public class AreaReqFromApartmentNumberModel : IAreaReq
    {
        public double ChildReq { get; set; }
        public double SportReq { get; set; }
        public double RestReq { get; set; }
        public double UtilityReq { get; set; }
        public double TrashReq { get; set; }
        public double DogsReq { get; set; }
        public double TotalReq { get; set; }
        public double GreeneryReq { get; set; }

        public AmenitiesModel CalculateReqArea(string name, int numberOfPeople, double ApartmentArea, int numberOfApartments)
        {
            double[] values = new double[8];
            values[0] = numberOfApartments * ChildReq;
            values[1] = numberOfApartments * SportReq;
            values[2] = numberOfApartments * RestReq;
            values[3] = numberOfApartments * UtilityReq;
            values[4] = numberOfApartments * TrashReq;
            values[5] = numberOfApartments * DogsReq;
            values[6] = numberOfApartments * TotalReq;
            values[7] = numberOfApartments * GreeneryReq;
            return new AmenitiesModel(name, values);
        }
    }
}
