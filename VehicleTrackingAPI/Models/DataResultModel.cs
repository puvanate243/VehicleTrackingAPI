using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTrackingAPI.Models
{
    public class DataResultModel
    {
        public object result { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
    }
}
