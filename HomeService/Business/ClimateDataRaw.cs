using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeService.Business
{
    public class ClimateDataRaw
    {
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Location { get; set; }
        public string Timestamp { get; set; }
        public string humidityAboveMaxAlertValue { get; set; }
        public string humidityBelowMinAlertValue { get; set; }
    }
}
