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
using System.Threading;
using Android.Util;
using System.Threading.Tasks;

namespace LightControl
{
    class HttpWebRequestHandler
    {
        Context context;
        string DeviceIp = "192.168.0.11";
        int count = 0;
        public HttpWebRequestHandler(Context context)
        {
            this.context = context;
        }

        public async Task<string> webRestHandler(string deviceId, string deviceName, string deviceState, string timer, string switchcase)
        {
            string temp = "";
            string url;
            Log.Debug("WebHandler", "Starting " + switchcase);
            try
            {
                switch (switchcase)
                {
                    case "add":
                        url = "http://" + DeviceIp + "/dbhandler.php/?device=" + deviceId + "&devicename=" + deviceName + "&timer=" + timer + "&state=add";
                        break;
                    case "delete":
                        url = "http://" + DeviceIp + "/dbhandler.php/?device=" + deviceId + "&devicename=" + deviceName + "&timer=" + timer + "&state=delete";
                        break;
                    case "control":
                        url = "http://" + DeviceIp + "/controller.php/?device=" + deviceId + "&onoff=" + deviceState + "&timer=" + timer;
                        break;
                    case "temp":
                        url = "http://" + DeviceIp + "/fetchtemp.php";
                        break;
                    case "start":
                        url = "http://" + DeviceIp + "/start.php";
                        break;
                    case "shutdown":
                        url = "http://" + DeviceIp + "/shutdown.php";
                        break;
                    case "caselighton":
                        url = "http://" + DeviceIp + "/caselighton.php";
                        break;
                    case "caselightclose":
                        url = "http://" + DeviceIp + "/caselightclose.php";
                        break;
                    default:
                        url = "http://" + DeviceIp + "/fetchall.php";
                        break;
                }

                //**<-FIRE-AND-FORGET->**//
                Console.WriteLine("Urli= ", url);
                temp = await SubmitUrl(url);


            }
            catch (WebException exception)
            {
                Console.WriteLine("ERROR WHEN SEND GET: " + exception);
                Toast.MakeText(context, "Error, something went wrong. try again...", ToastLength.Short).Show();
            }
            return temp;
        }

        public async Task<string> SubmitUrl(string url)
        {

            string temp = await Task.Factory.StartNew(() => SubmitUrlPrivate(url)).ConfigureAwait(false);
            //WebRequest http = WebRequest.Create(url);
            //WebResponse response = http.GetResponse();
            //Stream dataStream = response.GetResponseStream();
            //StreamReader sr = new StreamReader(dataStream);
            //string temp = sr.ReadToEnd();
            count++;
            Log.Debug("Response", "Count: " + count + "server response: " + temp);
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
                    webClient.OpenRead(url);
                    if (url.Contains("timer"))
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