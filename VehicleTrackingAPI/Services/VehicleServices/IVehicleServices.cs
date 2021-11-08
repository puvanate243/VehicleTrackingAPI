using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleTrackingAPI.Models;

namespace VehicleTrackingAPI.Services.VehicleServices
{
    public interface IVehicleServices
    {
        //Get
        Task<DataResultModel> GetAllVehicle();
        Task<DataResultModel> GetVehicleById(string id);
        Task<DataResultModel> GetVehiclePosition(string id);
        Task<DataResultModel> GetVehiclePositionAll(string id);

        //Post
        Task<DataResultModel> RegisterVehicle(VehicleModel vehicleModel);
        Task<DataResultModel> UpdateVehicleTracking(TrackingModel trackingModel);

        //Delete
        Task<DataResultModel> DeleteVehicle(string id);
    }
}
