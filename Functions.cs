﻿using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.BoundaryRepresentation;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.Geometry;
using System.Reflection;
using SiteCalculations.Models;
using System;
using Autodesk.AutoCAD.Colors;
using AcBr = Autodesk.AutoCAD.BoundaryRepresentation;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace SiteCalculations
{
    public class Functions
    {
        //General stuff
        Document doc = Application.DocumentManager.MdiActiveDocument;
        Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
        Database db = Application.DocumentManager.MdiActiveDocument.Database;
        RXClass rxClassBlockReference = RXClass.GetClass(typeof(BlockReference));
        RXClass rxClassPolyline = RXClass.GetClass(typeof(Polyline));
        RXClass rxClassText = RXClass.GetClass(typeof(DBText));
        //Layers
        string sectionsLayer = "20_Секции";
        string apartmentsBuildingsLayer = "21_Жилые_дома";
        string schoolBuildingsLayer = "22_Школы";
        string kindergartenBuildingsLayer = "23_Садики";
        string hospitalBuildingsLayer = "24_Больницы";
        string parkingBuildingsLayer = "25_Паркинги";
        public static string buildingBorderLayer = "13_Граница_";
        public static string stageBorderLayer = "12_Граница_";
        public static string siteBorderLayer = "11_Граница_площадки";
        public static string plotNumbersLayer = "10_Номера_участков";
        public string parkTableStyleName = "ГП Таблица паркомест";
        string buildingAreaExLayer = "31_Площадки_проект";
        string amenitiesLayerName = "30_Площадки_требуемые";
        public static List<ParkingReqModel> parkingCalcTypeList = new List<ParkingReqModel>();
        public static List<AmenitiesReqModel> amenitiesCalcTypeList = new List<AmenitiesReqModel>();
        public static List<CityModel> cityCalcTypeList = new List<CityModel>();
        //Parameters_of_dyn_blocks
        string[] parkingBlockPararmArray = { "Размер", "Тип", "Обычн_МГН", "РасширенноеММ" };
        //Arrays with data for table
        string[] parkingTypes = { "Постоянные", "Временные", "Гостевые", "Всего", "в т.ч. расш." };
        string[] generalParamArray = { "PlotArea", "TotalConstructionArea", "BuildingPartPercent", "NumberOfFloors",
            "TotalApartmentArea", "TotalNumberOfApartments", "TotalResidents", "TotalCommerceArea" };// TODO: add Number of floors
        string[] AmenitiesParamArrayReq = { "AmenitiesReq.TotalArea", "AmenitiesReq.ChildrenArea", "AmenitiesReq.SportArea", "AmenitiesReq.RestArea",
            "AmenitiesReq.UtilityArea", "AmenitiesReq.TrashArea", "AmenitiesReq.DogsArea", "AmenitiesReq.GreeneryArea" };
        string[] AmenitiesParamArrayEx = { "AmenitiesEx.TotalArea", "AmenitiesEx.ChildrenArea", "AmenitiesEx.SportArea", "AmenitiesEx.RestArea",
            "AmenitiesEx.UtilityArea", "AmenitiesEx.TrashArea", "AmenitiesEx.DogsArea", "AmenitiesEx.GreeneryArea" };
        string[] socialReqParamArray = { "SchoolsReq", "KindergartensReq", "HospitalsReq" };// remake as a class? worth it?
        string[] socialExParamArray = { "SchoolsEx", "KindergartensEx", "HospitalsEx" };
        string[] parkingParamArrayReq = { "TotalParkingReq.TotalLongParking", "TotalParkingReq.TotalShortParking", "TotalParkingReq.TotalGuestParking" };
        string[] parkingParamArrayEx = { "TotalParkingEx.TotalLongParking", "TotalParkingEx.TotalShortParking", "TotalParkingEx.TotalGuestParking" };
        string[] topOfTheTableArray = { "Имя", "Параметр", "Площадь участка, м2", "Площадь зайтройки, м2", "Процент застройки, %", "Кол-во этажей",
                "Площадь квартир", "Кол-во квартир", "Кол-во жителей", "Площадь нежилья", "Общая S площадок, в т.ч:", "- детских площадок",
                "- спортивных площадок", "- площадок отдыха", "- хозяйственных площадок", "- площадок для мусороконтейнеров", "- площадок выгула собак",
                "Площадь озеленения", "Кол-во парковок для постоянного храниния а/м", "Кол-во парковок для временного хранения а/м", "Кол-во гостевых парковок",
                "Кол-во мест в Школах", "Кол-во мест в садиках", "Кол-во посетителей в день в больницах" };
        // Table parameters
        double row_height = 8;
        double column_width = 12;
        //Functions for parking table
        //Get list of all buildings for table
        public List<string> GetSortedListOfParameterValues<T>(List<T> list, string paramName)
        {
            var output = new List<string>();
            //Adding all unique values to a list
            foreach (var block in list)
            {
                if (!output.Contains(GetObjectPropertyByName(block, paramName).ToString()))
                {
                    output.Add(GetObjectPropertyByName(block, paramName).ToString());
                }
            }
            //sorting
            try
            {
                int num = Convert.ToInt32(Regex.Match(output[0], @"\d+").Value);
                return SortListOfStringByNumbersInIt(output);
            }
            catch
            {
                output.Sort();
                return output;
            }    
        }
        public string[] CreateLineForParkingTable(List<ParkingBlockModel> parkingBlocks, List<string> names, string plotNumber, List<BuildingBorderModel> borders, bool isParkingBuilding = false)
        {
            string[] output = new string[names.Count*5+3];
            int[] parkingNumbers = new int[names.Count * 5];
            output[0] = plotNumber;
            output[1] = GetOnePropetyFromListOfObjectsBySecondPropertyValue(borders, "Name", "PlotNumber", plotNumber).ToString();
            //creating array for this plot
            foreach (var park in parkingBlocks)
            {
                if (park.PlotNumber == plotNumber)
                {
                    for (int i = 0; i < names.Count; i++)
                    {
                        if (park.ParkingIsForBuildingName == names[i])
                        {
                            switch (park.Type)
                            {
                                case "Long":
                                    parkingNumbers[i*5] += park.NumberOfParkings;
                                    break;
                                case "Short":
                                    parkingNumbers[i * 5 + 1] += park.NumberOfParkings;
                                    if (park.IsForDisabled)
                                    {
                                        parkingNumbers[i * 5 + 3] += park.NumberOfParkings;
                                    }
                                    if (park.IsForDisabledExtended)
                                    {
                                        parkingNumbers[i * 5 + 4] += park.NumberOfParkings;
                                    }
                                    break;
                                case "Guest":
                                    parkingNumbers[i * 5 + 2] += park.NumberOfParkings;
                                    if (park.IsForDisabled)
                                    {
                                        parkingNumbers[i * 5 + 3] += park.NumberOfParkings;
                                    }
                                    if (park.IsForDisabledExtended)
                                    {
                                        parkingNumbers[i * 5 + 4] += park.NumberOfParkings;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            if (parkingNumbers.Sum() != 0)
            {
                //Creating output string array
                output[output.Length - 1] = parkingNumbers.Sum().ToString();
                for (int i = 0; i < parkingNumbers.Length; i++)
                {
                    output[i + 2] = parkingNumbers[i].ToString();
                }
                return output;
            }
            else
            {
                return null;
            }
            
        }
        //Generate table.
        public void CreateParkingTable(List<string[]> list, List<string> buildingNames, List<ParkingModel> parkingReq, string siteName)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                using (DocumentLock acLckDoc = doc.LockDocument())
                {
                    var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;
                    var btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false) as BlockTableRecord;
                    ObjectId tbSt = new ObjectId();
                    DBDictionary tsd = (DBDictionary)tr.GetObject(db.TableStyleDictionaryId, OpenMode.ForRead);
                    foreach (DBDictionaryEntry entry in tsd)
                    {
                        var tStyle = (TableStyle)tr.GetObject(entry.Value, OpenMode.ForRead);
                        if (tStyle.Name == parkTableStyleName)
                        {
                            tbSt = entry.Value;
                        }
                    }
                    //Creating required row for table.
                    string[] req = new string[list[0].Length];
                    req[0] = "Требуемые";
                    req[1] = "Машиноместа";
                    for (int i = 0; i < parkingReq.Count; i++)
                    {
                        req[2 + i * 5] = parkingReq[i].TotalLongParking.ToString();
                        req[3 + i * 5] = parkingReq[i].TotalShortParking.ToString();
                        req[4 + i * 5] = parkingReq[i].TotalGuestParking.ToString();
                        req[5 + i * 5] = parkingReq[i].TotalDisabledParking.ToString();
                        req[6 + i * 5] = parkingReq[i].TotalDisabledBigParking.ToString();
                    }
                    //Calculating summ of existing parkings
                    string[] ex = new string[list[0].Length];
                    ex[0] = "Всего";
                    ex[1] = "на дом";
                    for (int i = 2; i < list[0].Length; i++)
                    {
                        ex[i] = list.Sum(x => Convert.ToInt32(x[i])).ToString();
                    }
                    req[list[0].Length - 1] = parkingReq.Sum(x => x.TotalLongParking + x.TotalShortParking + x.TotalGuestParking).ToString();
                    //Creating table
                    Table tb = new Table
                    {
                        TableStyle = tbSt,
                        Position = GetInsertionPoint()
                    };
                    //Creating title
                    tb.Rows[0].Style = "Название";
                    tb.Cells[0, 0].TextString = $"Распределение парковок по домам и участкам на площадке {siteName}";
                    //Creating header
                    tb.SetRowHeight(8);
                    tb.SetColumnWidth(8);
                    tb.InsertRows(1, 10, 1);
                    tb.InsertRows(2, 8, 1);
                    tb.Rows[1].Style = "Заголовок";
                    tb.InsertRows(3, 16, 1);
                    tb.Rows[2].Style = "Данные";
                    tb.InsertColumns(1, 30, 1);
                    tb.InsertColumns(2, 6, list[0].Length - 3);
                    tb.InsertColumns(list[0].Length - 2, 8, 1);

                    for (int i = 0; i < buildingNames.Count; i++)
                    {
                        CellRange nameRange = CellRange.Create(tb, 1, 2+i*5, 1, 2+i*5+4);
                        tb.MergeCells(nameRange);
                    }
                    CellRange range = CellRange.Create(tb,1, 0, 1, 1);
                    tb.MergeCells(range);
                    tb.Cells[1,0].TextString = "Позиция";
                    range = CellRange.Create(tb, 2, 0, 3, 1);
                    tb.MergeCells(range);
                    tb.Cells[2, 0].TextString = "Номер Участка";
                    range = CellRange.Create(tb, 1, list[0].Length - 1, 3, list[0].Length - 1);
                    tb.MergeCells(range);
                    tb.Cells[1, list[0].Length - 1].TextString = "Итого по участку";
                    tb.Cells[1, list[0].Length - 1].Contents[0].Rotation = Math.PI / 2;
                    for (var i = 0;i  < buildingNames.Count;i++)
                    {
                        tb.Cells[1, 2+i*5].TextString = buildingNames[i];
                        for (int j = 0; j < 5; j++)
                        {
                            if (j < 3)
                            {
                                range = CellRange.Create(tb, 2, 2+i*5+j, 3, 2+i*5+j);
                                tb.MergeCells(range);
                                tb.Cells[2, 2 + i * 5 + j].TextString = parkingTypes[j];
                                tb.Cells[2, 2 + i * 5 + j].Contents[0].Rotation = Math.PI / 2;
                            }
                            tb.Cells[3, 2 + i * 5 + j].TextString = parkingTypes[j];
                            tb.Cells[3, 2 + i * 5 + j].Contents[0].Rotation = Math.PI / 2;
                        }
                        range = CellRange.Create(tb, 2, 2 + i * 5 + 3, 2, 2 + i * 5 + 4);
                        tb.MergeCells(range);
                        tb.Cells[2, 2 + i * 5 + 3].TextString = "в т.ч. МГН";
                    }
                    tb.InsertRows(4, 8, 1);
                    var curCell = 0;
                    foreach (var item in req)
                    {
                        tb.Cells[4, curCell].TextString = item;
                        curCell++;
                    }
                    curCell = 0;
                    tb.InsertRows(5, 8, 1);
                    foreach (var item in ex)
                    {
                        tb.Cells[5, curCell].TextString = item;
                        curCell++;
                    }
                    var currentRow = 5;
                    curCell = 0;
                    foreach (var arr in list)
                    {
                        tb.InsertRows(currentRow+1, 8, 1);
                        currentRow++;
                        for (int i = 0; i < arr.Length; i++)
                        {
                            tb.Cells[currentRow, i].TextString = arr[i];
                        }
                    }
                    tb.GenerateLayout();
                    btr.AppendEntity(tb);
                    tr.AddNewlyCreatedDBObject(tb, true);
                }
                tr.Commit();
            }
        }
        //Function that creates parking models for buildings for existing parking
        public List<ParkingModel> CreateExParking(List<ParkingBlockModel> blockList, List<BuildingBorderModel> borders)
        {
            List<ParkingModel> output= new List<ParkingModel>();
            var sortedBlockList = SortListOfObjectsByParameterToSeparateLists(blockList, "PlotNumber");
            
            foreach (var item in sortedBlockList)
            {
                output.Add(new ParkingModel(item, SearchByPropNameAndValue(borders, "PlotNumber", item[0].PlotNumber)));
            }
            return output;
        }
        //Function that creates parking block models for existing parking parts
        public List<ParkingBlockModel> GetExParkingBlocks(List<BuildingBorderModel> borders)
        {
            List<ParkingBlockModel> output = new List<ParkingBlockModel>();
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
                            try
                            {
                                ObjectId oi = br.AttributeCollection[1];
                                var attRef = tr.GetObject(oi, OpenMode.ForRead) as AttributeReference;
                                ObjectId oi2= br.AttributeCollection[0];
                                var attRef2= tr.GetObject(oi2, OpenMode.ForRead) as AttributeReference;
                                ObjectId oi3 = br.AttributeCollection[3];
                                var attRef3 = tr.GetObject(oi3, OpenMode.ForRead) as AttributeReference;

                                if (attRef.Tag == "Этап" && attRef2.Tag == "КОЛ-ВО")
                                {
                                    string[] dynBlockPropValues = new string[7];
                                    var pc = br.DynamicBlockReferencePropertyCollection;
                                    for (var j = 0; j < pc.Count; j++)
                                    {
                                        for (int i = 0; i < parkingBlockPararmArray.Length; i++)
                                        {
                                            if (pc[j].PropertyName == parkingBlockPararmArray[i])
                                            { dynBlockPropValues[i] = pc[j].Value.ToString(); }
                                        }
                                    }
                                    dynBlockPropValues[4] = attRef.TextString;
                                    dynBlockPropValues[5] = attRef2.TextString;
                                    dynBlockPropValues[6] = attRef3.TextString;
                                    output.Add(new ParkingBlockModel(dynBlockPropValues));
                                }
                            }
                            catch { }
                            if (br.Layer == parkingBuildingsLayer && br != null)
                            {
                                var pc = br.DynamicBlockReferencePropertyCollection;
                                var plotNumbner = GetOnePropetyFromListOfObjectsBySecondPropertyValue(borders, "PlotNumber", "Name", pc[1].Value.ToString()).ToString();
                                for (var i = 8; i < 22; i += 2)
                                {
                                    if (Convert.ToInt32(pc[i + 1].Value) != 0)
                                    {
                                        output.Add(new ParkingBlockModel(Convert.ToInt32(pc[i + 1].Value), pc[i].Value.ToString(), plotNumbner));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return output;
        }
        public List<AmenitiesModel> GetExAmenities()
        {
            List<AmenitiesModel> output = new List<AmenitiesModel>();
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
                                output.Add(new AmenitiesModel(dynBlockPropValues));
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
                                string[] dynBlockPropValues = new string[10];
                                DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                for (int i = 0; i < 10; i++) // we only need first 10 properties
                                {
                                    dynBlockPropValues[i] = pc[i].Value.ToString();
                                }
                                output.Add(new ApartmentBuildingSectionModel(dynBlockPropValues , GetCenterOfABlock(br)));
                            }
                        }
                    }
                }
                tr.Commit();
            }
            return output;
        }
        public List<IBaseBuilding> GetBuildings(CityModel city, List<BuildingBorderModel> borders = null, List<AmenitiesModel> exParams = null, List<ParkingModel> exParking = null, bool onlyGetReqParams = false)
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
                            Point3d midPoint = GetCenterOfABlock(br);
                            if (onlyGetReqParams)
                            {
                                //Apartment buildings
                                if (br.Layer == apartmentsBuildingsLayer && br != null)
                                {
                                    string[] dynBlockPropValues = new string[9];
                                    DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                    for (int i = 0; i < dynBlockPropValues.Length; i++)
                                    {
                                        dynBlockPropValues[i] = pc[i].Value.ToString();
                                    }
                                    output.Add(new ApartmentBuildingModel(city, dynBlockPropValues, midPoint));
                                }
                            }
                            else
                            {
                                //Apartment buildings
                                if (br.Layer == apartmentsBuildingsLayer && br != null)
                                {
                                    string[] dynBlockPropValues = new string[9];
                                    DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                    for (int i = 0; i < dynBlockPropValues.Length; i++)
                                    {
                                        dynBlockPropValues[i] = pc[i].Value.ToString();
                                    }
                                    output.Add(new ApartmentBuildingModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), exParams.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), exParking.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), midPoint));
                                }
                                //Schools
                                else if (br.Layer == schoolBuildingsLayer && br != null)
                                {
                                    string[] dynBlockPropValues = new string[6];
                                    DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                    for (int i = 0; i < 6; i++)
                                    {
                                        dynBlockPropValues[i] = pc[i].Value.ToString();
                                    }
                                    output.Add(new SchoolModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), exParking.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), midPoint));
                                }
                                //Kindergartens
                                else if (br.Layer == kindergartenBuildingsLayer && br != null)
                                {
                                    string[] dynBlockPropValues = new string[6];
                                    DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                    for (int i = 0; i < 6; i++)
                                    {
                                        dynBlockPropValues[i] = pc[i].Value.ToString();
                                    }
                                    output.Add(new KindergartenModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), exParking.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), midPoint));
                                }
                                //Hospitals
                                else if (br.Layer == hospitalBuildingsLayer && br != null)
                                {
                                    string[] dynBlockPropValues = new string[6];
                                    DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                    for (int i = 0; i < 6; i++)
                                    {
                                        dynBlockPropValues[i] = pc[i].Value.ToString();
                                    }
                                    output.Add(new HospitalModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), exParking.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), midPoint));
                                }
                                //Parking
                                else if (br.Layer == parkingBuildingsLayer && br != null)
                                {
                                    string[] dynBlockPropValues = new string[8];
                                    DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                    for (int i = 0; i < 8; i++)
                                    {
                                        dynBlockPropValues[i] = pc[i].Value.ToString();
                                    }
                                    output.Add(new ParkingBuildingModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), exParking.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), midPoint));
                                }
                                //Add for Trade centers, office buildings,etc.
                            }
                        }
                    }
                }
                tr.Commit();
            }
            return output;
        }
        //Function to get all informations on borders needed for model
        public List<BuildingBorderModel> GetBorders(string siteName)
        {
            List<BuildingBorderModel> output = new List<BuildingBorderModel>();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                using (DocumentLock acLckDoc = doc.LockDocument())
                {
                    var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;
                    var btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead, false) as BlockTableRecord;
                    List<Polyline> buildingBordersList = new List<Polyline>();
                    List<string> buildingBordersNamesList = new List<string>();
                    List<DBText> buildingBorderTextobjects = new List<DBText>();
                    foreach (ObjectId objectId in btr)
                    {
                        if (objectId.ObjectClass == rxClassPolyline)
                        {
                            var pl = tr.GetObject(objectId, OpenMode.ForRead) as Polyline;
                            if (pl.Layer.Contains(buildingBorderLayer) && pl != null)
                            {
                                buildingBordersNamesList.Add(pl.Layer.Replace(buildingBorderLayer, ""));
                                buildingBordersList.Add(pl);
                            }
                            else if (pl.Layer.Contains(stageBorderLayer) && pl != null)
                            {
                                var stName = pl.Layer.Replace(stageBorderLayer, "");
                                output.Add(new BuildingBorderModel(stName, pl.Area));
                            }
                            else if (pl.Layer == siteBorderLayer && pl != null)
                            {
                                output.Add(new BuildingBorderModel(siteName, pl.Area));
                            }
                        }
                        if (objectId.ObjectClass == rxClassText)
                        {
                            var tx = tr.GetObject(objectId, OpenMode.ForRead) as DBText;
                            if (tx.Layer == plotNumbersLayer)
                            {
                                buildingBorderTextobjects.Add(tx);
                            }
                        }
                    }
                    if (buildingBordersList.Count == buildingBorderTextobjects.Count)
                    {
                        for (int i = 0; i < buildingBordersList.Count; i++)
                        {
                            int countCheck = output.Count;
                            foreach (var item in buildingBorderTextobjects)
                            {
                                // Checking if text position is inside polyline
                                if (CheckIfObjectIsInsidePolyline(buildingBordersList[i], item) != PointContainment.Outside)
                                {
                                    // Creating EntityBorderM<odel
                                    output.Add(new BuildingBorderModel(buildingBordersNamesList[i], buildingBordersList[i].Area, item.TextString ));
                                }
                            }
                            if (countCheck == output.Count)
                            {
                                ed.WriteMessage("К 1+ из границ не был найден номер, проверьте что у каждой границы есть номер внутри\n");
                            }
                        }
                    }
                    else
                    {
                        ed.WriteMessage("Кол-во участков и подписей не совпадает\n");
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
        public PointContainment CheckIfObjectIsInsidePolyline(Polyline pl, Object obj)
        {
            Point3d pt = new Point3d(0,0,0);
            Curve cur = (Curve)pl;
            if (obj is DBText tx)
            {
                pt = tx.Position;
            }
            if (obj is BlockReference br)
            {
                pt = br.Position;
            }
            using(Region region = RegionFromClosedCurve(pl))
            {
                PointContainment containment = GetPointContainment(region, pt);
                return containment;
            }
        }
        public Region RegionFromClosedCurve(Curve curve)
        {
            if (!curve.Closed)
                throw new ArgumentException("Curve must be closed.");
            DBObjectCollection curves = new DBObjectCollection();
            curves.Add(curve);
            using (DBObjectCollection regions = Region.CreateFromCurves(curves))
            {
                if (regions == null || regions.Count == 0)
                    throw new InvalidOperationException("Failed to create regions");
                if (regions.Count > 1)
                    throw new InvalidOperationException("Multiple regions created");
                return regions.Cast<Region>().First();
            }
        }
        public PointContainment GetPointContainment(Curve curve, Point3d point)
        {
            if (!curve.Closed)
                throw new ArgumentException("Curve must be closed.");
            Region region = RegionFromClosedCurve(curve);
            if (region == null)
                throw new InvalidOperationException("Failed to create region");
            using (region)
            {
                return GetPointContainment(region, point);
            }
        }
        public static PointContainment GetPointContainment(Region region, Point3d point)
        {
            PointContainment result = PointContainment.Outside;
            using (Brep brep = new Brep(region))
            {
                if (brep != null)
                {
                    using (BrepEntity ent = brep.GetPointContainment(point, out result))
                    {
                        if (ent is AcBr.Face)
                            result = PointContainment.Inside;
                    }
                }
            }
            return result;
        }
        public void CreateTableWithData(SiteModel site = null, List<BaseBigAreaModel> stages = null, List<List<IBaseBuilding>> buildingsByStage = null, BaseBigAreaModel stage = null, List<IBaseBuilding> buildings = null)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                using (DocumentLock acLckDoc = doc.LockDocument())
                {
                    var blockTable = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;
                    var blocktableRecord = tr.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false) as BlockTableRecord;
                    //Creating table
                    Table tb = new Table
                    {
                        TableStyle = db.Tablestyle,
                        Position = GetInsertionPoint()
                    };
                    tb.InsertRows(1, 30, 1);
                    tb.InsertColumns(1, 25, 1);
                    tb.SetColumnWidth(25);
                    tb.InsertColumns(3, column_width, 22);
                    //First line
                    if (site != null)
                    {
                        tb.Cells[0, 0].TextString = "ТЭП площадки " + site.Name; // TODO: FIX FOR DIFFERENT OPTIONS
                    }
                    else if (stages != null)
                    {
                        tb.Cells[0, 0].TextString = "ТЭП площадки " + stages[0].Name;
                    }
                    else 
                    {
                        tb.Cells[0, 0].TextString = "ТЭП площадки " + stage.Name; 
                    }
                    if (stage != null && stages == null)
                    {
                        stages = new List<BaseBigAreaModel>();
                        stages.Add(stage);
                    }
                    //for tracking
                    int current_row = 1;
                    //filling top row
                    for (var i = 0;i < topOfTheTableArray.Length;i++)
                    {
                        tb.Cells[current_row, i].TextString = topOfTheTableArray[i];
                    }
                    current_row++;
                    //Site
                    if (site != null)
                    {
                        AddAllDataForSingleObject(ref tb, ref current_row, 2, site);
                    }
                    // Stages
                    if (stages != null)
                    {
                        foreach (var item in stages)
                        {
                            AddAllDataForSingleObject(ref tb, ref current_row, 2, item);
                            if (buildingsByStage != null)
                            {
                                int bId = 0;
                                for (int i = 0; i < buildingsByStage.Count; i++)
                                {
                                    if (buildingsByStage[i][0].StageName == item.Name)
                                    {
                                        bId = i;
                                    }
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
            //for tracking2
            int current_column = starting_column;
            //Adding reneral parameters
            AddObjectDataToRowFromParameterArray(ref tb, current_row + 1, ref current_column, generalParamArray, obj, 0, true);
            //Adding area parameters
            AddObjectDataToRowFromParameterArray(ref tb, current_row, ref current_column, AmenitiesParamArrayReq, obj);
            AddObjectDataToRowFromParameterArray(ref tb, current_row + 1, ref current_column, AmenitiesParamArrayEx, obj, 0, true);
            //Adding parking parameters
            AddObjectDataToRowFromParameterArray(ref tb, current_row, ref current_column, parkingParamArrayReq, obj);
            AddObjectDataToRowFromParameterArray(ref tb, current_row + 1, ref current_column, parkingParamArrayEx, obj, 0, true);
            //Adding social parameters
            AddObjectDataToRowFromParameterArray(ref tb, current_row, ref current_column, socialReqParamArray, obj);
            AddObjectDataToRowFromParameterArray(ref tb, current_row + 1, ref current_column, socialExParamArray, obj, 0, true);
            current_row += 2;
        }
        //Function adds data from object, parameters are in array
        public void AddObjectDataToRowFromParameterArray<T>(ref Table tb, int current_row, ref int current_column, string[] array, T obj, int startPositionInArray = 0, bool ChangeCurrentColumn = false)
        {
            if (obj != null)
            {
                for (int i = startPositionInArray; i < array.Length; i++)
                {
                    if (GetObjectPropertyByName(obj, array[i]) != null && GetObjectPropertyByName(obj, array[i]).ToString() != "0")
                    {
                        tb.Cells[current_row, current_column + i - startPositionInArray].TextString = GetObjectPropertyByName(obj, array[i]).ToString();
                    } else
                    {
                        tb.Cells[current_row, current_column + i - startPositionInArray].TextString = " ";
                    }
                }
            }
            if (ChangeCurrentColumn)
            {
                current_column += array.Length - startPositionInArray;
            }
        }
        //Create Amenities required text
        public void CreateAmenitiesReqText(AmenitiesModel AmenitiesReq, Point3d midPoint)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                using (DocumentLock acLckDoc = doc.LockDocument())
                {
                    var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;
                    var btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false) as BlockTableRecord;
                    // Create a multiline text object
                    MText acMText = new MText
                    {
                        Color = Color.FromColorIndex(ColorMethod.ByLayer, 256),
                        Layer = amenitiesLayerName,
                        TextHeight = 0.5,
                        BackgroundFill = true,
                        Attachment = AttachmentPoint.MiddleCenter,
                        Location = midPoint,
                        Width = 8,
                        Contents = $"Для позиции {AmenitiesReq.Name} требуется:\n Детских площадок: {AmenitiesReq.ChildrenArea}\n Спортивных площадок: {AmenitiesReq.SportArea}\nПлощадок откдыха: {AmenitiesReq.RestArea}\nХозяйственных площадок: {AmenitiesReq.UtilityArea}\nМусорных площадок: {AmenitiesReq.TrashArea}\nПлощадок выгула собак: {AmenitiesReq.DogsArea}\nОбщая площадь площадок: {AmenitiesReq.TotalArea}\nОзеленение: {AmenitiesReq.GreeneryArea}\n"
                    };
                    btr.AppendEntity(acMText);
                    tr.AddNewlyCreatedDBObject(acMText, true);
                }
                tr.Commit();
            }
        }
        //Create Parking required text
        public void CreateParkingReqText(ParkingModel parkingReq, Point3d midPoint)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                using (DocumentLock acLckDoc = doc.LockDocument())
                {
                    var bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;
                    var btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false) as BlockTableRecord;
                    // Create a multiline text object
                    var midPointRight = new Point3d(midPoint.X + 8, midPoint.Y, midPoint.Z);
                    MText acMText = new MText
                    {
                        Color = Color.FromColorIndex(ColorMethod.ByLayer, 256),
                        Layer = amenitiesLayerName,
                        TextHeight = 0.5,
                        BackgroundFill = true,
                        Attachment = AttachmentPoint.MiddleCenter,
                        Location = midPointRight,
                        Width = 8,
                        Contents = $"Для позиции {parkingReq.Name} требуется:\n Постоянных м/мест: {parkingReq.TotalLongParking}\n Гостевых м/мест: {parkingReq.TotalGuestParking}\n Временных м/мест: {parkingReq.TotalShortParking},\nв том числе:\nм/мест для ММГН: {parkingReq.TotalDisabledParking },\nиз которых расширенных: {parkingReq.TotalDisabledBigParking}\n"
                    };
                    btr.AppendEntity(acMText);
                    tr.AddNewlyCreatedDBObject(acMText, true);
                }
                tr.Commit();
            }
        }
        public List<string> GetListOfLayersContaining(string partOfLayerName)
        {
            var list = new List<string>();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                using (DocumentLock acLckDoc = doc.LockDocument())
                {
                    LayerTable lt = tr.GetObject(db.LayerTableId, OpenMode.ForRead) as LayerTable;
                    foreach (ObjectId item in lt)
                    {
                        LayerTableRecord layer = tr.GetObject(item, OpenMode.ForRead) as LayerTableRecord;
                        if (layer.Name.Contains(partOfLayerName) )
                        {
                            list.Add(layer.Name);
                        }
                    }
                }
            }
            return list;
        }
        //Get center of a block
        public Point3d GetCenterOfABlock(BlockReference bl)
        {
            try
            {
                Extents3d ext = bl.GeometricExtents;
                Point3d center = ext.MinPoint + (ext.MaxPoint - ext.MinPoint) / 2.0;
                return center;
            }
            catch
            {
                return bl.Position;
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
        public Object GetObjectPropertyByName(Object obj, String name)
        {
            foreach (String part in name.Split('.'))
            {
                if (obj == null) { return null; }
                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }
                obj = info.GetValue(obj, null);
            }
            return obj;
        }
        public T GetObjectPropertyByName<T>(Object obj, String name)
        {
            Object retval = GetObjectPropertyByName(obj, name);
            if (retval == null) { return default(T); }
            // throws InvalidCastException if types are incompatible
            return (T)retval;
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
        public int DivideStringsGetRooundedInt(string value1, string value2)
        {
            double val1 = Convert.ToDouble(value1);
            double val2 = Convert.ToDouble(value2);
            return Convert.ToInt32(Math.Round(val1 / val2));
        }
        public object GetOnePropetyFromListOfObjectsBySecondPropertyValue<T>(List<T> list, string prop1, string prop2, string value1)
        {
            var item = SearchByPropNameAndValue(list, prop2, value1);
            return item.GetType().GetProperty(prop1).GetValue(item, null);
        }
        public List<T> SearchListOfListsByPropNameAndValue<T>(List<List<T>> list, string propName, string propValue)
        {
            foreach (List<T> item in list)
            {
                if (item.FirstOrDefault(x => x.GetType().GetProperty(propName).GetValue(x, null).ToString() == propValue) != null)
                {
                    return item;
                }
            }
            return null;
        }
        public T SearchByPropNameAndValue<T>(List<T> list, string propName, string propValue)
        {
            return list.FirstOrDefault(x => x.GetType().GetProperty(propName).GetValue(x, null).ToString() == propValue);
        }
        public string GetMaxMinFromListOfStrings(List<string> list, char separater)
        {
            int max = 0;
            int min = 100;
            foreach (var item in list)
            {
                var arr = item.Split(separater);
                foreach (var et in arr)
                {
                    var val = Convert.ToInt32(et);
                    if (max < val)
                    {
                        max = val;
                    }
                    if (min > val)
                    {
                        min = val;
                    }
                }
            }
            if (min == max)
            {
                return min.ToString();
            }
            return min + "-" + max;
        }
        public void DeserealiseJson<T>(ref List<T> list, string filename)
        {
            list = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(Directory.GetCurrentDirectory()+filename));
        }
        public void SerealiseJson<T>(ref List<T> list, string filename)
        {
            using (StreamWriter file = File.CreateText(Directory.GetCurrentDirectory()+filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, list);
            }
        }
        public List<string> SortListOfStringByNumbersInIt(List<string> list)
        {
            return list.OrderBy(s => int.Parse(Regex.Match(s, @"\d+").Value)).ToList();
        }
}
}
