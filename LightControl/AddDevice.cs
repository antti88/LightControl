﻿using System;
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
            Toast.MakeText(this, "Add Button clikced", ToastLength.Short).Show();
            
            //devicesItem items = new devicesItem();

            if (dName.Text == "" || dTimer.Text.ToString() == "")
            {
                Toast.MakeText(this, "Name or Time is empty!?", ToastLength.Short).Show();
            }
            else
            {
                alert = new AlertDialog.Builder(this);
                devName = dName.Text;
                devTimer = dTimer.Text.ToString();
                ObservableCollection<devicesItem> devlist;
                GetDevices GD = new GetDevices();
                devlist = await GD.GetDeviceList();
                
                alert.SetTitle("Redy to add device!");
                alert.SetMessage("Press OK when your device is ready to pairing");
                alert.SetPositiveButton("OK", (senderAlert, args) =>
                 {
                     Console.WriteLine("Devlis count: " + devlist.Count());
                     List<int> devidslist = new List<int>();
                     try
                     {

                         foreach (devicesItem items in devlist)
                         {
                             Console.WriteLine("DEVLIST VALUES: " + items.deviceId.ToString());
                             devidslist.Add(items.deviceId);
                         }
                         int deviceId = devidslist.Count;
                         if (deviceId != 0)
                         {
                             try
                             {
                                 HttpWebRequestHandler HWRH = new HttpWebRequestHandler(this);
                                 Console.WriteLine("TEXTINPUT: " + devName + devTimer);
                                 HWRH.webRestHandler(deviceId.ToString(), devName, null, devTimer, "add");
                                 HWRH.webRestHandler(deviceId.ToString(), devName, "on", "0", "control");
                             }
                             catch (Exception expection)
                             {
                                 Console.WriteLine("ERROR WHEN ADDING DEVICE: " + expection);
                             }

                             var main = new Intent(this, typeof(MainActivity));
                             StartActivity(main);
                         }

                     }
                     catch (Exception expection)
                     {
                         Console.WriteLine("foreach error: " + expection);
                     }

                 });

                Dialog dialog = alert.Create();
                dialog.Show();

            }
        }

        
    }
}