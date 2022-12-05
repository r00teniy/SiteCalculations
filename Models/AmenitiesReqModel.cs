using Newtonsoft.Json;
using System;
using System.Globalization;

namespace SiteCalculations.Models
{
    public class AmenitiesReqModel
    {
        public string Name { get; set; }
        public double ChildReqPeople { get; set; }
        public double ChildReqApartments { get; set; }
        public double ChildReqSqm { get; set; }
        public double SportReqPeople { get; set; }
        public double SportReqApartments { get; set; }
        public double SportReqSqm { get; set; }
        public double RestReqPeople { get; set; }
        public double RestReqApartments { get; set; }
        public double RestReqSqm { get; set; }
        public double UtilityReqPeople { get; set; }
        public double UtilityReqApartments { get; set; }
        public double UtilityReqSqm { get; set; }
        public double TrashReqPeople { get; set; }
        public double TrashReqApartments { get; set; }
        public double TrashReqSqm { get; set; }
        public double DogsReqPeople { get; set; }
        public double DogsReqApartments { get; set; }
        public double DogsReqSqm { get; set; }
        public double TotalReqPeople { get; set; }
        public double TotalReqApartments { get; set; }
        public double TotalReqSqm { get; set; }
        public double GreeneryReqPeople { get; set; }
        public double GreeneryReqApartments { get; set; }
        public double GreeneryReqSqm { get; set; }
        public AmenitiesModel CalculateReqArea(string name, int numberOfPeople, int numberOfApartments, double ApartmentArea)
        {
            double[] values = new double[8];
            values[0] = numberOfPeople * ChildReqPeople + numberOfApartments * ChildReqApartments + ApartmentArea * ChildReqSqm;
            values[1] = numberOfPeople * SportReqPeople + numberOfApartments * SportReqApartments + ApartmentArea * SportReqSqm;
            values[2] = numberOfPeople * RestReqPeople + numberOfApartments * RestReqApartments + ApartmentArea * RestReqSqm;
            values[3] = numberOfPeople * UtilityReqPeople + numberOfApartments * UtilityReqApartments + ApartmentArea * UtilityReqSqm;
            values[4] = numberOfPeople * TrashReqPeople + numberOfApartments * TrashReqApartments + ApartmentArea * TrashReqSqm;
            values[5] = numberOfPeople * DogsReqPeople + numberOfApartments * DogsReqApartments + ApartmentArea * DogsReqSqm;
            values[6] = numberOfPeople * TotalReqPeople + numberOfApartments * TotalReqApartments + ApartmentArea * TotalReqSqm;
            values[7] = numberOfPeople * GreeneryReqPeople + numberOfApartments * GreeneryReqApartments + ApartmentArea * GreeneryReqSqm;
            return new AmenitiesModel(name, values);
        }

        public AmenitiesReqModel(string[] parameters)
        {
            ChildReqPeople = Convert.ToDouble(parameters[0], CultureInfo.InvariantCulture);
            ChildReqApartments = Convert.ToDouble(parameters[1], CultureInfo.InvariantCulture);
            ChildReqSqm = Convert.ToDouble(parameters[2], CultureInfo.InvariantCulture);
            SportReqPeople = Convert.ToDouble(parameters[3], CultureInfo.InvariantCulture);
            SportReqApartments = Convert.ToDouble(parameters[4], CultureInfo.InvariantCulture);
            SportReqSqm = Convert.ToDouble(parameters[5], CultureInfo.InvariantCulture);
            RestReqPeople = Convert.ToDouble(parameters[6], CultureInfo.InvariantCulture);
            RestReqApartments = Convert.ToDouble(parameters[7], CultureInfo.InvariantCulture);
            RestReqSqm = Convert.ToDouble(parameters[8], CultureInfo.InvariantCulture);
            UtilityReqPeople = Convert.ToDouble(parameters[9], CultureInfo.InvariantCulture);
            UtilityReqApartments = Convert.ToDouble(parameters[10], CultureInfo.InvariantCulture);
            UtilityReqSqm = Convert.ToDouble(parameters[11], CultureInfo.InvariantCulture);
            TrashReqPeople = Convert.ToDouble(parameters[12], CultureInfo.InvariantCulture);
            TrashReqApartments = Convert.ToDouble(parameters[13], CultureInfo.InvariantCulture);
            TrashReqSqm = Convert.ToDouble(parameters[14], CultureInfo.InvariantCulture);
            DogsReqPeople = Convert.ToDouble(parameters[15], CultureInfo.InvariantCulture);
            DogsReqApartments = Convert.ToDouble(parameters[16], CultureInfo.InvariantCulture);
            DogsReqSqm = Convert.ToDouble(parameters[17], CultureInfo.InvariantCulture);
            TotalReqPeople = Convert.ToDouble(parameters[18], CultureInfo.InvariantCulture);
            TotalReqApartments = Convert.ToDouble(parameters[19], CultureInfo.InvariantCulture);
            TotalReqSqm = Convert.ToDouble(parameters[20], CultureInfo.InvariantCulture);
            GreeneryReqPeople = Convert.ToDouble(parameters[21], CultureInfo.InvariantCulture);
            GreeneryReqApartments = Convert.ToDouble(parameters[22], CultureInfo.InvariantCulture);
            GreeneryReqSqm = Convert.ToDouble(parameters[23], CultureInfo.InvariantCulture);
            Name = parameters[24];
        }
        [JsonConstructor]
        public AmenitiesReqModel()
        {
            
        }
    }
}
