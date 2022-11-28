using System;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System.Reflection;
using SiteCalculations.Models;
using System.Collections.Generic;
using static System.Collections.Specialized.BitVector32;
using Autodesk.AutoCAD.Windows.Data;
using System.Linq;

[assembly: CommandClass(typeof(SiteCalculations.Main))]

namespace SiteCalculations
{
    internal class Main
    {
        [CommandMethod("testtest")]
        public void testtest()
        {
            var f =  new Functions();
            List<EntityBorderModel> borders = f.GetBorders();
            List<ExParametersModel> exParams = f.GetExParameters();
            //test data
            AreaReqFromPeopleModel areaReq = new AreaReqFromPeopleModel(0.5,0.7,0.1,0.3,0.03,0.1,0,2); ;
            ParkingFromPeopleModel parking = new ParkingFromPeopleModel(0.42, 0.0756, 0.016,0.01, 0.01, 0.01);
            CityModel city = new CityModel("test",58.1, 35, 0.135, 0.065, 0.05, parking, areaReq);
            List<ParkingModel> exParking = new List<ParkingModel>
            {
                new ParkingModel("ГП-1", 50, 20, 20),
                new ParkingModel("ГП-2", 30, 10, 22),
                new ParkingModel("ГП-3", 80, 30, 10),
                new ParkingModel("ГП-4", 56, 26, 11),
                new ParkingModel("ГП-5", 45, 25, 42),
                new ParkingModel("ГП-6", 54, 23, 13),
                new ParkingModel("ГП-7", 52, 28, 12),
                new ParkingModel("ГП-10", 250, 0, 0)
            };
            //getting separate building
            List<IBaseBuilding> allBuildings = f.GetBuildings(city, borders, exParams, exParking);
            //Getting sections
            List<ApartmentBuildingSectionModel> allSections = f.GetSecttions();
            //Sorting sections by building name
            List<List<ApartmentBuildingSectionModel>> sectionsByBuilding = new List<List<ApartmentBuildingSectionModel>>();
            foreach (var se in allSections)
            {
                int entId = -1;
                for (int i = 0; i < sectionsByBuilding.Count; i++)
                {
                    if (se.BuildingName == sectionsByBuilding[i][0].BuildingName)
                    {
                        entId = i;
                    }
                }
                if (entId == -1)
                {
                    entId = sectionsByBuilding.Count;
                    sectionsByBuilding.Add(new List<ApartmentBuildingSectionModel>());
                }
                sectionsByBuilding[entId].Add(se);
            }
            //Creating Buildings from sections and adding them to main pool
            for (int i = 0; i < sectionsByBuilding.Count; i++)
            {
                allBuildings.Add(new ApartmentBuildingModel(city, sectionsByBuilding[i], borders.FirstOrDefault(c => c.Name == sectionsByBuilding[i][0].BuildingName).Area, exParams.FirstOrDefault(c => c.BuildingName == sectionsByBuilding[i][0].BuildingName), exParking.FirstOrDefault(c => c.Name == sectionsByBuilding[i][0].BuildingName)));
            }
            //SOrting buildings by stage
            List<List<IBaseBuilding>> buildingsByStage = new List<List<IBaseBuilding>>();
            foreach (var en in allBuildings)
            {
                int stId = -1;
                for (int i = 0; i < buildingsByStage.Count; i++)
                {
                    if (en.StageName == buildingsByStage[i][0].StageName)
                    {
                        stId = i;
                    }
                }
                if (stId == -1)
                {
                    stId = buildingsByStage.Count;
                    buildingsByStage.Add(new List<IBaseBuilding>());
                }
                buildingsByStage[stId].Add(en);
            }
            //creating stages
            List<BaseBigAreaModel> stages= new List<BaseBigAreaModel>(); ;
            for (int i = 0; i < buildingsByStage.Count; i++)
            {
                stages.Add(new StageModel(city, buildingsByStage[i]));
            }
            SiteModel site = new SiteModel(city, "Домодедово", stages);
            f.CreateSiteTable(site);
        }
    }
}
