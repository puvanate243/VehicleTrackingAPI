using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTrackingAPI.Models
{
    public class TrackingModel
    {
        public string vehicle_id { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public string districtName { get; set; }
        public string countryName { get; set; }
        public string areaName { get; set; }
   
    }
}
