using Newtonsoft.Json;
using SiteCalculations.Models;
using System;
using System.IO;
using System.Windows.Forms;

namespace SiteCalculations.Forms
{
    public partial class AmenitiesModelForm : Form
    {
        public AmenitiesModelForm()
        {
            InitializeComponent();
        }

        private CityModelForm cityModelForm = null;
        public AmenitiesModelForm(Form callingForm)
        {
            cityModelForm = callingForm as CityModelForm;
            InitializeComponent();
            cbChildren.SelectedIndex = 0;
            cbSport.SelectedIndex = 0;
            cbRest.SelectedIndex = 0;
            cbUtility.SelectedIndex = 0;
            cbTrash.SelectedIndex = 0;
            cbDogs.SelectedIndex = 0;
            cbTotal.SelectedIndex = 0;
            cbGreenery.SelectedIndex = 0;
        }
        private void createParkingButton_Click(object sender, EventArgs e)
        {
            string[] values = new string[25];
            switch (cbChildren.Text)
            {
                case "человека":
                    values[0] = bChildren.Text;
                    break;
                case "квартиру":
                    values[1] = bChildren.Text;
                    break;
                case "м2 площади":
                    values[2] = bChildren.Text;
                    break;
            }
            switch (cbSport.Text)
            {
                case "человека":
                    values[3] = bSport.Text;
                    break;
                case "квартиру":
                    values[4] = bSport.Text;
                    break;
                case "м2 площади":
                    values[5] = bSport.Text;
                    break;
            }
            switch (cbRest.Text)
            {
                case "человека":
                    values[6] = bRest.Text;
                    break;
                case "квартиру":
                    values[7] = bRest.Text;
                    break;
                case "м2 площади":
                    values[8] = bRest.Text;
                    break;
            }
            switch (cbUtility.Text)
            {
                case "человека":
                    values[9] = bUtility.Text;
                    break;
                case "квартиру":
                    values[10] = bUtility.Text;
                    break;
                case "м2 площади":
                    values[11] = bUtility.Text;
                    break;
            }
            switch (cbTrash.Text)
            {
                case "человека":
                    values[12] = bTrash.Text;
                    break;
                case "квартиру":
                    values[13] = bTrash.Text;
                    break;
                case "м2 площади":
                    values[14] = bTrash.Text;
                    break;
            }
            switch (cbDogs.Text)
            {
                case "человека":
                    values[15] = bDogs.Text;
                    break;
                case "квартиру":
                    values[16] = bDogs.Text;
                    break;
                case "м2 площади":
                    values[17] = bDogs.Text;
                    break;
            }
            switch (cbTotal.Text)
            {
                case "человека":
                    values[18] = bTotal.Text;
                    break;
                case "квартиру":
                    values[19] = bTotal.Text;
                    break;
                case "м2 площади":
                    values[20] = bTotal.Text;
                    break;
            }
            switch (cbGreenery.Text)
            {
                case "человека":
                    values[21] = bGreenery.Text;
                    break;
                case "квартиру":
                    values[22] = bGreenery.Text;
                    break;
                case "м2 площади":
                    values[23] = bGreenery.Text;
                    break;
            }
            values[24] = bName.Text;
            if (bName.Text == "")
            {
                errorLabel.Visible = true;
            }
            else
            {
                var f = new Functions();
                errorLabel.Visible = false;
                try
                { f.DeserealiseJson<AmenitiesReqModel>(ref Functions.amenitiesCalcTypeList, @".\amenities.json"); }
                catch { }
                Functions.amenitiesCalcTypeList.Add(new AmenitiesReqModel(values));
                cityModelForm.cbAmenities.DataSource = Functions.amenitiesCalcTypeList;
                cityModelForm.cbAmenities.SelectedIndex = Functions.amenitiesCalcTypeList.Count - 1;
                f.SerealiseJson<AmenitiesReqModel>(ref Functions.amenitiesCalcTypeList, @".\amenities.json");
                
                AmenitiesModelForm obj = (AmenitiesModelForm)Application.OpenForms["AmenitiesModelForm"];
                obj.Close();
            }
        }
    }
}
