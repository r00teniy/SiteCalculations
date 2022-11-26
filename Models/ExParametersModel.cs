using System;

namespace SiteCalculations.Models
{
    public class ExParametersModel
    {
        public string BuildingName { get; private set; }
        public double TotalChildAreaEx { get; private set; }
        public double TotalSportAreaEx { get; private set; }
        public double TotalRestAreaEx { get; private set; }
        public double TotalUtilityAreaEx { get; private set; }
        public double TotalTrashAreaEx { get; private set; }
        public double TotalDogsAreaEx { get; private set; }
        public double TotalAreaEx { get; private set; }
        public double TotalGreeneryAreaEx { get; private set; }

        public ExParametersModel(string[] parameters)
        {
            BuildingName= parameters[0];
            TotalChildAreaEx = Convert.ToDouble(parameters[1]);
            TotalSportAreaEx = Convert.ToDouble(parameters[2]);
            TotalRestAreaEx = Convert.ToDouble(parameters[3]);
            TotalUtilityAreaEx = Convert.ToDouble(parameters[4]);
            TotalTrashAreaEx = Convert.ToDouble(parameters[5]);
            TotalDogsAreaEx = Convert.ToDouble(parameters[6]);
            TotalAreaEx = Convert.ToDouble(parameters[7]);
            TotalGreeneryAreaEx = Convert.ToDouble(parameters[8]);
        }
    }
}
