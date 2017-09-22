using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;

namespace HomeService.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CallVerisureService([FromServices] INodeServices nodeServices)
        {
            var climateDataRaw = await nodeServices.InvokeExportAsync<ClimateDataRaw[]>(@"C:\Users\pa_suja\Documents\HomeService\HomeService\wwwroot\lib\Verisure\verisureApp.js", "sendClimateData");
            var alarmData = await nodeServices.InvokeExportAsync<AlarmStatus>(@"C:\Users\pa_suja\Documents\HomeService\HomeService\wwwroot\lib\Verisure\verisureApp.js", "sendAlarmStatus");
            ClimateData[] climateData = new ClimateData[climateDataRaw.Length];
            
            for (int i = 0; i < climateDataRaw.Length; i++)
            {

                climateData[i] = new ClimateData();
                climateData[i].Temperature = float.Parse(climateDataRaw[i].Temperature.Remove(climateDataRaw[i].Temperature.IndexOf('&')));
                if (climateDataRaw[i].Humidity.Equals(string.Empty))
                {
                    climateData[i].Humidity = 0;
                }
                else
                    climateData[i].Humidity = float.Parse(climateDataRaw[i].Humidity.Remove(climateDataRaw[i].Humidity.IndexOf('%')));
                climateData[i].Location = climateDataRaw[i].Location;
                climateData[i].Timestamp = climateDataRaw[i].Timestamp.Replace("Today","I dag");

            }
            ViewData["ClimateData"] = climateData;
            ViewData["AlarmStatusDate"] = alarmData.Date.Replace("Today", "I dag");
            ViewData["AlarmStatus"] = alarmData.Status;
            return View();
             
        }
    }
    public class ClimateDataRaw
    {
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Location { get; set; }
        public string Timestamp { get; set; }
    }
    public class ClimateData
    {
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public string Location { get; set; }
        public string Timestamp { get; set; }

    }
    public class AlarmStatus
    {
        public string Date { get; set; }
        public string Status { get; set; }
    }
}
