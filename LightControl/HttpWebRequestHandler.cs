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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                ThreadPool.QueueUserWorkItem(o => { request.GetResponse();});
                //request.Method = "GET";
                //using GET - request.Headers.Add ("Authorization","Authorizaation value");                   
                //request.ContentType = "application/json";

                //HttpWebResponse myResp = (HttpWebResponse)request.GetResponse();


                //string responseText;

                //using (var response = request.GetResponse())
                //{
                //    using (var reader = new StreamReader(response.GetResponseStream()))
                //    {
                //        responseText = reader.ReadToEnd();
                //        Console.WriteLine(responseText);
                //        Toast.MakeText(context, "Request send to id: " + deviceId, ToastLength.Short).Show();
                //    }
                //}
            }
            catch (WebException exception)
            {
                string responseText;
                //using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                //{
                //    responseText = reader.ReadToEnd();
                //    Console.WriteLine(responseText);
                //}
                Console.WriteLine("ERROR WHEN SEND GET: " + exception);
            }
        }
    }
}