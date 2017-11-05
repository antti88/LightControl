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

        public async Task<string> webRestHandler(string deviceId,string deviceName, string deviceState, string timer,string switchcase)
        {
            string temp = "";
            string url;
            Log.Debug("WebHandler", "Starting " + switchcase);
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
                    case "temp":
                        url = "http://192.168.10.54/fetchtemp.php";
                        break;

                    default:
                        url = "http://192.168.10.54/fetchall.php";
                        break;           
                }

                //**<-FIRE-AND-FORGET->**//
                Console.WriteLine("Urli= ",url);
                temp = await SubmitUrl(url);  
                
               
            }
            catch (WebException exception)
            {             
                Console.WriteLine("ERROR WHEN SEND GET: " + exception);
            }
            return temp;
        }

        public async Task<string> SubmitUrl(string url)
        {
           
          string temp = await Task.Factory.StartNew(() => SubmitUrlPrivate(url)).ConfigureAwait(false);
            Log.Debug("Response","server response: " + temp);
          return temp;
        }

        private static string SubmitUrlPrivate(string url)
        {
            string restemp = "";
           


                using (var webClient = new WebClient())
                {
                    try
                    {
                        Log.Info("Start Invoking URL: {0}", url);
                        using (webClient.OpenRead(url))
                        {
                        if (url.Contains("add"))
                        {
                            //DO nothing
                        }
                        else
                        {
                            Log.Info("End Invoking URL: {0}", url);
                            try
                            {

                                restemp = webClient.DownloadString(url);
                                return restemp;
                            }
                            catch (Exception exx)
                            {
                                Log.Error("TempFetch", "Error when fetch: " + exx);
                                return restemp;
                            }
                        }
                        }
                    }
                    catch (WebException ex)
                    {
                        if (ex.Status != WebExceptionStatus.Timeout)
                        {
                            Log.Info("Exception Invoking URL: {0} \n {1}", url, ex.ToString());
                            return restemp;
                            throw;

                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Info("Exception Invoking URL: {0} \n {1}", url, ex.ToString());
                        return restemp;
                        throw;
                    }
                    
                
                
            }
            return restemp;
        }
    }
}