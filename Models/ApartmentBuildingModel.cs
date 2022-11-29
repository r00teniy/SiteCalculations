using System;
using System.Collections.Generic;

namespace SiteCalculations.Models
{
    internal class ApartmentBuildingModel : IBaseBuilding
    {
        // Buildimg part
        public string StageName { get; private set; }
        public string Name { get; private set; }
        public double TotalConstructionArea { get; private set; }
        public double PlotArea { get; private set; }
        public double BuildingPartPercent { get; private set; }
        public string Floors { get; private set; }
        // Apartment part
        public int TotalResidents { get; private set; }
        public int TotalNumberOfApartments { get; private set; }
        public double TotalApartmentArea { get; private set; }
        // commerce part
        public double TotalCommerceArea { get; private set; }
        // Requirements part
        // Areas
        public double TotalChildAreaReq { get; private set; }
        public double TotalSportAreaReq { get; private set; }
        public double TotalRestAreaReq { get; private set; }
        public double TotalUtilityAreaReq { get; private set; }
        public double TotalTrashAreaReq { get; private set; }
        public double TotalDogsAreaReq { get; private set; }
        public double TotalAreaReq { get; private set; }
        public double TotalGreeneryAreaReq { get; private set; }
        // Existing part
        //Areas
        public double TotalChildAreaEx { get; private set; }
        public double TotalSportAreaEx { get; private set; }
        public double TotalRestAreaEx { get; private set; }
        public double TotalUtilityAreaEx { get; private set; }
        public double TotalDogsAreaEx { get; private set; }
        public double TotalTrashAreaEx { get; private set; }
        public double TotalAreaEx { get; private set; }
        public double TotalGreeneryAreaEx { get; private set; }
        // Parking
        public ParkingModel TotalParkingReq { get; private set; }
        public ParkingModel TotalParkingEx { get; private set; }

        //Combining building from sections
        public ApartmentBuildingModel(CityModel city, List<ApartmentBuildingSectionModel> sectionList, double plotArea, ExParametersModel ExParam, ParkingModel exParking)
        {
            StageName = sectionList[0].StageName;
            Name = sectionList[0].Name;
            foreach (var sec in sectionList)
            {
                TotalConstructionArea += sec.ConstructionArea;
                TotalNumberOfApartments += sec.NumberOfApartments;
                TotalApartmentArea += sec.ApartmentsArea;
                TotalCommerceArea += sec.CommerceArea;
                Floors += sec.NumberOfFloors.ToString()+", ";
            }
            PlotArea = Math.Round(plotArea, 2);
            TotalResidents = Convert.ToInt32(Math.Floor(TotalApartmentArea / city.SqMPerPerson));
            BuildingPartPercent = 100 * TotalConstructionArea / plotArea;
            // requirements
            var req = city.AreaReq.CalculateReqArea(TotalResidents, TotalApartmentArea, TotalNumberOfApartments);
            TotalChildAreaReq = req[0];
            TotalSportAreaReq = req[1];
            TotalRestAreaReq = req[2];
            TotalUtilityAreaReq = req[3]; 
            TotalTrashAreaReq = req[4];
            TotalDogsAreaReq = req[5];
            TotalAreaReq = req[6];
            TotalGreeneryAreaReq = req[7];
            // existing
            TotalChildAreaEx = ExParam.TotalChildAreaEx;
            TotalSportAreaEx = ExParam.TotalSportAreaEx;
            TotalRestAreaEx = ExParam.TotalRestAreaEx;
            TotalUtilityAreaEx = ExParam.TotalUtilityAreaEx;
            TotalTrashAreaEx = ExParam.TotalTrashAreaEx;
            TotalDogsAreaEx = ExParam.TotalDogsAreaEx;
            TotalAreaEx = ExParam.TotalAreaEx;
            TotalGreeneryAreaEx = ExParam.TotalGreeneryAreaEx;
            // Parking requires
            TotalParkingReq = city.Parking.CalculateParking(Name, new double[]{ TotalResidents, TotalApartmentArea, TotalNumberOfApartments, TotalCommerceArea, 0, 0, 0 });
            // existing
            TotalParkingEx = exParking;
        }
        public ApartmentBuildingModel(CityModel city, string[] buildingParams, double plotArea, ExParametersModel ExParam, ParkingModel exParking)
        {
            StageName = buildingParams[0];
            Name = buildingParams[1];
            TotalConstructionArea = Convert.ToDouble(buildingParams[2]);
            Floors = buildingParams[3];
            TotalNumberOfApartments = Convert.ToInt32(buildingParams[4]);
            TotalApartmentArea = Convert.ToDouble(buildingParams[5]);
            TotalCommerceArea = Convert.ToDouble(buildingParams[6]);
            PlotArea = plotArea;
            TotalResidents = Convert.ToInt32(Math.Floor(TotalApartmentArea / city.SqMPerPerson));
            BuildingPartPercent = 100 * TotalConstructionArea / plotArea;
            // requirements
            var req = city.AreaReq.CalculateReqArea(TotalResidents, TotalApartmentArea, TotalNumberOfApartments);
            TotalChildAreaReq = req[0];
            TotalSportAreaReq = req[1];
            TotalRestAreaReq = req[2];
            TotalUtilityAreaReq = req[3];
            TotalTrashAreaReq = req[4];
            TotalDogsAreaReq = req[5];
            TotalAreaReq = req[6];
            TotalGreeneryAreaReq = req[7];
            // existing
            TotalChildAreaEx = ExParam.TotalChildAreaEx;
            TotalSportAreaEx = ExParam.TotalSportAreaEx;
            TotalRestAreaEx = ExParam.TotalRestAreaEx;
            TotalUtilityAreaEx = ExParam.TotalUtilityAreaEx;
            TotalTrashAreaEx = ExParam.TotalTrashAreaEx;
            TotalDogsAreaEx = ExParam.TotalDogsAreaEx;
            TotalAreaEx = ExParam.TotalAreaEx;
            TotalGreeneryAreaEx = ExParam.TotalGreeneryAreaEx;
            // Parking requires
            TotalParkingReq = city.Parking.CalculateParking(Name, new double[] { TotalResidents, TotalApartmentArea, TotalNumberOfApartments, TotalCommerceArea, 0, 0, 0 });
            // existing
            TotalParkingEx = exParking;
        }
    }
}
