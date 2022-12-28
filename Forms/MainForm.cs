using Autodesk.AutoCAD.DatabaseServices;
using SiteCalculations.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

namespace SiteCalculations.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private void bCreateTable_Click(object sender, EventArgs e)
        {
            if (cbCity.SelectedItem != null && bName.Text != "")
            {
                var f = new Functions();
                CityModel city = cbCity.SelectedItem as CityModel;
                List<PlotBorderModel> plotBorders = f.GetPlotBorders();
                List<List<ZoneBorderModel>> zoneBorders = f.GetZoneBorders(bName.Text);
                //Adding plots to building zones and special zones
                f.AddPlotNumbersToZones(ref zoneBorders, plotBorders);
                List<AmenitiesModel> exAmenities = f.GetExAmenities();
                List<ParkingBlockModel> parkingBlocks = f.GetExParkingBlocks(zoneBorders[0]); ;
                //Creating parking models
                List<ParkingModel> exParking = f.CreateExParking(parkingBlocks, plotBorders);
                //getting separate building
                List<IBaseBuilding> allBuildings = f.GetBuildings(city, zoneBorders[0], exAmenities, exParking);
                //Getting sections
                List<ApartmentBuildingSectionModel> allSections = f.GetSecttions();
                //Sorting sections by building name
                List<List<ApartmentBuildingSectionModel>> sectionsByBuilding = f.SortListOfObjectsByParameterToSeparateLists(allSections, "Name");
                //Creating Buildings from sections and adding them to main pool
                for (int i = 0; i < sectionsByBuilding.Count; i++)
                {
                    allBuildings.Add(new ApartmentBuildingModel(city, sectionsByBuilding[i], f.SearchByPropNameAndValue(zoneBorders[0], "Name", sectionsByBuilding[i][0].Name), f.SearchByPropNameAndValue(exAmenities, "Name", sectionsByBuilding[i][0].Name), f.SearchByPropNameAndValue(exParking, "Name", sectionsByBuilding[i][0].Name)));
                }
                //Sorting buildings by name
                List<IBaseBuilding> sortedBuildings = f.Sort_List_By_PropertyName_Generic("Ascending", "Name", allBuildings);
                //Sorting buildings by stage
                List<List<IBaseBuilding>> buildingsByStage = f.SortListOfObjectsByParameterToSeparateLists(sortedBuildings, "StageName");
                //Creating Stages
                List<BaseBigAreaModel> stages = new List<BaseBigAreaModel>(); ;
                for (int i = 0; i < buildingsByStage.Count; i++)
                {
                    stages.Add(new StageModel(city, f.SearchByPropNameAndValue(zoneBorders[1], "Name", buildingsByStage[i][0].StageName).Area, buildingsByStage[i]));
                }
                //Sorting stages
                List<BaseBigAreaModel> sortedStages = f.Sort_List_By_PropertyName_Generic("Ascending", "Name", stages);
                //Creating Site
                SiteModel site = new SiteModel(city, bName.Text, f.SearchByPropNameAndValue(zoneBorders[2], "Name", bName.Text).Area, sortedStages);
                // Generating tables
                this.Hide();
                if (rbSite.Checked)
                {
                    if (cbStages_site.Checked)
                    {
                        if (cb_buildings.Checked)
                        {
                            f.CreateTableWithData(site, sortedStages, buildingsByStage);
                        }
                        else
                        {
                            f.CreateTableWithData(site, sortedStages);
                        }
                    }
                    else
                    {
                        f.CreateTableWithData(site);
                    }
                }
                else
                {
                    if (rbStage.Checked)
                    {
                        if (rbStages_AllStages.Checked)
                        {
                            if (cb_buildings.Checked)
                            {
                                f.CreateTableWithData(null, sortedStages, buildingsByStage);
                            }
                            else
                            {
                                f.CreateTableWithData(null, sortedStages);
                            }
                        }
                        else
                        {
                            if (cbStages_stages.SelectedItem != null)
                            {
                                if (cb_buildings.Checked)
                                {
                                    f.CreateTableWithData(null, null, null, f.SearchByPropNameAndValue(sortedStages, "Name", cbStages_stages.SelectedItem.ToString()), f.SearchListOfListsByPropNameAndValue(buildingsByStage, "StageName", cbStages_stages.SelectedItem.ToString()));
                                }
                                else
                                {
                                    f.CreateTableWithData(null, null, null, f.SearchByPropNameAndValue(sortedStages, "Name", cbStages_stages.SelectedItem.ToString()));
                                }
                            }
                            else
                            {
                                errLabel.Text = "выберите этап";
                            }
                        }
                    }
                    else
                    {
                        List<string[]> parkTableList = new List<string[]>();
                        var buildingNames = new List<string>();
                        var buildingsPlotNumbers = new List<string>();
                        List <ParkingModel> parkReqForTable = new List<ParkingModel>();
                        var plotNumbers = f.GetSortedListOfParameterValues(plotBorders, "PlotNumber");
                        //Creating parking table
                        if (rbStages_AllStages.Checked)
                        {
                            //Getting building names that aren't parking buildings with LINQ
                            buildingNames = (from build in sortedBuildings
                                             where !(build is ParkingBuildingModel)
                                             orderby build.Name
                                             select build.Name).ToList();
                            //Getting parking requirements with LINQ
                            parkReqForTable =
                            (from build in sortedBuildings
                             where !(build is ParkingBuildingModel)
                             orderby build.Name
                             select build.TotalParkingReq).ToList();
                            //plotnumbers with LINQ
                            buildingsPlotNumbers =
                            (from build in sortedBuildings
                             where !(build is ParkingBuildingModel)
                             orderby build.Name
                             select build.PlotNumber).ToList();
                        }
                        else
                        {
                            if (cbStages_stages.SelectedItem != null)
                            {
                                //Getting building names for this stage that aren't parking buildings with LINQ
                                buildingNames = (from build in sortedBuildings
                                                 where !(build is ParkingBuildingModel) && build.StageName == cbStages_stages.SelectedItem.ToString()
                                                 orderby build.Name
                                                 select build.Name).ToList();
                                //Getting parking requirements with LINQ
                                parkReqForTable =
                                (from build in sortedBuildings
                                 where !(build is ParkingBuildingModel) && build.StageName == cbStages_stages.SelectedItem.ToString()
                                 orderby build.Name
                                 select build.TotalParkingReq).ToList();
                                //plotnumbers with LINQ
                                buildingsPlotNumbers =
                                (from build in sortedBuildings
                                 where !(build is ParkingBuildingModel) && build.StageName == cbStages_stages.SelectedItem.ToString()
                                 orderby build.Name
                                 select build.PlotNumber).ToList();
                            }
                            else
                            {
                                errLabel.Text = "выберите этап";
                            }
                        }
                        //Creating list of parking buildings
                        var parkingBuildings = sortedBuildings.Where(x => x is ParkingBuildingModel).Select(x => x as ParkingBuildingModel).ToList();
                        //Creating lines for table
                        foreach (var item in plotNumbers)
                        {
                            var test = parkingBuildings.Where(x => x.PlotNumber == item).ToList();
                            if (test.Count == 0)
                            {
                                parkTableList.Add(f.CreateLineForParkingTable(parkingBlocks, buildingNames, item));
                            }
                            else
                            {
                                parkTableList.Add(f.CreateLineForParkingTable(parkingBlocks, buildingNames, item));
                                foreach (var t in test)
                                {
                                    parkTableList.Add(f.CreateLineForParkingTable(parkingBlocks, buildingNames, item, zoneBorders[0], true, t));
                                }
                                
                            }
                        }
                        parkTableList.RemoveAll(x => x == null);
                        var exParkingOnPlot = f.GetExParkingOnBuildingSite(parkingBlocks, buildingNames, buildingsPlotNumbers);
                        f.CreateParkingTable(parkTableList, buildingNames, parkReqForTable, exParkingOnPlot, rbStages_AllStages.Checked ? bName.Text : $"{bName.Text} по этапу {cbStages_stages.SelectedItem.ToString().Split(' ')[1]}");
                    }
                }
                f.StoreSettings((cbCity.SelectedItem as CityModel).CityName, bName.Text);
            }

        }
        private void bCityCreate_Click(object sender, EventArgs e)
        {
            var cmf = new CityModelForm(this);
            if (Functions.parkingCalcTypeList != null)
            {
                cmf.cbParking.DataSource = Functions.parkingCalcTypeList;
                cmf.cbParking.DisplayMember = "Name";
                cmf.cbParking.SelectedIndex = cmf.cbParking.Items.Count - 1;
            }
            if (Functions.amenitiesCalcTypeList != null)
            {
                cmf.cbAmenities.DataSource = Functions.amenitiesCalcTypeList;
                cmf.cbAmenities.DisplayMember = "Name";
                cmf.cbAmenities.SelectedIndex = cmf.cbAmenities.Items.Count - 1;
            }
            cmf.Show();
        }
        private void bCreateReq_Click(object sender, EventArgs e)
        {
            if (cbCity.SelectedItem != null)
            {
                errLabel.Text = "";
                var f = new Functions();
                CityModel city = f.SearchByPropNameAndValue(Functions.cityCalcTypeList, "CityName", cbCity.SelectedItem.ToString());
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
                this.Hide();
                foreach (var item in allBuildings)
                {
                    if (item is ApartmentBuildingModel mod)
                    {
                        f.CreateAmenitiesReqText(mod.AmenitiesReq, mod.MidPoint);
                        f.CreateParkingReqText(mod.TotalParkingReq, mod.MidPoint);
                    }
                }
                this.Show();
            }
            else
            {
                errLabel.Text = "Выберите город";
            }
        }

        private void rbStage_CheckedChanged(object sender, EventArgs e)
        {
            //Setting stages part visible
            rbStages_AllStages.Visible = true;
            rbStages_SingleStage.Visible = true;
            if (rbStages_AllStages.Checked)
            {
                cbStages_stages.Visible = false;
            }
            else
            {
                cbStages_stages.Visible = true;
            }
            //Setting site part invisible
            cbStages_site.Visible = false;
            cb_buildings.Visible = true;
        }

        private void rbParking_CheckedChanged(object sender, EventArgs e)
        {
            rbStages_AllStages.Visible = true;
            rbStages_SingleStage.Visible = true;
            cbStages_stages.Visible = false;
            cbStages_site.Visible = false;
            cb_buildings.Visible = false;
        }

        private void rbSite_CheckedChanged(object sender, EventArgs e)
        {
            //Setting stages part invisible
            rbStages_AllStages.Visible = false;
            rbStages_SingleStage.Visible = false;
            cbStages_stages.Visible = false;
            //Setting site part invisible
            cbStages_site.Visible = true;
            cb_buildings.Visible = true;
        }

        private void rbStages_SingleStage_CheckedChanged(object sender, EventArgs e)
        {
            var f = new Functions();
            List<string> stages = f.GetListOfLayersContaining(Functions.stageBorderLayer);
            foreach (var item in stages)
            {
                cbStages_stages.Items.Add(item.Replace(Functions.stageBorderLayer, ""));
            }
            cbStages_stages.SelectedIndex = 0;
            cbStages_stages.Visible = true;
        }

        private void rbStages_AllStages_CheckedChanged(object sender, EventArgs e)
        {
            cbStages_stages.Visible = false;
        }

        private void bCityDelete_Click(object sender, EventArgs e)
        {
            var f = new Functions();
            if (cbCity.SelectedItem != null)
            {
                f.DeserealiseJson<CityModel>(ref Functions.cityCalcTypeList, "\\city.json");
                Functions.cityCalcTypeList.Remove(Functions.cityCalcTypeList.FirstOrDefault(x => x.CityName == (cbCity.SelectedItem as CityModel).CityName));
                f.SerealiseJson<CityModel>(ref Functions.cityCalcTypeList, "\\city.json");
                cbCity.DataSource = Functions.cityCalcTypeList;
            }
        }
    }
}
