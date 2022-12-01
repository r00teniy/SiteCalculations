namespace SiteCalculations.Models
{
    public class AreaReqFromApartmentAreaModel : IAreaReq
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
            values[0] = ApartmentArea * ChildReq;
            values[1] = ApartmentArea * SportReq;
            values[2] = ApartmentArea * RestReq;
            values[3] = ApartmentArea * UtilityReq;
            values[4] = ApartmentArea * TrashReq;
            values[5] = ApartmentArea * DogsReq;
            values[6] = ApartmentArea * TotalReq;
            values[7] = ApartmentArea * GreeneryReq;
            return new AmenitiesModel(name, values);
        }
    }
}
