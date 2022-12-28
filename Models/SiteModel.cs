using ColorControl;
using System;
using System.Collections.Generic;

namespace SiteCalculations.Models
{
    public class SiteModel : BaseBigAreaModel
    {
        public List<string> PartNames { get; set; }
        public SiteModel(CityModel city, string name, double plotArea, List<BaseBigAreaModel> list)
        {
            City = city;
            Name = name;
            PartNames = new List<string>();
            var parkingReqList = new List<ParkingModel>();
            var parkingExList = new List<ParkingModel>();
            var amenitiesReqList = new List<AmenitiesModel>();
            var amenitiesExList = new List<AmenitiesModel>();
            List<string> floors = new List<string>();
            PlotArea = Math.Round(plotArea,2);
            foreach (var l in list)
            {
                floors.Add(l.NumberOfFloors);
                PartNames.Add(l.Name);
                parkingReqList.Add(l.TotalParkingReq);
                parkingExList.Add(l.TotalParkingEx);
                amenitiesReqList.Add(l.AmenitiesReq);
                amenitiesExList.Add(l.AmenitiesEx);
                TotalConstructionArea += l.TotalConstructionArea;
                TotalResidents += l.TotalResidents;
                TotalNumberOfApartments += l.TotalNumberOfApartments;
                TotalApartmentArea += l.TotalApartmentArea;
                TotalCommerceArea += l.TotalCommerceArea;
                SchoolsEx += l.SchoolsEx;
                KindergartensEx += l.KindergartensEx;
                HospitalsEx += l.HospitalsEx;
                SchoolsReq+= l.SchoolsReq;
                KindergartensReq+= l.KindergartensReq;
                HospitalsReq+= l.HospitalsReq;
                ParksReq+= l.ParksReq;
                ParksEx+= l.ParksEx;
                SportBuildingsReq+= l.SportBuildingsReq;
                SportBuildingsEx+= l.SportBuildingsEx;
                SportFieldsReq+= l.SportFieldsReq;
                SportFieldsEx+= l.SportFieldsEx;
            }
            Functions f = new Functions();
            NumberOfFloors = f.GetMaxMinFromListOfStrings(floors, '-');
            SchoolsReq = Math.Round(SchoolsReq,2);
            KindergartensReq = Math.Round(KindergartensReq,2);
            HospitalsReq = Math.Round(HospitalsReq, 2);
            AmenitiesReq = new AmenitiesModel(Name, amenitiesReqList);
            AmenitiesEx = new AmenitiesModel(Name, amenitiesExList);
            TotalParkingReq = new ParkingModel(Name, parkingReqList);
            TotalParkingEx = new ParkingModel(Name, parkingExList);
            BuildingPartPercent = Math.Round(100 * TotalConstructionArea / PlotArea,2);
        }
    }
}
