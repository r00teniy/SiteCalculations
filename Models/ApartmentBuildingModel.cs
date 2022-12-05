﻿using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;

namespace SiteCalculations.Models
{
    internal class ApartmentBuildingModel : IBaseBuilding
    {
        // Buildimg part
        public string StageName { get; private set; }
        public string Name { get; private set; }
        public double TotalConstructionArea { get; private set; }
        public double PlotArea { get; private set; }
        public string PlotNumber { get; private set; }
        public double BuildingPartPercent { get; private set; }
        public string NumberOfFloors { get; private set; }
        // Apartment part
        public int TotalResidents { get; private set; }
        public int TotalNumberOfApartments { get; private set; }
        public double TotalApartmentArea { get; private set; }
        // commerce part
        public double TotalCommerceArea { get; private set; }
        //Amenities
        public AmenitiesModel AmenitiesReq { get; private set; }
        public AmenitiesModel AmenitiesEx { get; private set; }

        // Parking
        public ParkingModel TotalParkingReq { get; private set; }
        public ParkingModel TotalParkingEx { get; private set; }
        public Point3d MidPoint { get; private set; }

        //Combining building from sections
        public ApartmentBuildingModel(CityModel city, List<ApartmentBuildingSectionModel> sectionList, BuildingBorderModel plot, AmenitiesModel exParam, ParkingModel exParking)
        {
            StageName = sectionList[0].StageName;
            Name = sectionList[0].Name;
            MidPoint = sectionList[0].MidPoint;
            foreach (var sec in sectionList)
            {
                TotalConstructionArea += sec.ConstructionArea;
                TotalNumberOfApartments += sec.NumberOfApartments;
                TotalApartmentArea += sec.ApartmentsArea;
                TotalCommerceArea += sec.CommerceArea;
                if (sec != sectionList[sectionList.Count - 1])
                {
                    NumberOfFloors += sec.NumberOfFloors.ToString() + ",";
                }
                else
                {
                    NumberOfFloors += sec.NumberOfFloors.ToString();
                }
            }
            PlotArea = Math.Round(plot.Area, 2);
            PlotNumber = plot.PlotNumber;
            TotalResidents = Convert.ToInt32(Math.Floor(TotalApartmentArea / city.SqMPerPerson));
            BuildingPartPercent = Math.Round(100 * TotalConstructionArea / PlotArea, 2);
            //Amenities
            AmenitiesReq = city.AreaReq.CalculateReqArea(Name,TotalResidents, TotalNumberOfApartments, TotalApartmentArea);
            AmenitiesEx = exParam;
            // Parking requires
            TotalParkingReq = city.Parking.CalculateParking(Name, new double[]{ TotalResidents, TotalApartmentArea, TotalNumberOfApartments, TotalCommerceArea, 0, 0, 0 });
            TotalParkingEx = exParking;
        }
        public ApartmentBuildingModel(CityModel city, string[] buildingParams, BuildingBorderModel plot, AmenitiesModel exParam, ParkingModel exParking, Point3d midPoint)
        {
            StageName = buildingParams[0];
            Name = buildingParams[1];
            MidPoint= midPoint;
            TotalConstructionArea = Convert.ToDouble(buildingParams[2]);
            NumberOfFloors = buildingParams[3];
            TotalNumberOfApartments = Convert.ToInt32(buildingParams[4]);
            TotalApartmentArea = Convert.ToDouble(buildingParams[5]);
            TotalCommerceArea = Convert.ToDouble(buildingParams[6]);
            PlotArea = Math.Round(plot.Area, 2);
            PlotNumber = plot.PlotNumber;
            TotalResidents = Convert.ToInt32(Math.Floor(TotalApartmentArea / city.SqMPerPerson));
            BuildingPartPercent = Math.Round(100 * TotalConstructionArea / PlotArea, 2);
            //Amenities
            AmenitiesReq = city.AreaReq.CalculateReqArea(Name, TotalResidents, TotalNumberOfApartments, TotalApartmentArea);
            AmenitiesEx = exParam;
            //Parking
            TotalParkingReq = city.Parking.CalculateParking(Name, new double[] { TotalResidents, TotalNumberOfApartments, TotalApartmentArea, TotalCommerceArea, 0, 0, 0 });
            TotalParkingEx = exParking;
        }
        //These 2 are for getting amenities only
        public ApartmentBuildingModel(CityModel city, string[] buildingParams, Point3d midPoint)
        {
            StageName = buildingParams[0];
            Name = buildingParams[1];
            MidPoint = midPoint;
            TotalConstructionArea = Convert.ToDouble(buildingParams[2]);
            NumberOfFloors = buildingParams[3];
            TotalNumberOfApartments = Convert.ToInt32(buildingParams[4]);
            TotalApartmentArea = Convert.ToDouble(buildingParams[5]);
            TotalCommerceArea = Convert.ToDouble(buildingParams[6]);
            TotalResidents = Convert.ToInt32(Math.Floor(TotalApartmentArea / city.SqMPerPerson));
            //Amenities
            AmenitiesReq = city.AreaReq.CalculateReqArea(Name, TotalResidents, TotalNumberOfApartments, TotalApartmentArea);
            //Parking
            TotalParkingReq = city.Parking.CalculateParking(Name, new double[] { TotalResidents, TotalNumberOfApartments, TotalApartmentArea, TotalCommerceArea, 0, 0, 0 });
        }
        public ApartmentBuildingModel(CityModel city, List<ApartmentBuildingSectionModel> sectionList)
        {
            StageName = sectionList[0].StageName;
            Name = sectionList[0].Name;
            MidPoint = sectionList[0].MidPoint;
            foreach (var sec in sectionList)
            {
                TotalConstructionArea += sec.ConstructionArea;
                TotalNumberOfApartments += sec.NumberOfApartments;
                TotalApartmentArea += sec.ApartmentsArea;
                TotalCommerceArea += sec.CommerceArea;              
            }
            TotalResidents = Convert.ToInt32(Math.Floor(TotalApartmentArea / city.SqMPerPerson));
            //Amenities
            AmenitiesReq = city.AreaReq.CalculateReqArea(Name, TotalResidents, TotalNumberOfApartments, TotalApartmentArea);
            // Parking requires
            TotalParkingReq = city.Parking.CalculateParking(Name, new double[] { TotalResidents, TotalNumberOfApartments, TotalApartmentArea, TotalCommerceArea, 0, 0, 0 });
        }
    }
}
