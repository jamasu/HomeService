using HomeService.Controllers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeService.Business
{

    public class AlarmChanges
    {

        public EventHandler AlarmEvent;

        public void Begin()
        {

            if (AlarmEvent != null)
            {
                AlarmEvent(this, EventArgs.Empty);
               
            }
        }
    }
}
