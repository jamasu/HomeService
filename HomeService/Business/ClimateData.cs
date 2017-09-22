using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeService.Business
{
    public class ClimateData
    {
        public float Temperature { get; set; }
        public float Humidity { get; set; }
        public string Location { get; set; }
        public string Timestamp { get; set; }
        public string HumidityAboveMaxAlertValue { get; set; }
        public string HumidityBelowMinAlertValue { get; set; }

    }
}
