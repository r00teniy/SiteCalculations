﻿using System;
using System.Collections.Generic;

namespace SiteCalculations.Models
{
    public class ParkingModel
    {
        public string Name { get; private set; }
        public int TotalLongParking { get; private set; }
        public int TotalShortParking { get; private set; }
        public int TotalGuestParking { get; private set; }
        public int ShortDisabledNormal { get; private set; }
        public int ShortDisabledBig { get; private set; }
        public int GuestDisabledNormal { get; private set; }
        public int GuestDisabledBig { get; private set; }

        public ParkingModel(string name, int[] parking)
        {
            Name = name;
            TotalLongParking = parking[0];
            TotalShortParking = parking[1];
            TotalGuestParking = parking[2];
            ShortDisabledNormal = parking[3];
            ShortDisabledBig = parking[4];
            GuestDisabledNormal = parking[5];
            GuestDisabledBig = parking[6];
        }
        public ParkingModel(string name, int parkLong, int parkShort, int parkGuest)
        {
            Name = name;
            TotalLongParking = parkLong;
            TotalShortParking = parkShort;
            TotalGuestParking = parkGuest;
            ShortDisabledNormal = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(parkShort) / 10));
            ShortDisabledBig = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(parkShort) / 20));
            GuestDisabledNormal = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(parkGuest) / 10));
            GuestDisabledBig = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(parkGuest) / 20));
        }
        public ParkingModel(string name, List<ParkingModel> parking)
        {
            Name = name;
            foreach (var par in parking)
            {
                TotalLongParking += par.TotalLongParking;
                TotalShortParking += par.TotalShortParking;
                TotalGuestParking += par.TotalGuestParking;
                ShortDisabledNormal += par.ShortDisabledNormal;
                ShortDisabledBig += par.ShortDisabledBig;
                GuestDisabledNormal += par.GuestDisabledNormal;
                GuestDisabledBig += par.GuestDisabledBig;
            }
        }
    }
}
