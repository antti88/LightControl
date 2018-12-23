using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using System.IO;
using System.Collections.ObjectModel;
using Android.Util;
using System.Threading.Tasks;

namespace LightControl
{
    [Activity(Label = "AddDevice")]
    public class AddDevice : Activity
    {
        Button addDevice;
        EditText dName;
        EditText dTimer;
        string devName;
        string devTimer;
        //Context context;
        AlertDialog.Builder alert;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddDevice);

            addDevice = FindViewById<Button>(Resource.Id.btnAddNewDev);
            dName = FindViewById<EditText>(Resource.Id.etDevName);
            dTimer = FindViewById<EditText>(Resource.Id.etDevTimer);


            addDevice.Click += AddDevice_Click;


            // Create your application here
        }

        private async void AddDevice_Click(object sender, EventArgs e)
        {
            //Toast.MakeText(this, "Add Button clikced", ToastLength.Short).Show();

            //devicesItem items = new devicesItem();

            if (dName.Text == "" || dTimer.Text.ToString() == "")
            {
                Toast.MakeText(this, "Name or Time is empty!?", ToastLength.Short).Show();
            }
            else
            {
                List<devicesItem> devlist;
                GetDevices GD = new GetDevices();
                devlist = await GD.GetDeviceList();
                alert = new AlertDialog.Builder(this);
                devName = dName.Text;
                devTimer = dTimer.Text.ToString();
                int deviceId = devlist.Count;
                deviceId = deviceId + 1;
                alert.SetTitle("Redy to add device!");
                alert.SetMessage("Press OK when your device is ready to pairing");
                alert.SetPositiveButton("OK", async (senderAlert, args) =>
                 {
                     //Console.WriteLine("Devlis count: " + devlist.Count());
                     List<int> devidslist = new List<int>();
                     try
                     {

                         //foreach (devicesItem items in devlist)
                         //{
                         //    Console.WriteLine("DEVLIST VALUES: " + items.deviceId.ToString());
                         //    if (devlist.Contains(items))
                         //    {
                         //        Log.Debug("LIST","ALREADY ON LIST: " + items.deviceName);
                         //    }
                         //    devidslist.Add(items.deviceId);
                         //    Log.Debug("AddDevice", "Devlist count: " + devidslist.Count);
                         //}
                         //int deviceId = devidslist.Count;

                         try
                         {
                             HttpWebRequestHandler HWRH = new HttpWebRequestHandler(this);
                             Console.WriteLine("TEXTINPUT: " + devName + devTimer);
                             await HWRH.webRestHandler(deviceId.ToString(), devName, null, devTimer, "add");
                             await Task.Delay(1000);                            
                             await HWRH.webRestHandler(deviceId.ToString(), devName, "on", "0", "control");
                         }
                         catch (Exception expection)
                         {
                             Console.WriteLine("ERROR WHEN ADDING DEVICE: " + expection);
                             Toast.MakeText(this, "Error! something went wrong, try again...", ToastLength.Short).Show();
                         }

                         //var main = new Intent(this, typeof(MainActivity));
                         //StartActivity(main);
                         Toast.MakeText(this, "Device " + devName.ToString() + " added!", ToastLength.Short).Show();
                         //this.Recreate();
                     }
                     catch (Exception expection)
                     {
                         Console.WriteLine("foreach error: " + expection);
                         Toast.MakeText(this, "Error! something went wrong, try again...", ToastLength.Short).Show();
                     }

                 });

                Dialog dialog = alert.Create();
                dialog.Show();

            }
        }


    }
}