
using System;
using System.Collections.Generic;

namespace SiteCalculations.Models
{
    public class AmenitiesModel
    {
        public string Name { get; private set; }
        public double ChildrenArea { get; private set; }
        public double SportArea { get; private set; }
        public double RestArea { get; private set; }
        public double UtilityArea { get; private set; }
        public double TrashArea { get; private set; }
        public double DogsArea { get; private set; }
        public double TotalArea { get; private set; }
        public double GreeneryArea { get; private set; }

        public AmenitiesModel(string[] parameters)
        {
            Name = parameters[0];
            ChildrenArea = Convert.ToDouble(parameters[1]);
            SportArea = Convert.ToDouble(parameters[2]);
            RestArea = Convert.ToDouble(parameters[3]);
            UtilityArea = Convert.ToDouble(parameters[4]);
            TrashArea = Convert.ToDouble(parameters[5]);
            DogsArea = Convert.ToDouble(parameters[6]);
            TotalArea = Convert.ToDouble(parameters[7]);
            GreeneryArea = Convert.ToDouble(parameters[8]);
        }
        public AmenitiesModel(string name, List<AmenitiesModel> amenitiesList)
        {
            Name = name;
            foreach (var ame in amenitiesList)
            {
                ChildrenArea += ame.ChildrenArea;
                SportArea += ame.SportArea;
                RestArea += ame.RestArea;
                UtilityArea += ame.UtilityArea;
                TrashArea += ame.TrashArea;
                DogsArea += ame.
                TotalArea += ame.TotalArea;
                GreeneryArea += ame.GreeneryArea;
            }
        }
        public AmenitiesModel(string name, double[] parameters)
        {
            Name = name;
            ChildrenArea = parameters[0];
            SportArea = parameters[1];
            RestArea = parameters[2];
            UtilityArea = parameters[3];
            TrashArea = parameters[4];
            DogsArea = parameters[5];
            TotalArea = parameters[6];
            GreeneryArea = parameters[7];
        }
    }
}
