using Android.App;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;
using System;
using Android.Content;
using Android.Util;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Android.Content.PM;

namespace LightControl
{
    [Activity(Label = "Marvin Control Panel", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {

        ListView lvDevices;
        TextView tvTemp;
        //List<devicesItem> listDevices;
        List<devicesItem> listDevices;
        // string[] devicesDBList;
        Button btnDeleteDevice;
        Button btnAddDevice;
        string nonetwork = "No Connection";
        CheckWiFi checkwifi;
        // devices;
        public CustomListAdapter adapter;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            listDevices = new List<devicesItem>();
            lvDevices = FindViewById<ListView>(Android.Resource.Id.List);
            btnDeleteDevice = FindViewById<Button>(Resource.Id.btnDeleteDevice);
            btnAddDevice = FindViewById<Button>(Resource.Id.btnAddDevice);
            tvTemp = FindViewById<TextView>(Resource.Id.tvTemp1);
            checkwifi = new CheckWiFi();
            try
            {
                Console.WriteLine("Wificheck");
                NetworkState state = checkwifi.State;
                Console.WriteLine("Wificheck: " + state.ToString());
                if (state != NetworkState.ConnectedWifi)
                {
                    Console.WriteLine("Starting show dialog");
                    ShowDialog("No WiFi", "You can only control lights via WiFi");
                }
                await getAdapter();

            }
            catch (Exception ex)
            {

                Console.WriteLine("Cant get Adapter: " + ex);
                Toast.MakeText(this, "No devices found! No Wlan on? wrong network?", ToastLength.Long).Show();
            }
            StartTempFetch();
            lvDevices.ItemClick += LvDevices_ItemClick;
            btnAddDevice.Click += BtnAddDevice_Click;
            btnDeleteDevice.Click += BtnDeleteDevice_Click;

        }
        public async System.Threading.Tasks.Task getAdapter()
        {
            try
            {
                Log.Error(nonetwork, "getAdapter try: ");
                GetDevices GD = new GetDevices();
                if (listDevices != null)
                {
                    listDevices.Clear();
                }
                
                listDevices = await GD.GetDeviceList();
                Log.Error(nonetwork, "listDevices count: " + listDevices.Count);
                if (listDevices == null)
                {
                    Log.Error(nonetwork, "getAdapter Listdevice: null");
                    Toast.MakeText(this, "No devices found! No Wlan on? wrong network?", ToastLength.Long).Show();
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);

                    alert.SetTitle("No network");
                    alert.SetMessage("Recreating application...");
                    alert.SetPositiveButton("OK", (senderAlert, args) =>
                    {
                        this.Recreate();
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                }
                else
                {
                    
                    Log.Error(nonetwork, "getAdapter Listdevice: Else");
                    adapter = new CustomListAdapter(this, listDevices);
                    lvDevices.Adapter = adapter;
                }
            }
            catch (Exception exce)
            {
                Log.Error(nonetwork, "getAdapter catch: " + exce);
                Toast.MakeText(this, "No devices found! No Wlan? Wrong network?", ToastLength.Long).Show();
            }

        }

        private void BtnDeleteDevice_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Press devicename long to delete device", ToastLength.Long).Show();

        }

        private void BtnAddDevice_Click(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine("Wificheck");
                NetworkState state = checkwifi.State;
                Console.WriteLine("Wificheck: " + state.ToString());
                if (state != NetworkState.ConnectedWifi)
                {
                    Console.WriteLine("Starting show dialog");
                    ShowDialog("No WiFi", "You can only control lights via WiFi");
                }
                else
                {//Toast.MakeText(this, "Button ADD clicked", ToastLength.Short).Show();
                    var activityAdd = new Intent(this, typeof(AddDevice));
                    StartActivity(activityAdd);
                    this.OnPause();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in btn add device: " + ex);
                Toast.MakeText(this, "Error, try again...", ToastLength.Long).Show();
            }
        }

        private void LvDevices_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            //Toast.MakeText(this, "Button ON: " + listDevices[e.Position].deviceId + " clicked", ToastLength.Short).Show();
            var index = lvDevices.SelectedItemId;
            index = index + 1;
            Log.Info("LIST:", "INDEX= " + index);
            HttpWebRequestHandler HWRH = new HttpWebRequestHandler(this);
            HWRH.webRestHandler(index.ToString(), "deleting", "deleting", "deleting", "delete");
        }



        protected override async void OnResume()
        {
            base.OnResume();
            Console.WriteLine("ONRESUME START:");

            await getAdapter();
            //listDevices.Clear();
            //GetDevices GD = new GetDevices();
            //listDevices = await GD.GetDeviceList();
            //adapter.NotifyDataSetChanged();
        }



        protected override void OnRestart()
        {
            base.OnStart();
            Console.WriteLine("ONSTART START:");
            //GetDevices GD = new GetDevices();
            //listDevices = await GD.GetDeviceList();
            //adapter.NotifyDataSetChanged();
        }

        public void updateList(CustomListAdapter customadapter)
        {
            //lvDevices = FindViewById<ListView>(Android.Resource.Id.List);
            //lvDevices.Invalidate();
            //lvDevices.Adapter = customadapter;

        }

        public async void StartTempFetch()
        {
            
           while (true)
            {
                Log.Info("Temp", "While started fetchin temp 20s");
                DBDeviceHandler DBDH = new DBDeviceHandler(this, null, null, null);
                string temp = await DBDH.GetTemp();
                if (String.IsNullOrEmpty(temp))
                {
                    Log.Info("Temp", "not available");
                    tvTemp.Text = "Temp not available";
                }
                else
                {
                    try
                    {
                        string output = Regex.Replace(temp, @"[|[|]|]", "");
                        //dynamic data = JsonConvert.DeserializeObject(temp);
                        //dynamic obj = JsonConvert.DeserializeObject(temp);
                        Log.Info("Regex", "Output= " + output);
                        string temparature = JObject.Parse(output)["temparature"].ToString();
                        string date = JObject.Parse(output)["tdate"].ToString();
                       

                        Log.Info("Temp", "temp and date is:" + temp);
                        tvTemp.Text = date + " Temp is: " + temparature;
                    }
                    catch(Exception ex)
                    {
                        Log.Info("JsonParse","Json parse error: " + ex);
                        tvTemp.Text = temp;
                    }
                }

                Log.Info("Temp", "Fetch done: " + tvTemp.Text);

                await Task.Delay(300000);


            }
        
        }

        public void ShowDialog(string Title, string Message, int IconAttribute = Android.Resource.Attribute.AlertDialogIcon)
        {
            

            var builder = new AlertDialog.Builder(this);
            builder.SetIconAttribute(IconAttribute);
            builder.SetTitle(Title);
            builder.SetMessage(Message);

            builder.SetPositiveButton("Open settings", (senderAlert, args) =>
            {
                this.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionWifiSettings));
                //try
                //{
                //    Console.WriteLine("Wificheck");
                //    NetworkState state = checkwifi.State;
                //    Console.WriteLine("Wificheck: " + state.ToString());
                //    if (state != NetworkState.ConnectedWifi)
                //    {
                //        Console.WriteLine("Starting show dialog");
                //        ShowDialog("No WiFi", "You can only control lights via WiFi");
                //    }
                   
                //}
                //catch (Exception ex)
                //{
                //    Toast.MakeText(this, "Error, try again...", ToastLength.Long).Show();
                //}
            });
            builder.Show();
            
        }

    }
}

