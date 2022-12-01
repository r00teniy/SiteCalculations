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
            PlotArea = Math.Round(plotArea,2);
            foreach (var l in list)
            {
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
            }
            AmenitiesReq = new AmenitiesModel(Name, amenitiesReqList);
            AmenitiesEx = new AmenitiesModel(Name, amenitiesExList);
            TotalParkingReq = new ParkingModel(Name, parkingReqList);
            TotalParkingEx = new ParkingModel(Name, parkingExList);
            BuildingPartPercent = Math.Round(100 * TotalConstructionArea / PlotArea,2);
        }
    }
}
