using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.Geometry;
using System.Reflection;
using SiteCalculations.Models;
using System.Diagnostics;


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
        string stageBorderLayer = "12_Граница_Этапа_";
        string siteBorderLayer = "11_Граница_площадки";
        string buildingAreaExLayer = "31_Площадки_проект";
        //Arrays with data for table
        string[] generalParamArray = { "Name", "PlotArea", "TotalConstructionArea", "BuildingPartPercent",
            "TotalApartmentArea", "TotalNumberOfApartments", "TotalResidents", "TotalCommerceArea" };
        string[] livingReqParamArray = { "TotalAreaReq", "TotalChildAreaReq", "TotalSportAreaReq", "TotalRestAreaReq", 
            "TotalUtilityAreaReq", "TotalTrashAreaReq", "TotalDogsAreaReq", "TotalGreeneryAreaReq" };
        string[] livingExParamArray = { "TotalAreaEx", "TotalChildAreaEx", "TotalSportAreaEx", "TotalRestAreaEx",
            "TotalDogsAreaEx", "TotalTrashAreaEx", "TotalUtilityAreaEx", "TotalGreeneryAreaEx" };
        string[] socialReqParamArray = { "SchoolsReq", "KindergartensReq", "HospitalsReq" };
        string[] socialExParamArray = { "SchoolsEx", "KindergartensEx", "HospitalsEx" };
        string[] parkingParamArray = { "TotalLongParking", "TotalShortParking", "TotalGuestParking" };
        string[] TopOfTheTableArray = { "№", "Имя", "Площадь участка, м2", "Площадь зайтройки, м2", "Процент застройки, %",
                "Площадь квартир", "Кол-во квартир", "Кол-во жителей", "Площадь нежилья", "Общая S площадок, в т.ч:", "- детских площадок",
                "- спортивных площадок", "- площадок отдыха", "- хозяйственных площадок", "- площадок для мусороконтейнеров", "- площадок выгула собак",
                "Площадь озеленения", "Кол-во парковок для постоянного храниния а/м", "Кол-во парковок для временного хранения а/м", "Кол-во гостевых парковок",
                "Кол-во мест в Школах", "Кол-во мест в садиках", "Кол-во посетителей в день в больницах" };
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
            }
            return output;
        }
        public List<EntityBorderModel> GetBorders()
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
                                output.Add(new EntityBorderModel("Площадка", pl.Area));
                            }
                        }
                    }
                }
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
            /*if (pPtRes.Status == PromptStatus.Cancel)
            {
                return;
            }*/
            return pt;
        }
        public void CreateSiteTable(SiteModel site)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                using (DocumentLock acLckDoc = doc.LockDocument())
                {
                    var blockTable = trans.GetObject(db.BlockTableId, OpenMode.ForRead, false) as BlockTable;
                    var blocktableRecord = trans.GetObject(blockTable[BlockTableRecord.ModelSpace], OpenMode.ForWrite, false) as BlockTableRecord;
                    Table tb = new Table();
                    tb.TableStyle = db.Tablestyle;
                    tb.Position = GetInsertionPoint();
                    // row height
                    double row_height = 8;
                    // column width
                    double column_width = 20;
                    tb.InsertRows(1, 20, 1);
                    tb.InsertRows(2, row_height, 2);
                    tb.InsertColumns(0, column_width, 22);
                    //First line
                    tb.Cells[0, 0].TextString = "ТЭП площадки " + site.Name + " в городе " + site.City.CityName;
                    tb.Cells[2, 0].TextString = "Требуется:";
                    tb.Cells[3, 0].TextString = "Запроектировано:";
                    //for tracking
                    int current_row = 1;
                    for (var i = 0;i < TopOfTheTableArray.Length;i++)
                    {
                        tb.Cells[current_row, i].TextString = TopOfTheTableArray[i];
                    }
                    //for tracking2
                    current_row++;
                    int current_collumn = 1;
                    for (int i = 0; i < generalParamArray.Length; i++)
                    {
                        tb.Cells[current_row+1, current_collumn].TextString = site.GetType().GetProperty(generalParamArray[i]).GetValue(site, null).ToString();
                        current_collumn++;
                    }
                    for (int i = 0; i < livingReqParamArray.Length; i++)
                    {
                        tb.Cells[current_row, current_collumn].TextString = site.GetType().GetProperty(livingReqParamArray[i]).GetValue(site, null).ToString();
                        tb.Cells[current_row+1, current_collumn].TextString = site.GetType().GetProperty(livingExParamArray[i]).GetValue(site, null).ToString();
                        current_collumn++;
                    }
                    for (int i = 0; i < parkingParamArray.Length; i++)
                    {
                        tb.Cells[current_row, current_collumn].TextString = site.TotalParkingReq.GetType().GetProperty(parkingParamArray[i]).GetValue(site.TotalParkingReq, null).ToString();
                        tb.Cells[current_row + 1, current_collumn].TextString = site.TotalParkingEx.GetType().GetProperty(parkingParamArray[i]).GetValue(site.TotalParkingEx, null).ToString();
                        current_collumn++;
                    }
                    for (int i = 0; i < socialReqParamArray.Length; i++)
                    {
                        tb.Cells[current_row, current_collumn].TextString = site.GetType().GetProperty(socialReqParamArray[i]).GetValue(site, null).ToString();
                        tb.Cells[current_row + 1, current_collumn].TextString = site.GetType().GetProperty(socialExParamArray[i]).GetValue(site, null).ToString();
                        current_collumn++;
                    }
                    current_row++;
                    
                    tb.GenerateLayout();
                    blocktableRecord.AppendEntity(tb);
                    trans.AddNewlyCreatedDBObject(tb, true);
                }
                trans.Commit();
            }
        }
    }
}
