using System;
using System.Collections.Generic;

namespace SiteCalculations.Models
{
    internal class ApartmentBuildingModel : IBaseBuilding
    {
        // Buildimg part
        public string StageName { get; private set; }
        public string BuildingName { get; private set; }
        public double ConstructionArea { get; private set; }
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
        // Parking
        public int TotalLongParkingReq { get; private set; }
        public int TotalShortParkingReq { get; private set; }
        public int TotalGuestParkingReq { get; private set; }
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
        public int TotalLongParkingEx { get; private set; }
        public int TotalShortParkingEx { get; private set; }
        public int TotalGuestParkingEx { get; private set; }
        //Combining building from sections
        public ApartmentBuildingModel(CityModel city, List<ApartmentBuildingSectionModel> sectionList, double plotArea, ExParametersModel ExParam, int[] parking)
        {
            StageName = sectionList[0].StageName;
            BuildingName = sectionList[0].BuildingName;
            foreach (var sec in sectionList)
            {
                ConstructionArea += sec.ConstructionArea;
                TotalNumberOfApartments += sec.NumberOfApartments;
                TotalApartmentArea += sec.ApartmentsArea;
                TotalCommerceArea += sec.CommerceArea;
                Floors += sec.NumberOfFloors.ToString()+", ";
            }
            PlotArea = plotArea;
            TotalResidents = Convert.ToInt32(Math.Floor(TotalApartmentArea / city.SqMPerPerson));
            BuildingPartPercent = 100 * ConstructionArea / plotArea;
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
            var park = city.Parking.CalculateParking(TotalResidents, TotalApartmentArea, TotalNumberOfApartments, TotalCommerceArea);
            TotalLongParkingReq = park[0];
            TotalShortParkingReq = park[1];
            TotalGuestParkingReq = park[2];
            // existing
            TotalLongParkingEx = parking[0];
            TotalShortParkingEx = parking[1];
            TotalGuestParkingEx = parking[2];
        }
        public ApartmentBuildingModel(CityModel city, string[] buildingParams, double plotArea, ExParametersModel ExParam, int[] parking)
        {
            StageName = buildingParams[0];
            BuildingName = buildingParams[1];
            ConstructionArea = Convert.ToDouble(buildingParams[2]);
            Floors = buildingParams[3];
            TotalNumberOfApartments = Convert.ToInt32(buildingParams[4]);
            TotalApartmentArea = Convert.ToDouble(buildingParams[5]);
            TotalCommerceArea = Convert.ToDouble(buildingParams[6]);
            PlotArea = plotArea;
            TotalResidents = Convert.ToInt32(Math.Floor(TotalApartmentArea / city.SqMPerPerson));
            BuildingPartPercent = 100 * ConstructionArea / plotArea;
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
            var park = city.Parking.CalculateParking(TotalResidents, TotalApartmentArea, TotalNumberOfApartments, TotalCommerceArea);
            TotalLongParkingReq = park[0];
            TotalShortParkingReq = park[1];
            TotalGuestParkingReq = park[2];
            // existing
            TotalLongParkingEx = parking[0];
            TotalShortParkingEx = parking[1];
            TotalGuestParkingEx = parking[2];
        }
    }
}
