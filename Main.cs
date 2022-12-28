using Autodesk.AutoCAD.Runtime;
using SiteCalculations.Forms;
using SiteCalculations.Models;
using System.Linq;

[assembly: CommandClass(typeof(SiteCalculations.Main))]

namespace SiteCalculations
{
    internal class Main
    {
        [CommandMethod("SiteCalc")]
        public void SiteCals()
        {
            MainForm mainForm= new MainForm();
            var f = new Functions();
            try { f.DeserealiseJson<CityModel>(ref Functions.cityCalcTypeList, "\\city.json"); }
            catch { }
            try { f.DeserealiseJson<AmenitiesReqModel>(ref Functions.amenitiesCalcTypeList, "\\amenities.json"); }
            catch { }
            try { f.DeserealiseJson<ParkingReqModel>(ref Functions.parkingCalcTypeList, "\\parking.json"); }
            catch { }
            if (Functions.cityCalcTypeList != null)
            {
                mainForm.cbCity.DataSource= Functions.cityCalcTypeList;
                mainForm.cbCity.DisplayMember = "CityName";
                
            }
            try
            {
                string[] settings = f.RetrieveSettings();
                mainForm.cbCity.SelectedIndex = (settings[0] != null) ? Functions.cityCalcTypeList.IndexOf(Functions.cityCalcTypeList.Where(x => x.CityName == settings[0]).First()) : mainForm.cbCity.Items.Count - 1;
                mainForm.bName.Text = (settings[1] != null) ? settings[1] : "";
                mainForm.Show();
            }
            catch { }
        }
    }
}
