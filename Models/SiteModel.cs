using System.Collections.Generic;

namespace SiteCalculations.Models
{
    public class SiteModel : BaseBigAreaModel
    {
        public double CityLatitude { get; set; }
        public List<string> PartNames { get; set; }
        public SiteModel(CityModel city, string name, List<BaseBigAreaModel> list)
        {
            City = city;
            CityLatitude = city.CityLatitude;
            Name = name;
            PartNames = new List<string>();
            var parkReq = new List<ParkingModel>();
            var parkEx = new List<ParkingModel>();
            foreach (var l in list)
            {
                PartNames.Add(l.Name);
                parkReq.Add(l.TotalParkingReq);
                parkEx.Add(l.TotalParkingEx);
                TotalConstructionArea += l.TotalConstructionArea;
                PlotArea += l.PlotArea;
                TotalResidents += l.TotalResidents;
                TotalNumberOfApartments += l.TotalNumberOfApartments;
                TotalApartmentArea += l.TotalApartmentArea;
                TotalCommerceArea += l.TotalCommerceArea;
                TotalChildAreaReq += l.TotalChildAreaReq;
                TotalSportAreaReq += l.TotalSportAreaReq;
                TotalRestAreaReq += l.TotalRestAreaReq;
                TotalUtilityAreaReq += l.TotalUtilityAreaReq;
                TotalTrashAreaReq += l.TotalTrashAreaReq;
                TotalDogsAreaReq += l.TotalDogsAreaReq;
                TotalAreaReq += l.TotalAreaReq;
                TotalGreeneryAreaReq += l.TotalGreeneryAreaReq;
                TotalChildAreaEx += l.TotalChildAreaEx;
                TotalSportAreaEx += l.TotalSportAreaEx;
                TotalRestAreaEx += l.TotalRestAreaEx;
                TotalUtilityAreaEx += l.TotalUtilityAreaEx;
                TotalDogsAreaEx += l.TotalDogsAreaEx;
                TotalTrashAreaEx += l.TotalTrashAreaEx;
                TotalAreaEx += l.TotalAreaEx;
                TotalGreeneryAreaEx += l.TotalGreeneryAreaEx;
                SchoolsEx += l.SchoolsEx;
                KindergartensEx += l.KindergartensEx;
                HospitalsEx += l.HospitalsEx;
                SchoolsReq+= l.SchoolsReq;
                KindergartensReq+= l.KindergartensReq;
                HospitalsReq+= l.HospitalsReq;
            }
            if (TotalAreaReq == 0)
            {
                TotalAreaReq = -1;
            }
            TotalParkingReq = new ParkingModel(Name, parkReq);
            TotalParkingEx = new ParkingModel(Name, parkEx);
            BuildingPartPercent = 100 * TotalConstructionArea / PlotArea;
        }
    }
}
