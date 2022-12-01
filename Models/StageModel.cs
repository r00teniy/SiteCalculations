using System;
using System.Collections.Generic;

namespace SiteCalculations.Models
{
    public class StageModel : BaseBigAreaModel
    {
        public List<string> Buildings { get; private set; }

        public StageModel(CityModel city, double plotArea, List<IBaseBuilding> buildingsList)
        {
            Name = buildingsList[0].StageName;
            City = city;
            Buildings = new List<string>();
            var parkingReq = new List<ParkingModel>();
            var parkingEx = new List<ParkingModel>();
            var amenitiesReqList = new List<AmenitiesModel>();
            var amenitiesExList = new List<AmenitiesModel>();
            PlotArea = Math.Round(plotArea,2);
            foreach (var bd in buildingsList)
            {
                Buildings.Add(bd.Name);
                TotalConstructionArea += bd.TotalConstructionArea;
                parkingReq.Add(bd.TotalParkingReq);
                parkingEx.Add(bd.TotalParkingEx);
                // for apartmentbuilding
                if (bd is ApartmentBuildingModel mod)
                {
                    amenitiesReqList.Add(mod.AmenitiesReq);
                    amenitiesExList.Add(mod.AmenitiesEx);
                    TotalResidents += mod.TotalResidents;
                    TotalNumberOfApartments += mod.TotalNumberOfApartments;
                    TotalApartmentArea += mod.TotalApartmentArea;
                    TotalCommerceArea += mod.TotalCommerceArea;
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
            AmenitiesReq = new AmenitiesModel(Name, amenitiesReqList);
            AmenitiesEx = new AmenitiesModel(Name, amenitiesExList);
            TotalParkingReq = new ParkingModel(Name, parkingReq);
            TotalParkingEx = new ParkingModel(Name, parkingEx);
            BuildingPartPercent = Math.Round(100 * TotalConstructionArea / PlotArea, 2);
            SchoolsReq = TotalResidents * city.SchoolsReq;
            KindergartensReq = TotalResidents * city.KindergartensReq;
            HospitalsReq = TotalResidents * city.HospitalsReq;
        }
    }
}
