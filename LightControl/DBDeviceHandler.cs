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
            }
        }

    }
}