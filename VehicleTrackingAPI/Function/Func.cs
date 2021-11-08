using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleTrackingAPI.Function
{
    public class Func
    {
        //Connect SQL
        public static DataTable ConnectDatabase(string query)
        {
            string connectionString = "DatabaseAccess";
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString);
            SqlDataAdapter data = new SqlDataAdapter(query, conn);
            DataTable dataTable = new DataTable();

            try
            {

                int recordsAffected = data.Fill(dataTable);
                if (recordsAffected > 0)
                {
                    foreach (DataRow dataRow in dataTable.Rows)
                    {
                        System.Console.WriteLine(dataRow[0]);
                    }
                }

                return dataTable;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return null;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
            }
        }


        //Convert
        public static double ConvertDouble(string str)
        {
            double d;
            if (double.TryParse(str, out d))
            {
                return d;
            }
            else
            {
                return 0;
            }
        }
        public static int ConvertInt(string str)
        {
            int i;
            if (int.TryParse(str, out i))
            {
                return i;
            }
            else
            {
                return 0;
            }
        }
        public static DateTime? ConvertDate(string str)
        {
            DateTime dt_out;
            if (true == DateTime.TryParse(str, out dt_out))
            {
                return dt_out;
            }
            else
            {
                if (3 == str.Split('/').Length)
                {
                    string str2 = str.Split('/')[2] + "/" + str.Split('/')[1] + "/" + str.Split('/')[0];
                    if (true == DateTime.TryParse(str2, out dt_out))
                    {
                        return dt_out;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        //Log
        public static void WriteLogLocal(string err_cd, string str)
        {
            string path = Directory.GetCurrentDirectory();
            if (false == Directory.Exists(path + "/Log"))
            {
                Directory.CreateDirectory(path + "/Log");
            }

            string s_file = path + "/Log/" + DateTime.Now.ToString("yyyyMMdd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + ".txt";

            str = DateTime.Now.ToString("yyyy/MM/dd", System.Globalization.DateTimeFormatInfo.InvariantInfo) + " " +
                    DateTime.Now.ToString("HH:mm:ss") + " " + err_cd + " : " + str;
            StreamWriter writer = new StreamWriter(s_file, true);
            writer.WriteLine(str);
            writer.Close();
        }

    }
}
