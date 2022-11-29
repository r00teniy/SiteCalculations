﻿using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.Geometry;
using System.Reflection;
using SiteCalculations.Models;
using System.Collections;
using System.Security.Cryptography;


namespace SiteCalculations
{
    public class Functions
    {
        //General stuff
        public Document doc = Application.DocumentManager.MdiActiveDocument;
        public Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        public Database db = Application.DocumentManager.MdiActiveDocument.Database;
        public RXClass rxClassBlockReference = RXClass.GetClass(typeof(BlockReference));
        public RXClass rxClassPolyline = RXClass.GetClass(typeof(Polyline));
        //Layers
        string sectionsLayer = "20_Секции";
        string ApartmentsBuildingsLayer = "21_Жилые_дома";
        string SchoolBuildingsLayer = "22_Школы";
        string KindergartenBuildingsLayer = "23_Садики";
        string HospitalBuildingsLayer = "24_Больницы";
        string ParkingBuildingsLayer = "25_Паркинги";
        string buildingBorderLayer = "13_Граница_";
        string stageBorderLayer = "12_Граница_";
        string siteBorderLayer = "11_Граница_площадки";
        string buildingAreaExLayer = "31_Площадки_проект";
        //Arrays with data for table
        string[] generalParamArray = { "PlotArea", "TotalConstructionArea", "BuildingPartPercent",
            "TotalApartmentArea", "TotalNumberOfApartments", "TotalResidents", "TotalCommerceArea" };
        string[] livingReqParamArray = { "TotalAreaReq", "TotalChildAreaReq", "TotalSportAreaReq", "TotalRestAreaReq", 
            "TotalUtilityAreaReq", "TotalTrashAreaReq", "TotalDogsAreaReq", "TotalGreeneryAreaReq" };
        string[] livingExParamArray = { "TotalAreaEx", "TotalChildAreaEx", "TotalSportAreaEx", "TotalRestAreaEx",
            "TotalDogsAreaEx", "TotalTrashAreaEx", "TotalUtilityAreaEx", "TotalGreeneryAreaEx" };
        string[] socialReqParamArray = { "SchoolsReq", "KindergartensReq", "HospitalsReq" };
        string[] socialExParamArray = { "SchoolsEx", "KindergartensEx", "HospitalsEx" };
        string[] parkingParamArray = { "TotalLongParking", "TotalShortParking", "TotalGuestParking" };
        string[] TopOfTheTableArray = { "Имя", "Параметр", "Площадь участка, м2", "Площадь зайтройки, м2", "Процент застройки, %",
                "Площадь квартир", "Кол-во квартир", "Кол-во жителей", "Площадь нежилья", "Общая S площадок, в т.ч:", "- детских площадок",
                "- спортивных площадок", "- площадок отдыха", "- хозяйственных площадок", "- площадок для мусороконтейнеров", "- площадок выгула собак",
                "Площадь озеленения", "Кол-во парковок для постоянного храниния а/м", "Кол-во парковок для временного хранения а/м", "Кол-во гостевых парковок",
                "Кол-во мест в Школах", "Кол-во мест в садиках", "Кол-во посетителей в день в больницах" };
        // Table parameters
        double row_height = 8;
        double column_width = 12;
        public List<ExParametersModel> GetExParameters()
        {
            List<ExParametersModel> output = new List<ExParametersModel>();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                using (DocumentLock acLckDoc = doc.LockDocument())
                {
                    var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;
                    var btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead, false) as BlockTableRecord;
                    foreach (ObjectId objectId in btr)
                    {
                        if (objectId.ObjectClass == rxClassBlockReference)
                        {
                            var br = tr.GetObject(objectId, OpenMode.ForRead) as BlockReference;
                            if (br.Layer == buildingAreaExLayer && br != null)
                            {
                                string[] dynBlockPropValues = new string[9];
                                var pc = br.DynamicBlockReferencePropertyCollection;
                                for (int i = 0; i < 9; i++) // It only hav 9 properties
                                    {
                                        dynBlockPropValues[i] = pc[i].Value.ToString();
                                    }
                                output.Add(new ExParametersModel(dynBlockPropValues));
                            }
                        }
                    }
                }
                tr.Commit();
            }
            return output;
        }
        public List<ApartmentBuildingSectionModel> GetSecttions()
        {
            List<ApartmentBuildingSectionModel> output= new List<ApartmentBuildingSectionModel>();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                using (DocumentLock acLckDoc = doc.LockDocument())
                {
                    var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;
                    var btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead, false) as BlockTableRecord;
                    foreach (ObjectId objectId in btr)
                    {
                        if (objectId.ObjectClass == rxClassBlockReference)
                        {
                            var br = tr.GetObject(objectId, OpenMode.ForRead) as BlockReference;
                            if (br.Layer == sectionsLayer && br != null)
                            {
                                string[] dynBlockPropValues = new string[8];
                                DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                for (int i = 0; i < 8; i++) // we only need first 8 properties
                                {
                                    dynBlockPropValues[i] = pc[i].Value.ToString();
                                }
                                output.Add(new ApartmentBuildingSectionModel(dynBlockPropValues));
                            }
                        }
                    }
                }
                tr.Commit();
            }
            return output;
        }
        public List<IBaseBuilding> GetBuildings(CityModel city, List<EntityBorderModel> borders, List<ExParametersModel> exParams, List<ParkingModel> parking)
        {
            List<IBaseBuilding> output = new List<IBaseBuilding>();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                using (DocumentLock acLckDoc = doc.LockDocument())
                {
                    var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;
                    var btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead, false) as BlockTableRecord;
                    foreach (ObjectId objectId in btr)
                    {
                        if (objectId.ObjectClass == rxClassBlockReference)
                        {
                            var br = tr.GetObject(objectId, OpenMode.ForRead) as BlockReference;
                            //Apartment buildings
                            if (br.Layer == ApartmentsBuildingsLayer && br != null)
                            {
                                string[] dynBlockPropValues = new string[7];
                                DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                for (int i = 0; i < 7; i++)
                                {
                                    dynBlockPropValues[i] = pc[i].Value.ToString();
                                }
                                output.Add(new ApartmentBuildingModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]).Area, exParams.FirstOrDefault(c => c.BuildingName == dynBlockPropValues[1]), parking.FirstOrDefault(c => c.Name == dynBlockPropValues[1])));
                            }
                            //Schools
                            else if (br.Layer == SchoolBuildingsLayer && br != null)
                            {
                                string[] dynBlockPropValues = new string[6];
                                DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                for (int i = 0; i < 6; i++)
                                {
                                    dynBlockPropValues[i] = pc[i].Value.ToString();
                                }
                                output.Add(new SchoolModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]).Area, parking.FirstOrDefault(c => c.Name == dynBlockPropValues[1])));
                            }
                            //Kindergartens
                            else if (br.Layer == KindergartenBuildingsLayer && br != null)
                            {
                                string[] dynBlockPropValues = new string[6];
                                DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                for (int i = 0; i < 6; i++)
                                {
                                    dynBlockPropValues[i] = pc[i].Value.ToString();
                                }
                                output.Add(new KindergartenModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]).Area, parking.FirstOrDefault(c => c.Name == dynBlockPropValues[1])));
                            }
                            //Hospitals
                            else if (br.Layer == HospitalBuildingsLayer && br != null)
                            {
                                string[] dynBlockPropValues = new string[6];
                                DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                for (int i = 0; i < 6; i++)
                                {
                                    dynBlockPropValues[i] = pc[i].Value.ToString();
                                }
                                output.Add(new HospitalModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]).Area, parking.FirstOrDefault(c => c.Name == dynBlockPropValues[1])));
                            }
                            //Parking
                            else if (br.Layer == ParkingBuildingsLayer && br != null)
                            {
                                string[] dynBlockPropValues = new string[6];
                                DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                for (int i = 0; i < 6; i++)
                                {
                                    dynBlockPropValues[i] = pc[i].Value.ToString();
                                }
                                output.Add(new ParkingBuildingModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]).Area, parking.FirstOrDefault(c => c.Name == dynBlockPropValues[1])));
                            }
                            //Add for Trade centers, office buildings,etc.
                        }
                    }
                }
                tr.Commit();
            }
            return output;
        }
        public List<EntityBorderModel> GetBorders(string siteName)
        {
            List<EntityBorderModel> output = new List<EntityBorderModel>();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                using (DocumentLock acLckDoc = doc.LockDocument())
                {
                    var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;
                    var btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead, false) as BlockTableRecord;
                    foreach (ObjectId objectId in btr)
                    {
                        if (objectId.ObjectClass == rxClassPolyline)
                        {
                            var pl = tr.GetObject(objectId, OpenMode.ForRead) as Polyline;
                            if (pl.Layer.Contains(buildingBorderLayer) && pl != null)
                            {
                                var bdName = pl.Layer.Replace(buildingBorderLayer, "");
                                output.Add(new EntityBorderModel(bdName, pl.Area));

                            }
                            else if (pl.Layer.Contains(stageBorderLayer) && pl != null)
                            {
                                var stName = pl.Layer.Replace(stageBorderLayer, "");
                                output.Add(new EntityBorderModel(stName, pl.Area));
                            }
                            else if (pl.Layer == siteBorderLayer && pl != null)
                            {
                                output.Add(new EntityBorderModel(siteName, pl.Area));
                            }
                        }
                    }
                }
                tr.Commit();
            }
            return output;
        }
        public Point3d GetInsertionPoint()
        {
            PromptPointResult pPtRes;
            PromptPointOptions pPtOpts = new PromptPointOptions("");
            pPtOpts.Message = "\nВыберете точку положения таблицы: ";
            pPtRes = ed.GetPoint(pPtOpts);
            Point3d pt = pPtRes.Value;
            return pt;
        }
        public void CreateSiteTable(SiteModel site = null, List<BaseBigAreaModel> stages = null, List<List<IBaseBuilding>> buildingsByStage = null, BaseBigAreaModel stage = null, List<IBaseBuilding> buildings = null)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                using (DocumentLock acLckDoc = doc.LockDocument())
                {
                    var blockTable = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;
                    var blocktableRecord = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false) as BlockTableRecord;
                    //Adding stages to site if they are provided.
                    List<BaseBigAreaModel> bigcol = new List<BaseBigAreaModel>();
                    if (site !=null)
                    {
                        bigcol.Add(site);
                    }
                    if (stages != null)
                    {
                        bigcol.AddRange(stages);
                    }
                    if (stage != null)
                    {
                        bigcol.Add(stage);
                    }
                    Table tb = new Table
                    {
                        TableStyle = db.Tablestyle,
                        Position = GetInsertionPoint()
                    };
                    tb.InsertRows(1, 30, 1);
                    tb.InsertColumns(1, 25, 1);
                    tb.SetColumnWidth(25);
                    tb.InsertColumns(3, column_width, 21);
                    //First line
                    tb.Cells[0, 0].TextString = "ТЭП площадки " + bigcol[0].Name; // TODO: FIX FOR DIFFERENT OPTIONS
                    //for tracking
                    int current_row = 1;
                    //filling top row
                    for (var i = 0;i < TopOfTheTableArray.Length;i++)
                    {
                        tb.Cells[current_row, i].TextString = TopOfTheTableArray[i];
                    }
                    current_row++;
                    // sites and stages
                    foreach (var item in bigcol)
                    {
                        AddAllDataForSingleObject(ref tb, ref current_row, 2, item);
                        if (buildingsByStage != null)
                        {
                            int bId = 0;
                            for (int i = 0; i < buildingsByStage.Count; i++)
                            {
                                if (buildingsByStage[i][0].StageName == item.Name) { bId = i; }
                            }
                            foreach (var bItem in buildingsByStage[bId])
                            {
                                AddAllDataForSingleObject(ref tb, ref current_row, 2, bItem);
                            }
                        }
                        if (buildings != null)
                        {
                            foreach (var bem in buildings)
                            {
                                AddAllDataForSingleObject(ref tb, ref current_row, 2, bem);
                            }
                        }
                    }
                    tb.GenerateLayout();
                    blocktableRecord.AppendEntity(tb);
                    tr.AddNewlyCreatedDBObject(tb, true);
                }
                tr.Commit();
            }
        }
        public void AddAllDataForSingleObject<T>(ref Table tb, ref int current_row, int starting_column,T obj)
        {
            tb.InsertRows(current_row, row_height, 2);
            tb.Cells[current_row, 1].TextString = "Требуется:";
            tb.Cells[current_row + 1, 1].TextString = "Запроектировано:";
            CellRange nameRange = CellRange.Create(tb, current_row, 0, current_row + 1, 0);
            tb.MergeCells(nameRange);
            tb.Cells[current_row, 0].TextString = GetObjectPropertyByName(obj, "Name").ToString();
            //Getting separate objects
            ParkingModel parking = GetObjectPropertyByName(obj, "TotalParkingReq") as ParkingModel;
            //for tracking2
            int current_column = starting_column;
            //Adding reneral parameters
            AddObjectDataToRowFromParameterArray(ref tb, current_row + 1, ref current_column, generalParamArray, obj, 0, true);
            //Adding area parameters
            AddObjectDataToRowFromParameterArray(ref tb, current_row, ref current_column, livingReqParamArray, obj);
            AddObjectDataToRowFromParameterArray(ref tb, current_row + 1, ref current_column, livingExParamArray, obj, 0, true);
            //Adding parking parameters
            AddObjectDataToRowFromParameterArray(ref tb, current_row, ref current_column, parkingParamArray, parking);
            AddObjectDataToRowFromParameterArray(ref tb, current_row + 1, ref current_column, parkingParamArray, parking, 0, true);
            //Adding social parameters
            AddObjectDataToRowFromParameterArray(ref tb, current_row, ref current_column, socialReqParamArray, obj);
            AddObjectDataToRowFromParameterArray(ref tb, current_row + 1, ref current_column, socialReqParamArray, obj, 0, true);
            current_row += 2;
        }
        //Function adds data from object, parameters are in array
        public void AddObjectDataToRowFromParameterArray<T>(ref Table tb, int current_row, ref int current_column, string[] array, T obj, int startPositionInArray = 0, bool ChangeCurrentColumn = false)
        {
            for (int i = startPositionInArray; i < array.Length; i++)
            {
                if (obj.GetType().GetProperty(array[i]) != null)
                {
                    tb.Cells[current_row, current_column + i - startPositionInArray].TextString = obj.GetType().GetProperty(array[i]).GetValue(obj, null).ToString();
                }
                else
                {
                    tb.Cells[current_row, current_column + i - startPositionInArray].TextString = " ";
                }
            }
            if (ChangeCurrentColumn)
            {
                current_column += array.Length - startPositionInArray;
            }
        }
        //Sort List<T> generic
        public List<T> Sort_List_By_PropertyName_Generic<T>(string sortDirection, string propName, List<T> data)
        {
            List<T> data_sorted = new List<T>();
            if (sortDirection == "Ascending")
            {
                data_sorted = (from n in data orderby GetObjectPropertyByName(n, propName) ascending select n).ToList();
            }
            else if (sortDirection == "Descending")
            {
                data_sorted = (from n in data orderby GetObjectPropertyByName(n, propName) descending select n).ToList();
            }
            return data_sorted;
        }
        public object GetObjectPropertyByName(object item, string propName)
        {
            //Use reflection to get order type
            return item.GetType().GetProperty(propName).GetValue(item, null);
        }
        public List<List<T>> SortListOfObjectsByParameterToSeparateLists<T>(List<T> objects, string parameter)
        {
            List<List<T>> sortedList = new List<List<T>>();
            foreach (var ob in objects)
            {
                int obId = -1;
                for (int i = 0; i < sortedList.Count; i++)
                {
                    if (GetObjectPropertyByName(ob, parameter).ToString() == GetObjectPropertyByName(sortedList[i][0], parameter).ToString())
                    {
                        obId = i;
                    }
                }
                if (obId == -1)
                {
                    obId = sortedList.Count;
                    sortedList.Add(new List<T>());
                }
                sortedList[obId].Add(ob);
            }
            return sortedList;
        }
    }
}
