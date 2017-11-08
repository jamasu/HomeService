using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using HomeService.Business;
using CognitiveServicesTTS;
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;

namespace HomeService.Controllers
{
    public class HomeController : Controller
    {
        private const string VeriSureAppFilePath = @"wwwroot\lib\Verisure\verisureApp.js";
        private const string APIToken = "f905040cdb3e4d9482aba2d35d3a2bae";
        private const string VoiceFile = @"Business\VoiceFile.txt";
        private const string CognitiveServiceRun = @"C:\Users\pa_suja\Source\Repos\HomeService\VoiceCortana\bin\Debug\CognitiveServicesTTS.exe";

        public INodeServices NodeServices { get; set; }
        public EventHandler AlarmEvent;

        public string AlarmTime { get; set; }
        public bool BeginStarted { get; set; }

        public HomeController(INodeServices nodeServices)
        {
            NodeServices = nodeServices;
        }

        public IActionResult Index()
        {
            return View("Index");
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
                climateData[i].Timestamp = climateDataRaw[i].Timestamp.Replace("Today", "I dag");

            }
            ViewData["ClimateData"] = climateData;
            ViewData["AlarmStatusDate"] = alarmData.Date.Replace("Today", "I dag");
            ViewData["AlarmStatus"] = alarmData.Status;
            ViewData["AlarmUser"] = alarmData.Name;
            ViewData["AlarmLabel"] = alarmData.Label;
            AlarmTime = alarmData.Date;
            WriteToVoiceFile(climateData, alarmData);
            StartVoiceProgram();

            if (!BeginStarted)
            {
                BeginStarted = true;
                ListenAlarmChanges listenAlarmChanges = new ListenAlarmChanges(this); //Create listening on Alarmchanges
                listenAlarmChanges.Listen();
                Begin(); //begin eventhandler run only once.
            }

            return View();

        }
        public async Task ProcessNodeData([FromServices] INodeServices nodeServices)
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
                climateData[i].Timestamp = climateDataRaw[i].Timestamp.Replace("Today", "I dag");

            }
            ViewData["ClimateData"] = climateData;
            ViewData["AlarmStatusDate"] = alarmData.Date.Replace("Today", "I dag");
            ViewData["AlarmStatus"] = alarmData.Status;
            ViewData["AlarmUser"] = alarmData.Name;
            ViewData["AlarmLabel"] = alarmData.Label;
            AlarmTime = alarmData.Date;
        }
        public void Begin()
        {
            Thread thread = new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(5000);
                    if (AlarmEvent != null)
                    {
                        AlarmEvent(this, null);
                    }
                }
            });
            thread.Start();
        }
        public DateTime GetAlarmTime()
        {
            string alarmTimeNow = AlarmTime.Replace("Today", "");
            alarmTimeNow = alarmTimeNow.Replace("Yesterday", "");
            alarmTimeNow = alarmTimeNow.Replace("PM", "");
            alarmTimeNow = alarmTimeNow.Replace("AM", "");
            DateTime dt = Convert.ToDateTime(alarmTimeNow);

            return dt;
        }
        public void WriteToVoiceFile(ClimateData[] climateDataObjecsToFile, AlarmStatus alarmData)
        {
            FileStream fstream = new FileStream(VoiceFile, FileMode.Truncate);
            string fileContent = ClimateStatusCheck(climateDataObjecsToFile, alarmData);
            byte[] info = new UTF8Encoding(true).GetBytes(fileContent);

            fstream.Write(info, 0, fileContent.Length);
            fstream.Dispose();
        }

        private static string ClimateStatusCheck(ClimateData[] climateDataObjecsToFile, AlarmStatus alarmData)
        {
            string fileContent = "";
            fileContent += "Velkommen " + alarmData.Name; fileContent += Environment.NewLine;
            fileContent += Environment.NewLine;
            fileContent += "klokken er.! " + DateTime.Now.ToString("HH:mm"); fileContent += Environment.NewLine;
            fileContent += "Utfører system sjekk !.";

            for (int i = 0; i < climateDataObjecsToFile.Length; i++)
            {
                if (climateDataObjecsToFile[i].Temperature > 22 && climateDataObjecsToFile[i].Temperature > 0)
                {
                    fileContent += climateDataObjecsToFile[i].Location; fileContent += " ";
                    fileContent += " " + climateDataObjecsToFile[i].Temperature + " grader!. for varmt!"; fileContent += Environment.NewLine;
                }
                else if (climateDataObjecsToFile[i].Temperature < 18 && climateDataObjecsToFile[i].Temperature > 0)
                {
                    fileContent += climateDataObjecsToFile[i].Location; fileContent += " ";
                    fileContent += " " + climateDataObjecsToFile[i].Temperature + " grader!. for kaldt!"; fileContent += Environment.NewLine;
                }
                else if (climateDataObjecsToFile[i].Humidity > 55 && climateDataObjecsToFile[i].Humidity > 0)
                {
                    fileContent += climateDataObjecsToFile[i].Location; fileContent += " ";
                    fileContent += " " + " Foktighetsnivå høy!" + climateDataObjecsToFile[i].Humidity; fileContent += Environment.NewLine;
                }
                else if (climateDataObjecsToFile[i].Humidity < 35 && climateDataObjecsToFile[i].Humidity > 0)
                {
                    fileContent += climateDataObjecsToFile[i].Location; fileContent += " ";
                    fileContent += " " + " Foktighetsnivå lav!" + climateDataObjecsToFile[i].Humidity; fileContent += Environment.NewLine;
                }
                else
                {
                    fileContent += climateDataObjecsToFile[i].Location; fileContent += " OK. ";
                }
            }
            return fileContent;
        }

        public void StartVoiceProgram()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = CognitiveServiceRun;
            psi.WorkingDirectory = System.IO.Path.GetDirectoryName(psi.FileName);
            Process.Start(psi);
        }
    }
}