using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.Geometry;
using System.Reflection;
using SiteCalculations.Models;
using System.Collections;
using System;
using System.Globalization;
using Autodesk.AutoCAD.Internal.Windows;
using Autodesk.AutoCAD.Customization;
using Autodesk.AutoCAD.Windows.InfoCenterAcconn;
using System.Net;
using Autodesk.AutoCAD.Colors;

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
        string AmenitiesLayerName = "30_Площадки_требуемые";
        //Parameters_of_dyn_blocks
        string[] parkingBlockPararmArray = { "Размер", "Тип", "Обычн_МГН", "РасширенноеММ" };
        //Arrays with data for table
        string[] generalParamArray = { "PlotArea", "TotalConstructionArea", "BuildingPartPercent",
            "TotalApartmentArea", "TotalNumberOfApartments", "TotalResidents", "TotalCommerceArea" };// add Number of floors
        string[] AmenitiesParamArray = { "TotalArea", "ChildrenArea", "SportArea", "RestArea", 
            "UtilityArea", "TrashArea", "DogsArea", "GreeneryArea" };
        string[] socialReqParamArray = { "SchoolsReq", "KindergartensReq", "HospitalsReq" };// remake as a class? worth it?
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
        public List<ParkingBlockModel> GetExParkingBlocks()
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

                                if (attRef.Tag == "Этап" && attRef2.Tag == "КОЛ-ВО")
                                {
                                    string[] dynBlockPropValues = new string[6];
                                    var pc = br.DynamicBlockReferencePropertyCollection;
                                    for (var j = 0; j < pc.Count; j++)
                                    {
                                        for (int i = 0; i < parkingBlockPararmArray.Length; i++)
                                        {
                                            if (pc[j].PropertyName == parkingBlockPararmArray[i])
                                            {
                                                dynBlockPropValues[i] = pc[j].Value.ToString();
                                            }
                                        }
                                    }
                                    dynBlockPropValues[4] = attRef.TextString;
                                    dynBlockPropValues[5] = attRef2.TextString;
                                    output.Add(new ParkingBlockModel(dynBlockPropValues));
                                }
                            }
                            catch { ed.WriteMessage("ded  "); }
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
                                string[] dynBlockPropValues = new string[8];
                                DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                for (int i = 0; i < 8; i++) // we only need first 8 properties
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
        public List<IBaseBuilding> GetBuildings(CityModel city, List<EntityBorderModel> borders = null, List<AmenitiesModel> exParams = null, List<ParkingModel> exParking = null, bool onlyGetReqParams = false)
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
                                if (br.Layer == ApartmentsBuildingsLayer && br != null)
                                {
                                    ed.WriteMessage("block \n");
                                    string[] dynBlockPropValues = new string[7];
                                    DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                    for (int i = 0; i < 7; i++)
                                    {
                                        dynBlockPropValues[i] = pc[i].Value.ToString();
                                    }
                                    output.Add(new ApartmentBuildingModel(city, dynBlockPropValues, midPoint));
                                }
                            }
                            else
                            {
                                //Apartment buildings
                                if (br.Layer == ApartmentsBuildingsLayer && br != null)
                                {
                                    string[] dynBlockPropValues = new string[7];
                                    DynamicBlockReferencePropertyCollection pc = br.DynamicBlockReferencePropertyCollection;
                                    for (int i = 0; i < 7; i++)
                                    {
                                        dynBlockPropValues[i] = pc[i].Value.ToString();
                                    }
                                    output.Add(new ApartmentBuildingModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]).Area, exParams.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), exParking.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), midPoint));
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
                                    output.Add(new SchoolModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]).Area, exParking.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), midPoint));
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
                                    output.Add(new KindergartenModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]).Area, exParking.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), midPoint));
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
                                    output.Add(new HospitalModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]).Area, exParking.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), midPoint));
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
                                    output.Add(new ParkingBuildingModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]).Area, exParking.FirstOrDefault(c => c.Name == dynBlockPropValues[1]), midPoint));
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
            ParkingModel parkingReq = GetObjectPropertyByName(obj, "TotalParkingReq") as ParkingModel;
            ParkingModel parkingEx = GetObjectPropertyByName(obj, "TotalParkingEx") as ParkingModel;
            AmenitiesModel amenitiesReq = GetObjectPropertyByName(obj, "AmenitiesReq") as AmenitiesModel;
            AmenitiesModel amenitiesEx = GetObjectPropertyByName(obj, "AmenitiesEx") as AmenitiesModel;
            //for tracking2
            int current_column = starting_column;
            //Adding reneral parameters
            AddObjectDataToRowFromParameterArray(ref tb, current_row + 1, ref current_column, generalParamArray, obj, 0, true);
            //Adding area parameters
            AddObjectDataToRowFromParameterArray(ref tb, current_row, ref current_column, AmenitiesParamArray, amenitiesReq);
            AddObjectDataToRowFromParameterArray(ref tb, current_row + 1, ref current_column, AmenitiesParamArray, amenitiesEx, 0, true);
            //Adding parking parameters
            AddObjectDataToRowFromParameterArray(ref tb, current_row, ref current_column, parkingParamArray, parkingReq);
            AddObjectDataToRowFromParameterArray(ref tb, current_row + 1, ref current_column, parkingParamArray, parkingEx, 0, true);
            //Adding social parameters
            AddObjectDataToRowFromParameterArray(ref tb, current_row, ref current_column, socialReqParamArray, obj);
            AddObjectDataToRowFromParameterArray(ref tb, current_row + 1, ref current_column, socialExParamArray, obj, 0, true);
            current_row += 2;
        }
        //Function adds data from object, parameters are in array
        public void AddObjectDataToRowFromParameterArray<T>(ref Table tb, int current_row, ref int current_column, string[] array, T obj, int startPositionInArray = 0, bool ChangeCurrentColumn = false)
        {
            for (int i = startPositionInArray; i < array.Length; i++)
            {
                if (obj.GetType().GetProperty(array[i]) != null && obj.GetType().GetProperty(array[i]).GetValue(obj, null).ToString() != "0")
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
        //Create Amenities block
        public void CreateAmenitiesBlock(AmenitiesModel AmenitiesReq, Point3d midPoint)
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
                        Layer = AmenitiesLayerName,
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
        public int DivideStringsGetRooundedInt(string value1, string value2)
        {
            double val1 = Convert.ToDouble(value1);
            double val2 = Convert.ToDouble(value2);
            return Convert.ToInt32(Math.Round(val1 / val2));
        }
    }
}
