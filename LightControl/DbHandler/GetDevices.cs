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
using Org.Json;
using Java.Util;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Android.Util;
using System.Collections.ObjectModel;

namespace LightControl
{
    class GetDevices

    {
        Context context;
        public GetDevices()
        {
            //this.context = context;
        }
        public async System.Threading.Tasks.Task<ObservableCollection<devicesItem>> GetDeviceList()
        {
            try
            {

                string url = "http://192.168.10.54/fetchall.php";
                Console.WriteLine(url);
                //string url = "http://192.168.10.54/controller.php/?device=2&onoff=on";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                //using GET - request.Headers.Add ("Authorization","Authorizaation value");                   
                request.ContentType = "application/json";




                HttpWebResponse myResp = (HttpWebResponse)request.GetResponse();

                string responseText;

                using (var response = request.GetResponse())
                {
                    using (var reader = new StreamReader(response.GetResponseStream()))
                    {

                        var responseData = await reader.ReadToEndAsync();
                        Console.WriteLine("responsedata: " + responseData);

                        ObservableCollection<devicesItem> itemslist;
                            //List<devicesItem> itemslist;

                            //devicesItem items = new devicesItem();

                            //var json = JsonConvert.SerializeObject(items);

                            itemslist = JsonConvert.DeserializeObject<ObservableCollection<devicesItem>>(responseData);

                            //Console.WriteLine("DATAJSON: " + itemslist[0].deviceName + itemslist[1].deviceName);
                        
                           
                        
                        return itemslist;
                    }
                }
            }
            catch (WebException exception)
            {
                string responseText;
                //using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                //{
                //    responseText = reader.ReadToEnd();
                Console.WriteLine("WEB ERROR: "+exception);
                //    return "ERROR";
                //}

                //ObservableCollection<devicesItem> itemsError = new ObservableCollection<devicesItem>();
                //devicesItem item = new devicesItem()
                //{

                //    deviceId = 1,
                //    deviceName = "ERROR ",
                //    deviceOn = "err ",
                //    deviceOff = "err",
                //    timer = 1
                //};

                //itemsError.Add(item);
                

                return null;
            }
            catch (Exception exception)
            {
                string responseText;
                //using (var reader = new StreamReader(exception.Response.GetResponseStream()))
                //{
                //    responseText = reader.ReadToEnd();
                Console.WriteLine("JSONPARSE ERROR: "+ exception);
                //    return "ERROR";
                //}

                //ObservableCollection<devicesItem> itemsError = new ObservableCollection<devicesItem>();
                //devicesItem item = new devicesItem()
                //{

                //    deviceId = 1,
                //    deviceName = "ERROR ",
                //    deviceOn = "err ",
                //    deviceOff = "err",
                //    timer = 1
                //};

                //itemsError.Add(item);


                return null;
            }
        }
    }
}