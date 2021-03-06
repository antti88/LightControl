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
using Java.Lang;
using System.Net;
using System.IO;
using System.Collections.ObjectModel;
using Android.Util;

namespace LightControl
{
   public class CustomListAdapter : BaseAdapter
    {
        List<devicesItem> devicelistArrayList;
        private Activity activity;

        public CustomListAdapter(Activity activity, List<devicesItem> results)
        {
            this.activity = activity;
            devicelistArrayList = results;
            //mInflater = (LayoutInflater)activity.GetSystemService(Context.LayoutInflaterService);
        }




        public override int Count
        {
            get
            {
                Log.Debug("ArrayAdapter", "List count: " + devicelistArrayList.Count);
                return devicelistArrayList.Count;
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return null;
        }

        public override long GetItemId(int position)
        {
            return devicelistArrayList[position].deviceId;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.devicelistview, parent, false);

            

            var deviceName = view.FindViewById<TextView>(Resource.Id.tvdevicename);
            var deviceOn = view.FindViewById<Button>(Resource.Id.btndeviceon);
            var deviceOff = view.FindViewById<Button>(Resource.Id.btndeviceoff);
            var deviceTimer = view.FindViewById<Button>(Resource.Id.btndevicetimer);
            

            //GetDevices GD = new GetDevices();
            //List<devicesItem> deviceslist = new List<devicesItem>();

            //deviceslist = await GD.GetDeviceList();

            //deviceName.Text = "DeviceName";
            deviceName.Text = devicelistArrayList[position].deviceName + " " + devicelistArrayList[position].deviceId;
            deviceOn.Text = devicelistArrayList[position].deviceOn;
            deviceOff.Text = devicelistArrayList[position].deviceOff;
            deviceTimer.Text = devicelistArrayList[position].timer.ToString();


            var localOn = new LocalOnclickListener();
            var localOff = new LocalOnclickListener();
            var localTimer = new LocalOnclickListener();
            

            localOn.HandleOnClick = () =>
            {
                HttpWebRequestHandler HWRH = new HttpWebRequestHandler(activity);
                int id = position + 1;
                //Toast.MakeText(this.activity, devicelistArrayList[position].Name + "DEVICEON id: " + position, ToastLength.Short).Show();
                HWRH.webRestHandler(id.ToString(),null,"on",devicelistArrayList[position].timer.ToString(),"control");
                Log.Info("ButtonOn: ","Clicked");
            };

            

            localOff.HandleOnClick = () =>
            {
                HttpWebRequestHandler HWRH = new HttpWebRequestHandler(activity);
                int id = position + 1;
                //Toast.MakeText(this.activity, devicelistArrayList[position].Name + "DEVICEON id: " + position, ToastLength.Short).Show();
                HWRH.webRestHandler(id.ToString(), null, "off", "0", "control");
                Log.Info("ButtonOFF: ", "Clicked");

            };

            localTimer.HandleOnClick = () =>
            {

                //Toast.MakeText(this.activity, devicelistArrayList[position].Name + "DEVICETIMER id: " + position, ToastLength.Short).Show();
            };
            
            deviceOn.SetOnClickListener(localOn);
            deviceOff.SetOnClickListener(localOff);
            deviceTimer.SetOnClickListener(localTimer);
            deviceName.LongClick += delegate
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(activity);
                alert.SetTitle("Remove device?");
                alert.SetMessage("Press OK if you really want delete device");
                alert.SetPositiveButton("OK",(senderAlert, args) =>
                 {

                     HttpWebRequestHandler HWRH = new HttpWebRequestHandler(activity);
                     int id = position + 1;
                     HWRH.webRestHandler(id.ToString(), null, null, null, "delete");
                     Toast.MakeText(activity, "DELETING DEVICE... " + id.ToString(), ToastLength.Long).Show();
                     devicelistArrayList.RemoveAt(position);
                     //Recreate activity for refreshing listview
                     activity.Recreate();
                 });

                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                 {
                       
                 });

                Dialog dialog = alert.Create();
                dialog.Show();

            };

            

            //deviceOn.Click += delegate
            //{
            //    int deviceId = devicelistArrayList[position].id;
            //    string deviceName = devicelistArrayList[position].Name;
                
            //    //Toast.MakeText(this.activity,"DeviceOn id: "+ deviceId, ToastLength.Short).Show();

            //    if (deviceName == "kahvinkeitin " + deviceId)
            //    {
            //        Toast.MakeText(this.activity, "IF DeviceOn id: " + deviceId, ToastLength.Short).Show();
            //    }

            //};

            return view;
        }

       
    }
}