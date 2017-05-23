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
using System.Threading;
using Android.Util;
using System.Threading.Tasks;

namespace LightControl
{
    class HttpWebRequestHandler
    {
        Context context;
        public HttpWebRequestHandler(Context context)
        {
            this.context = context;
        }

        public void webRestHandler(string deviceId,string deviceName, string deviceState, string timer,string switchcase)
        {
            string url;
            try
            {
                switch (switchcase)
                {
                    case "add":
                        url = "http://192.168.10.54/dbhandler.php/?device=" + deviceId + "&devicename=" + deviceName + "&timer=" + timer + "&state=add";
                        break;
                    case "delete":
                        url = "http://192.168.10.54/dbhandler.php/?device=" + deviceId  + "&state=delete";
                        break;
                    case "control":
                        url = "http://192.168.10.54/controller.php/?device=" + deviceId + "&onoff=" + deviceState + "&timer=" + timer;                      
                        break;
                    
                    default:
                        url = "http://192.168.10.54/fetchall.php";
                        break;           
                }

                //**<-FIRE-AND-FORGET->**//
                Console.WriteLine(url);
                SubmitUrl(url);  
               
            }
            catch (WebException exception)
            {             
                Console.WriteLine("ERROR WHEN SEND GET: " + exception);
            }
        }

        public static void SubmitUrl(string url)
        {
            Task.Factory.StartNew(() => SubmitUrlPrivate(url)).ConfigureAwait(false);
        }

        private static void SubmitUrlPrivate(string url)
        {
            using (var webClient = new WebClient())
            {
                try
                {
                    Log.Info("Start Invoking URL: {0}", url);
                    using (webClient.OpenRead(url))
                    {
                        Log.Info("End Invoking URL: {0}", url);
                    }
                }
                catch (WebException ex)
                {
                    if (ex.Status != WebExceptionStatus.Timeout)
                    {
                        Log.Info("Exception Invoking URL: {0} \n {1}", url, ex.ToString());
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Log.Info("Exception Invoking URL: {0} \n {1}", url, ex.ToString());
                    throw;
                }
            }
        }
    }
}