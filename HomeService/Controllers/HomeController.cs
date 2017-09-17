﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HomeService.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CallVerisureService([FromServices] INodeServices nodeServices)
        {
            var climateData = await nodeServices.InvokeExportAsync<ClimateData[]>(@"C:\Users\pa_suja\Source\Repos\HomeService\HomeService\wwwroot\lib\Verisure\verisureApp.js", "sendClimateData");
            var alarmData = await nodeServices.InvokeExportAsync<AlarmStatus>(@"C:\Users\pa_suja\Source\Repos\HomeService\HomeService\wwwroot\lib\Verisure\verisureApp.js", "sendAlarmStatus");
            foreach (var data in climateData)
            {
                data.Temperature = data.Temperature.Remove(data.Temperature.IndexOf('&'));
            }
            ViewData["ClimateData"] = climateData;
            ViewData["AlarmStatusDate"] = alarmData.Date;
            ViewData["AlarmStatus"] = alarmData.Status;
            return View();

        }
    }
    public class ClimateData
    {
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Location { get; set; }
        public string Timestamp { get; set; }
    }
    public class AlarmStatus
    {
        public string Date { get; set; }
        public string Status { get; set; }
    }
}
