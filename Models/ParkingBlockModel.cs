using System;

namespace SiteCalculations.Models
{
    public class ParkingBlockModel
    {
        public int NumberOfParkings { get; private set; }
        public string ParkingIsForBuildingName { get; private set; }
        public string Type { get; private set; }
        public string Size { get; private set; }
        public bool IsForDisabled { get; private set; }
        public bool IsForDisabledExtended { get; private set; }
        public string PlotNumber { get; private set; }
        public ParkingBlockModel(string[] parkParams)
        {
            NumberOfParkings = Convert.ToInt32(parkParams[5]);
            ParkingIsForBuildingName = "ГП-" + parkParams[4];
            PlotNumber= parkParams[6];
            if (parkParams[2] == "0")
            { IsForDisabled = false; }
            else
            { IsForDisabled = true; }
            if (parkParams[3] == "0")
            { IsForDisabledExtended = false; }
            else
            { IsForDisabledExtended = true; }
            Size = parkParams[0];
            switch(parkParams[1])
            {
                case "Постоянное":
                    Type = "Long";
                    break;
                case "Временное":
                    Type = "Short";
                    break;
                case "Гостевое":
                    Type = "Guest";
                    break;

            }
        }
    }
}
