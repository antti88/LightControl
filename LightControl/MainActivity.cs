using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;
using Android.Content;
using Android.Util;
using System.Collections.ObjectModel;

namespace LightControl
{
    [Activity(Label = "LightControl", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        ListView lvDevices;
        //List<devicesItem> listDevices;
        ObservableCollection<devicesItem> listDevices;
        // string[] devicesDBList;
        Button btnDeleteDevice;
        Button btnAddDevice;
        // devices;
        CustomListAdapter adapter;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            //listDevices = new List<devicesItem>();
            listDevices = new ObservableCollection<devicesItem>();

            //GetDevices GD = new GetDevices();
            //listDevices = await GD.GetDeviceList();
            
            //devicesDBList = new string[] { devices };

            //for (int i = 0; i < devicesDBList.Length; ++i)
            //{
            //    Console.WriteLine("DeviceList: " + devicesDBList[i]);
            //}
            //for (int i = 0; i < 5; i++)
            //{
            //    //Console.WriteLine("DeviceList: " + devicesDBList[i]);
            //    devicesItem item = new devicesItem()
            //    {

            //        deviceId = i,
            //        deviceName = "kahvinkeitin " + i,
            //        deviceOn = "on ",
            //        deviceOff = "off",
            //        timer = 1
            //    };

            //    listDevices.Add(item);
            //}

            lvDevices = FindViewById<ListView>(Android.Resource.Id.List);
            btnDeleteDevice = FindViewById<Button>(Resource.Id.btnDeleteDevice);
            btnAddDevice = FindViewById<Button>(Resource.Id.btnAddDevice);
            await getAdapter();
            lvDevices.ChoiceMode = ChoiceMode.Single;
            lvDevices.Adapter = adapter;

            lvDevices.ItemClick += LvDevices_ItemClick;
            btnAddDevice.Click += BtnAddDevice_Click;
            btnDeleteDevice.Click += BtnDeleteDevice_Click;

        }
        public async System.Threading.Tasks.Task getAdapter()
        {
            GetDevices GD = new GetDevices();
            listDevices = await GD.GetDeviceList();
            adapter = new CustomListAdapter(this, listDevices);
            adapter.NotifyDataSetChanged();
            //lvDevices.Adapter = adapter;


        }

        private void BtnDeleteDevice_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Press devicename long to delete device", ToastLength.Long).Show();
            
        }

        private void BtnAddDevice_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Button ADD clicked", ToastLength.Short).Show();
            var activityAdd = new Intent(this, typeof(AddDevice));
            StartActivity(activityAdd);
            
        }

        private void LvDevices_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, "Button ON: " + listDevices[e.Position].deviceId + " clicked", ToastLength.Short).Show();
            var index = lvDevices.SelectedItemId;
            Log.Info("LIST:", "INDEX= " + index);
            HttpWebRequestHandler HWRH = new HttpWebRequestHandler(this);
            HWRH.webRestHandler(index.ToString(), null, null, null, "delete");
        }

        protected void onResume()
        {
            getAdapter();
        }

        protected void onStart()
        {
            getAdapter();
        }

        public void notifyDataChanges()
        {
            adapter.NotifyDataSetChanged();
        }

    }
}

