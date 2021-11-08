using GMap.NET;
using GMap.NET.MapProviders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VehicleTrackingAPI.Function;
using VehicleTrackingAPI.Models;

namespace VehicleTrackingAPI.Services.VehicleServices
{
    public class VehicleServices : IVehicleServices
    {

        //Get
        public async Task<DataResultModel> GetAllVehicle()
        {
            DataResultModel dataResultModel = new DataResultModel();
            try
            {
                List<VehicleModel> vehicleList = new List<VehicleModel>();
                string sql = @"SELECT * FROM [TB_MS_VEHICLE]";
                DataTable dt = Func.ConnectDatabase(sql);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        VehicleModel temp = new VehicleModel();
                        temp.id = dt.Rows[i][0].ToString();
                        temp.name = dt.Rows[i][1].ToString();
                        temp.update_date = dt.Rows[i][2].ToString();
                        temp.create_date = dt.Rows[i][3].ToString();
                        vehicleList.Add(temp);
                    }
                    dataResultModel.result = vehicleList;
                    dataResultModel.success = true;
                }
                else
                {
                    dataResultModel.message = "Don't have data.";
                    dataResultModel.success = false;
                }
             
                return dataResultModel;
            }
            catch(Exception ex)
            {
                Func.WriteLogLocal("ERROR GAV", ex.Message);
                dataResultModel.message = "API Error!";
                dataResultModel.success = false;
                return dataResultModel;
            }
        }
        public async Task<DataResultModel> GetVehicleById(string id)
        {
            DataResultModel dataResultModel = new DataResultModel();
            try
            {
                string sql = @"SELECT * FROM [TB_MS_VEHICLE] WHERE [VEHICLE_ID] = '" + id + "'";
                DataTable dt = Func.ConnectDatabase(sql);

                VehicleModel vehicle = new VehicleModel();
                if (dt.Rows.Count > 0)
                {
                    vehicle.id = dt.Rows[0][0].ToString();
                    vehicle.name = dt.Rows[0][1].ToString();
                    vehicle.update_date = dt.Rows[0][2].ToString();
                    vehicle.create_date = dt.Rows[0][3].ToString();
                    dataResultModel.result = vehicle;
                    dataResultModel.success = true;
                }
                else
                {
                    dataResultModel.message = "Can't find this vehicle id.";
                    dataResultModel.success = false;
                }


                return dataResultModel;
            }
            catch (Exception ex)
            {
                Func.WriteLogLocal("ERROR GVBI", ex.Message);
                dataResultModel.message = "API Error!";
                dataResultModel.success = false;
                return dataResultModel;
            }
        }
        public async Task<DataResultModel> GetVehiclePosition(string id)
        {
            DataResultModel dataResultModel = new DataResultModel();
            try
            {
                GMapProviders.GoogleMap.ApiKey = ConfigurationManager.AppSettings.Get("GMapAPIKey");
                PointLatLng point = new PointLatLng();
                List<Placemark> placemarks = null;
                TrackingModel trackingModel = new TrackingModel();

                string sql = @"SELECT TOP(1) [VEHICLE_ID]
                                        ,[LAT]
                                        ,[LNG]
                                  FROM [VehicleTracking].[dbo].[TB_PR_TRACKING]
                                  WHERE [VEHICLE_ID] = '" + id + "'" +
                                      "ORDER BY [TRACKING_ID] DESC";
                DataTable dt = Func.ConnectDatabase(sql);

                if(dt.Rows.Count > 0)
                {
                    trackingModel.vehicle_id = dt.Rows[0]["VEHICLE_ID"].ToString();
                    trackingModel.lat = dt.Rows[0]["LAT"].ToString();
                    trackingModel.lng = dt.Rows[0]["LNG"].ToString();
                    point.Lat = Func.ConvertDouble(dt.Rows[0]["LAT"].ToString());
                    point.Lng = Func.ConvertDouble(dt.Rows[0]["LNG"].ToString());
                    var GMapStatus = GMapProviders.GoogleMap.GetPlacemarks(point, out placemarks);
                    if (GMapStatus == GeoCoderStatusCode.OK && placemarks != null)
                    {
                        trackingModel.districtName = placemarks[0].DistrictName; 
                        trackingModel.countryName = placemarks[0].CountryName; 
                        trackingModel.areaName = placemarks[0].AdministrativeAreaName; 

                    }

                    dataResultModel.result = trackingModel;
                    dataResultModel.success = true;
                }
                else
                {
                    dataResultModel.message = "Can't find this trakcing.";
                    dataResultModel.success = false;
                }

                return dataResultModel;
            }
            catch(Exception ex)
            {
                Func.WriteLogLocal("ERROR GVP", ex.Message);
                dataResultModel.message = "API Error!";
                dataResultModel.success = false;
                return dataResultModel;
            }
           
        }
        public async Task<DataResultModel> GetVehiclePositionAll(string id)
        {
            DataResultModel dataResultModel = new DataResultModel();
            try
            {
                GMapProviders.GoogleMap.ApiKey = ConfigurationManager.AppSettings.Get("GMapAPIKey");
                PointLatLng point = new PointLatLng();
                List<TrackingModel> trackingModelList = new List<TrackingModel>();
                string sql = @"SELECT [VEHICLE_ID]
                                        ,[LAT]
                                        ,[LNG]
                                  FROM [TB_PR_TRACKING]
                                  WHERE [VEHICLE_ID] = '" + id + "'" +
                                 "ORDER BY [TRACKING_ID] DESC";
                DataTable dt = Func.ConnectDatabase(sql);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        List<Placemark> placemarks = null;
                        TrackingModel trackingModel = new TrackingModel();
                        trackingModel.vehicle_id = dt.Rows[i]["VEHICLE_ID"].ToString();
                        trackingModel.lat = dt.Rows[i]["LAT"].ToString();
                        trackingModel.lng = dt.Rows[i]["LNG"].ToString();
                        point.Lat = Func.ConvertDouble(dt.Rows[i]["LAT"].ToString());
                        point.Lng = Func.ConvertDouble(dt.Rows[i]["LNG"].ToString());
                        var GMapStatus = GMapProviders.GoogleMap.GetPlacemarks(point, out placemarks);
                        if (GMapStatus == GeoCoderStatusCode.OK && placemarks != null)
                        {
                            trackingModel.districtName = placemarks[0].DistrictName;
                            trackingModel.countryName = placemarks[0].CountryName;
                            trackingModel.areaName = placemarks[0].AdministrativeAreaName;

                        }
                        trackingModelList.Add(trackingModel);
                    }

                    dataResultModel.result = trackingModelList;
                    dataResultModel.success = true;
                }
                else
                {
                    dataResultModel.message = "Can't find this trakcing.";
                    dataResultModel.success = false;
                }



                return dataResultModel;
            }
            catch (Exception ex)
            {
                Func.WriteLogLocal("ERROR GVPA", ex.Message);
                dataResultModel.message = "API Error!";
                dataResultModel.success = false;
                return dataResultModel;
            }

        }
       
        //Post
        public async Task<DataResultModel> RegisterVehicle(VehicleModel vehicleModel)
        {
            DataResultModel dataResultModel = new DataResultModel();
            try
            {
                if(string.IsNullOrEmpty(vehicleModel.id) || string.IsNullOrEmpty(vehicleModel.name))
                {
                    dataResultModel.message = "Data not enought!";
                    dataResultModel.success = false;
                    return dataResultModel;
                }

                if (!CheckVehicleRegistered(vehicleModel.id))
                {
                    string sql = @"INSERT INTO [TB_MS_VEHICLE]
                                   ([VEHICLE_ID]
                                   ,[VEHICLE_NAME]
                                   ,[UPDATE_DATE]
                                   ,[CREATE_DATE])
                             VALUES
                                   ('" + vehicleModel.id + "'" +
                                            ",'" + vehicleModel.name + "'" +
                                           @",GETDATE()
                                   ,GETDATE());
                            SELECT TOP(1) * FROM [TB_MS_VEHICLE] WHERE [VEHICLE_NAME] = '" + vehicleModel.name + "' ORDER BY [VEHICLE_ID] DESC;";
                    DataTable dt = Func.ConnectDatabase(sql);

                    VehicleModel vehicle = new VehicleModel();
                    if (dt.Rows.Count > 0)
                    {
                        vehicle.id = dt.Rows[0][0].ToString();
                        vehicle.name = dt.Rows[0][1].ToString();
                        vehicle.update_date = dt.Rows[0][2].ToString();
                        vehicle.create_date = dt.Rows[0][3].ToString();
                    }

                    dataResultModel.result = vehicle;
                    dataResultModel.success = true;
                }
                else
                {
                    dataResultModel.message = "This vehicle id is already used.";
                    dataResultModel.success = false;
                }
                return dataResultModel;
            }
            catch(Exception ex)
            {
                Func.WriteLogLocal("ERROR RV", ex.Message);
                dataResultModel.message = "API Error!";
                dataResultModel.success = false;
                return dataResultModel;
            }
        }
        public async Task<DataResultModel> UpdateVehicleTracking(TrackingModel trackingModel)
        {
            DataResultModel dataResultModel = new DataResultModel();
            try
            {
                string id = trackingModel.vehicle_id;
                double lat = Func.ConvertDouble(trackingModel.lat);
                double lng = Func.ConvertDouble(trackingModel.lng);
                if (CheckVehicleRegistered(id))
                {
                    string sql_insert = @"INSERT INTO [TB_PR_TRACKING]
                                                ([VEHICLE_ID]
                                                ,[LAT]
                                                ,[LNG]
                                                ,[UPDATE_DATE])
                                            VALUES
                                                ('"+ id + "'"+
                                                ",'"+ lat + "'"+
                                                ",'" + lng + "'" +
                                                ",GETDATE())";
                    Func.ConnectDatabase(sql_insert);

                    dataResultModel.success = true;
                }
                else
                {
                    dataResultModel.message = "This vehicle id still not register!";
                    dataResultModel.success = false;
                }
                return dataResultModel;
            }
            catch (Exception ex)
            {
                Func.WriteLogLocal("ERROR DV", ex.Message);
                dataResultModel.message = "API Error!";
                dataResultModel.success = false;
                return dataResultModel;
            }

        }

        //Delete
        public async Task<DataResultModel> DeleteVehicle(string id)
        {
            DataResultModel dataResultModel = new DataResultModel();
            try
            {
                if(CheckVehicleRegistered(id))
                {
                    string sql_delete = @"DELETE FROM [TB_MS_VEHICLE] WHERE [VEHICLE_ID] = '"+ id + "'";
                    Func.ConnectDatabase(sql_delete);
    
                    dataResultModel.success = true;
                }
                else
                {
                    dataResultModel.message = "Can't find this vehicle id.";
                    dataResultModel.success = false;
                }

                return dataResultModel;
            }
            catch (Exception ex)
            {
                Func.WriteLogLocal("ERROR DV", ex.Message);
                dataResultModel.message = "API Error!";
                dataResultModel.success = false;
                return dataResultModel;
            }

        }
            

        private bool CheckVehicleRegistered(string id)
        {
            try
            {
                string sql = @"SELECT [VEHICLE_ID] FROM [TB_MS_VEHICLE] WHERE [VEHICLE_ID] = '" + id + "'";
                DataTable dt = Func.ConnectDatabase(sql);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                Func.WriteLogLocal("ERROR CVR",ex.Message);
                return false;
            }
        }

    }
}
