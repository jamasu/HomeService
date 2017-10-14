using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using HomeService.Business;
namespace HomeService.Controllers
{
    public class HomeController : Controller
    {
        public const string VeriSureAppFilePath = @"wwwroot\lib\Verisure\verisureApp.js";
       
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CallVerisureService([FromServices] INodeServices nodeServices)
        {
            var climateDataRaw = await nodeServices.InvokeExportAsync<ClimateDataRaw[]>(VeriSureAppFilePath, "sendClimateData");
            var alarmData = await nodeServices.InvokeExportAsync<AlarmStatus>(VeriSureAppFilePath, "sendAlarmStatus");
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
            ViewData["AlarmUser"] = alarmData.Name;
            ViewData["AlarmLabel"] = alarmData.Label;            
            return View();
             
        }
    }
   
   
    
}
