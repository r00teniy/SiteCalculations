namespace SiteCalculations.Models
{
    public class AreaReqFromPeopleModel : IAreaReq
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
            values[0] = numberOfPeople * ChildReq;
            values[1] = numberOfPeople * SportReq;
            values[2] = numberOfPeople * RestReq;
            values[3] = numberOfPeople * UtilityReq;
            values[4] = numberOfPeople * TrashReq;
            values[5] = numberOfPeople * DogsReq;
            values[6] = numberOfPeople * TotalReq;
            values[7] = numberOfPeople * GreeneryReq;
            return new AmenitiesModel(name, values);
        }
        public AreaReqFromPeopleModel(double childReq, double sportReq, double restReq, double utilityReq, double trashReq, double dogsReq, double totalReq, double greeneryReq)
        {
            ChildReq = childReq;
            SportReq = sportReq;
            RestReq = restReq;
            UtilityReq = utilityReq;
            TrashReq = trashReq;
            DogsReq = dogsReq;
            TotalReq = totalReq;
            GreeneryReq = greeneryReq;
        }
    }
}

