using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VehicleTrackingAPI.Models;
using VehicleTrackingAPI.Services.VehicleServices;

namespace VehicleTrackingAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class VehicleTrackingController : ControllerBase
    {
        private readonly IVehicleServices _vehicleServices;

        public VehicleTrackingController(IVehicleServices vehicleServices)
        {
            _vehicleServices = vehicleServices;
        }


        [HttpGet]
        [Route("")]
        public IActionResult Start()
        {
            return Ok("Welcome to Vehicle Tracking API");
        }
        [Route("getall")]
        public async Task<IActionResult> GetAllVehicle()
        {
            return Ok(await _vehicleServices.GetAllVehicle());
        }
        [Route("getbyid")]
        public async Task<IActionResult> GetVehicleById(string id)
        {
            return Ok(await _vehicleServices.GetVehicleById(id));
        }
        [Route("getposition")]
        public async Task<IActionResult> GetVehiclePosition(string id)
        {
            return Ok(await _vehicleServices.GetVehiclePosition(id));
        }
        [Route("getallposition")]
        public async Task<IActionResult> GetVehiclePositionAll(string id)
        {
            return Ok(await _vehicleServices.GetVehiclePositionAll(id));
        }
       
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterVehicle(VehicleModel vehicleModel)
        {
            return Ok(await _vehicleServices.RegisterVehicle(vehicleModel));
        }
        [Route("setposition")]
        public async Task<IActionResult> UpdateVehicleTracking(TrackingModel trackingModel)
        {
            return Ok(await _vehicleServices.UpdateVehicleTracking(trackingModel));
        }

        [HttpDelete]
        [Route("delete")]
        public async Task<IActionResult> DeleteVehicle(string id)
        {
            return Ok(await _vehicleServices.DeleteVehicle(id));
        }
    }
}
