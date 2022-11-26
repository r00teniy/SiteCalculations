using System.Collections.Generic;

namespace SiteCalculations.Models
{
    public class SiteModel : BaseBigAreaModel
    {
        public string City { get; set; }
        public double CityLatitude { get; set; }
        public List<string> PartNames { get; set; }

        public SiteModel(string city, double cityLatitude, string name, List<BaseBigAreaModel> list)
        {
            City = city;
            CityLatitude = cityLatitude;
            Name = name;
            PartNames = new List<string>();
            foreach (var l in list)
            {
                PartNames.Add(l.Name);
                TotalConstructionArea += l.TotalConstructionArea;
                PlotArea += l.PlotArea;
                TotalResidents += l.TotalResidents;
                TotalNumberOfApartments = l.TotalNumberOfApartments;
                TotalApartmentArea = l.TotalApartmentArea;
                TotalCommerceArea = l.TotalCommerceArea;
                TotalChildAreaReq = l.TotalChildAreaReq;
                TotalSportAreaReq = l.TotalSportAreaReq;
                TotalRestAreaReq = l.TotalRestAreaReq;
                TotalUtilityAreaReq = l.TotalUtilityAreaReq;
                TotalTrashAreaReq = l.TotalTrashAreaReq;
                TotalDogsAreaReq = l.TotalDogsAreaReq;
                TotalAreaReq = l.TotalAreaReq;
                TotalGreeneryAreaReq = l.TotalGreeneryAreaReq;
                TotalLongParkingReq = l.TotalLongParkingReq;
                TotalShortParkingReq = l.TotalShortParkingReq;
                TotalGuestParkingReq = l.TotalGuestParkingReq;
                TotalChildAreaEx = l.TotalChildAreaEx;
                TotalSportAreaEx = l.TotalSportAreaEx;
                TotalRestAreaEx = l.TotalRestAreaEx;
                TotalUtilityAreaEx = l.TotalUtilityAreaEx;
                TotalDogsAreaEx = l.TotalDogsAreaEx;
                TotalTrashAreaEx = l.TotalTrashAreaEx;
                TotalAreaEx = l.TotalAreaEx;
                TotalGreeneryAreaEx = l.TotalGreeneryAreaEx;
                TotalLongParkingEx = l.TotalLongParkingEx;
                TotalShortParkingEx = l.TotalShortParkingEx;
                TotalGuestParkingEx = l.TotalGuestParkingEx;
            }
            BuildingPartPercent = 100 * TotalConstructionArea / PlotArea;
        }
    }
}
