using Autodesk.AutoCAD.Runtime;
using SiteCalculations.Forms;
using SiteCalculations.Models;

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
                foreach (var item in Functions.cityCalcTypeList)
                {
                    mainForm.cbCity.Items.Add(item.CityName);
                }
                mainForm.cbCity.SelectedIndex = mainForm.cbCity.Items.Count - 1;
            }
            mainForm.Show();
        }
    }
}
