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
using Android.Util;
using System.Threading.Tasks;

namespace LightControl
{
   public class DBDeviceHandler
    {
        string dId;
        string dName;
        string dTimer;
        Context context;
       public DBDeviceHandler(Context context,string device,string devicename, string timer)

        {
            this.dId = device;
            this.dName = devicename;
            this.dTimer = timer;
            this.context = context;

        }

        public void deleteDevice()
        {
            try
            {

                HttpWebRequestHandler HWRH = new HttpWebRequestHandler(context);
                HWRH.webRestHandler(dId, dName, null, dTimer, "delete");
            }
            catch (Exception expcetion)
            {
                Console.WriteLine("ERROR WHEN DELETING DEVICE: " + expcetion);
                Toast.MakeText(context, "Error, something went wrong. try again...", ToastLength.Short).Show();
            }
        }

        public void addDevice()
        {
            try
            {

                HttpWebRequestHandler HWRH = new HttpWebRequestHandler(context);
                HWRH.webRestHandler(dId, dName, null, dTimer, "add");

            }
            catch (Exception exception)
            {
                Console.WriteLine("ERROR WHEN ADDING DEVICE: " + exception);
                Toast.MakeText(context, "Error, something went wrong. try again...", ToastLength.Short).Show();
            }
        }
        public async Task<string> GetTemp()
        {
            string temp = "";
            try
            {
                HttpWebRequestHandler HWRH = new HttpWebRequestHandler(context);
                temp = await HWRH.webRestHandler(null, null, null, null, "temp");

            }
            catch(Exception ex)
            {
                Log.Error("GetTemp", "Error when trying to get temp: " + ex);
                Toast.MakeText(context, "Error, something went wrong. try again...", ToastLength.Short).Show();
            }
            return temp;
        }

    }
}