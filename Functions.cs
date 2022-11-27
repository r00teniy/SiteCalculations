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
using static System.Collections.Specialized.BitVector32;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Xml.Linq;

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
        string buildingBorderLayer = "13_Граница_";
        string stageBorderLayer = "12_Граница_Этапа_";
        string siteBorderLayer = "11_Граница_площадки";
        string buildingAreaExLayer = "22_Площадки_проект";
        //Arrays with data for table
        string[] siteParamArray = { "CityName", "Name", "PlotArea", "TotalConstructionArea", "BuildingPartPercent",
                "TotalApartmentArea", "TotalNumberOfAppartments", "TotalResidents", "TotalCommerceArea", "TotalAreaReq", "TotalChildAreaReq",
                "TotalSportAreaReq", "TotalRestAreaReq", "TotalUtilityAreaReq", "TotalTrashAreaReq", "TotalDogsAreaReq", "TotalGreeneryAreaReq",
                "TotalAreaEx", "TotalChildAreaEx", "TotalSportAreaEx", "TotalRelaxAreaEx", "TotalDogsAreaEx", "TotalTrashAreaEx",
                "TotalUtilityAreaEx", "TotalGreeneryAreaEx", "TotalLongParkingReq", "TotalShortParkingReq", "TotalGuestParkingReq" };
        string[] siteTableArray = { "Город", "Площадка", "Площадь площадки, м2", "Площадь зайтройки, м2", "Процент застройки",
                "Площадь квартир", "Кол-во квартир", "Кол-во жителей", "Площадь нежилья", "Общая требуемая S, в т.ч:", "- детских площадок",
                "- спортивных площадок", "- площадок отдыха", "- хозяйственных площадок", "- площадок для мусороконтейнеров", "- площадок выгула собак",
                "Требуемая площадь озеленения", "Общая проектная S, в т.ч:", "- детских площадок",
                "- спортивных площадок", "- площадок отдыха", "- площадок выгула собак", "- площадок для мусороконтейнеров", "- хозяйственных площадок",
                "Проектная площадь озеленения", "Кол-во парковок для постоянного храниния а/м", "Кол-во парковок для временного хранения а/м", "Кол-во гостевых парковок" };
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
        public List<IBaseBuilding> GetBuildings(CityModel city, List<EntityBorderModel> borders, List<ExParametersModel> exParams, int[] parking)
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
                                output.Add(new ApartmentBuildingModel(city, dynBlockPropValues, borders.FirstOrDefault(c => c.Name == dynBlockPropValues[1]).Area, exParams.FirstOrDefault(c => c.BuildingName == dynBlockPropValues[1]), parking));
                            }
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
    }
}
