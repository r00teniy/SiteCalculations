using SiteCalculations.Models;
using System;
using System.Collections.Generic;
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
                CityModel city = f.SearchByPropNameAndValue(Functions.cityCalcTypeList, "CityName", cbCity.SelectedItem.ToString());
                List<BuildingBorderModel> borders = f.GetBorders(bName.Text);
                List<AmenitiesModel> exAmenities = f.GetExAmenities();
                List<ParkingBlockModel> parkingBlocks = f.GetExParkingBlocks(borders);
                //Creating parking models
                List<ParkingModel> exParking = f.CreateExParking(parkingBlocks, borders);
                //getting separate building
                List<IBaseBuilding> allBuildings = f.GetBuildings(city, borders, exAmenities, exParking);
                //Getting sections
                List<ApartmentBuildingSectionModel> allSections = f.GetSecttions();
                //Sorting sections by building name
                List<List<ApartmentBuildingSectionModel>> sectionsByBuilding = f.SortListOfObjectsByParameterToSeparateLists(allSections, "Name");
                //Creating Buildings from sections and adding them to main pool
                for (int i = 0; i < sectionsByBuilding.Count; i++)
                {
                    allBuildings.Add(new ApartmentBuildingModel(city, sectionsByBuilding[i], f.SearchByPropNameAndValue(borders, "Name", sectionsByBuilding[i][0].Name), f.SearchByPropNameAndValue(exAmenities, "Name", sectionsByBuilding[i][0].Name), f.SearchByPropNameAndValue(exParking, "Name", sectionsByBuilding[i][0].Name)));
                }
                //Sorting buildings by name
                List<IBaseBuilding> sortedBuildings = f.Sort_List_By_PropertyName_Generic("Ascending", "Name", allBuildings);
                //Sorting buildings by stage
                List<List<IBaseBuilding>> buildingsByStage = f.SortListOfObjectsByParameterToSeparateLists(sortedBuildings, "StageName");
                //Creating Stages
                List<BaseBigAreaModel> stages = new List<BaseBigAreaModel>(); ;
                for (int i = 0; i < buildingsByStage.Count; i++)
                {
                    stages.Add(new StageModel(city, f.SearchByPropNameAndValue(borders, "Name", buildingsByStage[i][0].StageName).Area, buildingsByStage[i]));
                }
                //Sorting stages
                List<BaseBigAreaModel> sortedStages = f.Sort_List_By_PropertyName_Generic("Ascending", "Name", stages);
                //Creating Site
                SiteModel site = new SiteModel(city, bName.Text, f.SearchByPropNameAndValue(borders, "Name", bName.Text).Area, sortedStages);
                //Data for parking Table
                var plotNumbers = f.GetSortedListOfParameterValues(sortedBuildings, "PlotNumber");
                //var buildingNames = f.GetSortedListOfParameterValues(sortedBuildings, "Name"); // Old version
                //Getting building names with LINQ
                var buildingNames = 
                    (from build in sortedBuildings
                    where !(build is ParkingBuildingModel)
                    orderby build.Name
                    select build.Name).ToList();
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
                        errLabel.Text = "парковки находятся в разработке";
                        List<string[]> parkTableList= new List<string[]>();
                        foreach (var item in plotNumbers)
                        {
                            parkTableList.Add(f.CreateLineForParkingTable(parkingBlocks, buildingNames, item, borders));
                        }
                        parkTableList.RemoveAll(x => x == null);
                        f.CreateParkingTable(parkTableList, buildingNames);
                    }
                }
                this.Show();
            }
        }
        private void bCityCreate_Click(object sender, EventArgs e)
        {
            var cmf = new CityModelForm(this);
            if (Functions.parkingCalcTypeList != null)
            {
                foreach (var item in Functions.parkingCalcTypeList)
                {
                    cmf.cbParking.Items.Add(item.Name);
                }
                cmf.cbParking.SelectedIndex = Functions.parkingCalcTypeList.Count - 1;
            }
            if (Functions.amenitiesCalcTypeList != null)
            {
                foreach (var item in Functions.amenitiesCalcTypeList)
                {
                    cmf.cbAmenities.Items.Add(item.Name);
                }
                cmf.cbAmenities.SelectedIndex = Functions.amenitiesCalcTypeList.Count - 1;
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
                foreach (var item in allBuildings)
                {
                    if (item is ApartmentBuildingModel mod)
                    {
                        f.CreateAmenitiesReqText(mod.AmenitiesReq, mod.MidPoint);
                        f.CreateParkingReqText(mod.TotalParkingReq, mod.MidPoint);
                    }
                }
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
            //Setting stages part invisible
            rbStages_AllStages.Visible = false;
            rbStages_SingleStage.Visible = false;
            cbStages_stages.Visible = false;
            //Setting site part visible
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
    }
}
