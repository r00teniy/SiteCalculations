using Newtonsoft.Json;
using SiteCalculations.Models;
using System;
using System.IO;
using System.Windows.Forms;

namespace SiteCalculations.Forms
{
    public partial class ParkingModelForm : Form
    {
        public ParkingModelForm()
        {
            InitializeComponent();
        }
        private CityModelForm cityModelForm = null;
        public ParkingModelForm(Form callingForm)
        {
            cityModelForm = callingForm as CityModelForm;
            InitializeComponent();
            cbGuest.SelectedIndex = 0;
            cbLong.SelectedIndex = 0;
        }
        private void createParkingButton_Click(object sender, EventArgs e)
        {
            string[] values = new string[15];
            switch (cbLong.Text)
            {
                case "человека":
                    values[0] = bLong.Text;
                    break;
                case "квартиру":
                    values[1] = bLong.Text;
                    break;
                case "м2 площади":
                    values[2] = bLong.Text;
                    break;
            }
            values[3] = bShort.Text;
            switch (cbGuest.Text)
            {
                case "человека":
                    values[4] = bGuest.Text;
                    break;
                case "квартиру":
                    values[5] = bGuest.Text;
                    break;
                case "м2 площади":
                    values[6] = bGuest.Text;
                    break;
            }
            values[7] = bSchool.Text;
            values[8] = bKindergarten.Text;
            values[9] = bHospital.Text;
            values[10] = bSport.Text;
            values[11] = bFastFood.Text;
            values[12] = bOffice.Text;
            values[13] = bStore.Text;
            values[14] = bName.Text;
            if (bName.Text == "")
            {
                errorLabel.Visible = true;
            }
            else
            {
                var f = new Functions();
                errorLabel.Visible = false;
                f.DeserealiseJson<ParkingReqModel>(ref Functions.parkingCalcTypeList, @".\parking.json");
                Functions.parkingCalcTypeList.Add(new ParkingReqModel(values));
                cityModelForm.cbParking.Items.Add(Functions.parkingCalcTypeList[Functions.parkingCalcTypeList.Count - 1].Name);
                cityModelForm.cbParking.SelectedIndex = Functions.parkingCalcTypeList.Count - 1;
                f.SerealiseJson<ParkingReqModel>(ref Functions.parkingCalcTypeList, @".\parking.json");
                ParkingModelForm obj = (ParkingModelForm)Application.OpenForms["ParkingModelForm"];
                obj.Close();
            }
        }
    }
}
