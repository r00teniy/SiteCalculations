﻿using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using SiteCalculations.Models;
using System.Collections.Generic;
using System.Linq;


[assembly: CommandClass(typeof(SiteCalculations.Main))]

namespace SiteCalculations
{
    internal class Main
    {
        [CommandMethod("testtest")]
        public void testtest()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            var f =  new Functions();
            List<EntityBorderModel> borders = f.GetBorders("Домодедово");
            List<AmenitiesModel> exParams = f.GetExAmenities();
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
                new ParkingModel("ГП-10", 400, 0, 0)
            };
            //getting separate building
            List<IBaseBuilding> allBuildings = f.GetBuildings(city, borders, exParams, exParking);
            //Getting sections
            List<ApartmentBuildingSectionModel> allSections = f.GetSecttions();
            //Sorting sections by building name
            List<List<ApartmentBuildingSectionModel>> sectionsByBuilding = f.SortListOfObjectsByParameterToSeparateLists(allSections, "Name");
            //Creating Buildings from sections and adding them to main pool
            for (int i = 0; i < sectionsByBuilding.Count; i++)
            {
                allBuildings.Add(new ApartmentBuildingModel(city, sectionsByBuilding[i], borders.FirstOrDefault(c => c.Name == sectionsByBuilding[i][0].Name).Area, exParams.FirstOrDefault(c => c.Name == sectionsByBuilding[i][0].Name), exParking.FirstOrDefault(c => c.Name == sectionsByBuilding[i][0].Name)));
            }
            //Sorting buildings by name
            List<IBaseBuilding> sortedBuildings = f.Sort_List_By_PropertyName_Generic("Ascending", "Name", allBuildings);
            //Sorting buildings by stage
            List<List<IBaseBuilding>> buildingsByStage = f.SortListOfObjectsByParameterToSeparateLists(sortedBuildings, "StageName");
            //Creating Site
            List<BaseBigAreaModel> stages= new List<BaseBigAreaModel>(); ;
            for (int i = 0; i < buildingsByStage.Count; i++)
            {
                stages.Add(new StageModel(city, borders.FirstOrDefault(c => c.Name == buildingsByStage[i][0].StageName).Area, buildingsByStage[i]));
            }
            SiteModel site = new SiteModel(city, "Домодедово", borders.FirstOrDefault(c => c.Name == "Домодедово").Area, stages);
            List<BaseBigAreaModel> sortedStages = f.Sort_List_By_PropertyName_Generic("Ascending","Name",stages);
            /*var parkings = f.GetExParkingBlocks();
            foreach (var par in parkings)
            {
                ed.WriteMessage(par.NumberOfParkings.ToString() + "\n");
            }*/
            f.CreateSiteTable(site);
            f.CreateSiteTable(site, sortedStages);
            f.CreateSiteTable(site, sortedStages, buildingsByStage);
            f.CreateSiteTable(null, null, null, sortedStages[0], buildingsByStage[0]);
        }
        [CommandMethod("CAIAID")]
        public void CalculateAndInsertAmenitiesInDrawing()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            var f = new Functions();
            AreaReqFromPeopleModel areaReq = new AreaReqFromPeopleModel(0.5, 0.7, 0.1, 0.3, 0.03, 0.1, 0, 2); ;
            ParkingFromPeopleModel parking = new ParkingFromPeopleModel(0.42, 0.0756, 0.016, 0.01, 0.01, 0.01);
            CityModel city = new CityModel("test", 58.1, 35, 0.135, 0.065, 0.05, parking, areaReq);
            ed.WriteMessage("making buildings list");
            //Getting separate buildings
            List<IBaseBuilding> allBuildings = f.GetBuildings(city, null, null, null, true);
            List<ApartmentBuildingSectionModel> allSections = f.GetSecttions();
            //Sorting sections by building name
            List<List<ApartmentBuildingSectionModel>> sectionsByBuilding = f.SortListOfObjectsByParameterToSeparateLists(allSections, "Name");
            //Creating Buildings from sections and adding them to main pool
            for (int i = 0; i < sectionsByBuilding.Count; i++)
            {
                allBuildings.Add(new ApartmentBuildingModel(city, sectionsByBuilding[i]));
            }
            foreach (var item in allBuildings)
            {
                if (item is ApartmentBuildingModel mod)
                {
                    ed.WriteMessage("creating block \n");
                    f.CreateAmenitiesBlock(mod.AmenitiesReq, mod.MidPoint);
                }
            }
        }
    }
}
