using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace SiteCalculations.Models
{
    public class StageModel : BaseBigAreaModel
    {
        public List<string> Buildings { get; private set; }

        public StageModel(List<IBaseBuilding> buildingsList)
        {
            Name = buildingsList[0].StageName;
            Buildings = new List<string>();
            foreach (var bd in buildingsList)
            {
                Buildings.Add(bd.BuildingName);
                TotalConstructionArea += bd.ConstructionArea;
                PlotArea += bd.PlotArea;
                // for apartmentbuilding
                if (bd is ApartmentBuildingModel mod)
                {
                    TotalResidents += mod.TotalResidents;
                    TotalNumberOfApartments = mod.TotalNumberOfApartments;
                    TotalApartmentArea = mod.TotalApartmentArea;
                    TotalCommerceArea = mod.TotalCommerceArea;
                    TotalChildAreaReq = mod.TotalChildAreaReq;
                    TotalSportAreaReq = mod.TotalSportAreaReq;
                    TotalRestAreaReq = mod.TotalRestAreaReq;
                    TotalUtilityAreaReq = mod.TotalUtilityAreaReq;
                    TotalTrashAreaReq = mod.TotalTrashAreaReq;
                    TotalDogsAreaReq = mod.TotalDogsAreaReq;
                    TotalAreaReq = mod.TotalAreaReq;
                    TotalGreeneryAreaReq = mod.TotalGreeneryAreaReq;
                    TotalLongParkingReq = mod.TotalLongParkingReq;
                    TotalShortParkingReq = mod.TotalShortParkingReq;
                    TotalGuestParkingReq = mod.TotalGuestParkingReq;
                    TotalChildAreaEx = mod.TotalChildAreaEx;
                    TotalSportAreaEx = mod.TotalSportAreaEx;
                    TotalRestAreaEx = mod.TotalRestAreaEx;
                    TotalUtilityAreaEx = mod.TotalUtilityAreaEx;
                    TotalDogsAreaEx = mod.TotalDogsAreaEx;
                    TotalTrashAreaEx = mod.TotalTrashAreaEx;
                    TotalAreaEx = mod.TotalAreaEx;
                    TotalGreeneryAreaEx = mod.TotalGreeneryAreaEx;
                    TotalLongParkingEx = mod.TotalLongParkingEx;
                    TotalShortParkingEx = mod.TotalShortParkingEx;
                    TotalGuestParkingEx = mod.TotalGuestParkingEx;
                }
            }
            BuildingPartPercent = 100 * TotalConstructionArea / PlotArea;
        }
    }
}
