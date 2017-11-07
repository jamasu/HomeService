using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeService.Controllers;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;

namespace HomeService.Business
{
    public class ListenAlarmChanges
    {
        public HomeController HomeController { get; set; }
        ~ListenAlarmChanges()
        {

        }
        public ListenAlarmChanges(HomeController homeController)
        {
            HomeController = homeController;
        }
        public void Listen()
        {
            HomeController.AlarmEvent += OnAlarmChanged;

        }
        public void OnAlarmChanged(Object sender, EventArgs e)
        {

            HomeController.ProcessNodeData(HomeController.NodeServices).ContinueWith(t => Console.WriteLine(t.Exception),
        TaskContinuationOptions.OnlyOnFaulted);

            if (DateTime.Now.ToString("hh:mm").Equals(HomeController.GetAlarmTime().ToString("hh:mm")))
            {
                HomeController.StartVoiceProgram();
                HomeController.Dispose();
                GC.Collect();
                Thread.Sleep(60000); //sleep for a minute to let this AlarmEvent pass.
            }
        }
    }
}