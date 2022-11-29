using System;
using System.Security.Cryptography.X509Certificates;

namespace SiteCalculations.Models
{
    public class ApartmentBuildingSectionModel
    {
        public int SectionNumber { get; set; }
        public int NumberOfFloors { get; set; }
        public string StageName { get; set; }
        public string Name { get; set; }
        public double ConstructionArea { get; set; }
        public double ApartmentsArea { get; set; }
        public double CommerceArea { get; set; }
        public int NumberOfApartments { get; set; }

        public ApartmentBuildingSectionModel(string[] parameters)
        {
            StageName = parameters[0];
            Name = parameters[1];
            SectionNumber = Convert.ToInt32(parameters[2]);
            NumberOfFloors = Convert.ToInt32(parameters[3]);
            NumberOfApartments = Convert.ToInt32(parameters[4]);
            ConstructionArea = Convert.ToDouble(parameters[5]);
            ApartmentsArea = Convert.ToDouble(parameters[6]);
            CommerceArea = Convert.ToDouble(parameters[7]);

        }
    }
}
