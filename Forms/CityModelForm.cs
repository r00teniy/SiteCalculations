using SiteCalculations.Forms;
using SiteCalculations.Models;
using System;
using System.Windows.Forms;

namespace SiteCalculations
{
    public partial class CityModelForm : Form
    {
        public CityModelForm()
        {
            InitializeComponent();
        }
        private MainForm mainForm = null;
        public CityModelForm(Form callingForm)
        {
            mainForm = callingForm as MainForm;
            InitializeComponent();
        }
        private void createAmenitiesButton_Click(object sender, EventArgs e)
        {
            var amf = new AmenitiesModelForm(this);
            amf.Show();
        }

        private void createParkingButton_Click(object sender, EventArgs e)
        {
            var pmf = new ParkingModelForm(this);
            pmf.Show();
        }

        private void createCityButton_Click(object sender, EventArgs e)
        {
            var f = new Functions();
            if (boxName.Text != "" && cbAmenities.SelectedItem != null && cbParking.SelectedItem != null)
            {
                errorLabel.Visible = false;
                f.DeserealiseJson<CityModel>(ref Functions.cityCalcTypeList, @".\city.json");
                Functions.cityCalcTypeList.Add(new CityModel(boxName.Text, Convert.ToDouble(boxLatitude.Text), Convert.ToDouble(boxSqm.Text), Convert.ToDouble(boxSchools.Text),
                    Convert.ToDouble(boxKindergartens.Text), Convert.ToDouble(boxHospitals.Text), f.SearchByPropNameAndValue(Functions.parkingCalcTypeList, "Name", 
                    cbParking.SelectedItem.ToString()), f.SearchByPropNameAndValue(Functions.amenitiesCalcTypeList, "Name", cbAmenities.SelectedItem.ToString())));
                mainForm.cbCity.Items.Add(Functions.cityCalcTypeList[Functions.cityCalcTypeList.Count - 1].CityName);
                mainForm.cbCity.SelectedIndex = Functions.cityCalcTypeList.Count - 1;
                f.SerealiseJson<CityModel>(ref Functions.cityCalcTypeList, @".\city.json");
                CityModelForm obj = (CityModelForm)Application.OpenForms["CityModelForm"];
                obj.Close();
            }
            else
            {
                errorLabel.Visible = true;
            }
        }
    }
}
