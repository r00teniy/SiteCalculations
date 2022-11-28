using Autodesk.Windows.ToolBars;
using System.Collections.Generic;

namespace SiteCalculations.Models
{
    public class StageModel : BaseBigAreaModel
    {
        public List<string> Buildings { get; private set; }

        public StageModel(CityModel city, List<IBaseBuilding> buildingsList)
        {
            Name = buildingsList[0].StageName;
            City = city;
            Buildings = new List<string>();
            var parkReq = new List<ParkingModel>();
            var parkEx = new List<ParkingModel>();
            foreach (var bd in buildingsList)
            {
                Buildings.Add(bd.Name);
                TotalConstructionArea += bd.ConstructionArea;
                PlotArea += bd.PlotArea;
                parkReq.Add(bd.TotalParkingReq);
                parkEx.Add(bd.TotalParkingEx);
                // for apartmentbuilding
                if (bd is ApartmentBuildingModel mod)
                {
                    TotalResidents += mod.TotalResidents;
                    TotalNumberOfApartments += mod.TotalNumberOfApartments;
                    TotalApartmentArea += mod.TotalApartmentArea;
                    TotalCommerceArea += mod.TotalCommerceArea;
                    TotalChildAreaReq += mod.TotalChildAreaReq;
                    TotalSportAreaReq += mod.TotalSportAreaReq;
                    TotalRestAreaReq += mod.TotalRestAreaReq;
                    TotalUtilityAreaReq += mod.TotalUtilityAreaReq;
                    TotalTrashAreaReq += mod.TotalTrashAreaReq;
                    TotalDogsAreaReq += mod.TotalDogsAreaReq;
                    TotalAreaReq += mod.TotalAreaReq;
                    TotalGreeneryAreaReq += mod.TotalGreeneryAreaReq;
                    TotalChildAreaEx += mod.TotalChildAreaEx;
                    TotalSportAreaEx += mod.TotalSportAreaEx;
                    TotalRestAreaEx += mod.TotalRestAreaEx;
                    TotalUtilityAreaEx += mod.TotalUtilityAreaEx;
                    TotalDogsAreaEx += mod.TotalDogsAreaEx;
                    TotalTrashAreaEx += mod.TotalTrashAreaEx;
                    TotalAreaEx += mod.TotalAreaEx;
                    TotalGreeneryAreaEx += mod.TotalGreeneryAreaEx;
                }
                if (bd is SchoolModel sm)
                {
                    SchoolsEx += sm.NumberOfStudents;
                }
                if (bd is KindergartenModel km)
                {
                    KindergartensEx += km.NumberOfStudents;
                }
                if (bd is HospitalModel hm)
                {
                    HospitalsEx += hm.NumberOfPatientsPerDay;
                }
            }
            TotalParkingReq = new ParkingModel(Name, parkReq);
            TotalParkingEx = new ParkingModel(Name, parkEx);
            BuildingPartPercent = 100 * TotalConstructionArea / PlotArea;
            SchoolsReq = TotalResidents * city.SchoolsReq;
            KindergartensReq = TotalResidents * city.KindergartensReq;
            HospitalsReq = TotalResidents * city.HospitalsReq;
        }
    }
}
