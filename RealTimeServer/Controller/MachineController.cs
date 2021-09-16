using RealTimeServer.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RealTimeServer.Controller
{
    public class MachineController
    {
        static HttpClient client = new HttpClient();
        //Codigo url del PowerBi
        static string powerBiPostUrl = "https://api.powerbi.com/beta/d73a39db-6eda-495d-8000-7579f56d68b7/datasets/80444dc8-184f-4039-a871-378d6dfe53b6/rows?key=qgyeZA6pGbLGh6BpH2pHKe7%2BNXnCAOZaGLKPqkjCPFy6GqmZW3JJ0Ie3xDQkZVZgn7%2FzDZtLrMviudzRr%2BKpcQ%3D%3D";
        
        private static string serverkey = Program.getKey();
        //Metodo para ver los tecnicos
        public static string usuarios()
        {
            SqlConnection conn = new SqlConnection(serverkey);
            conn.Open();
            SqlCommand cmd;
            string insertQuery = "select * from Users;";
            cmd = new SqlCommand(insertQuery, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            string output ="";
            while (dr.Read())
            {
                Console.Write(dr.GetValue(1));
                output = output + dr.GetValue(1) + "\n";
            }
            Console.Write(output);
            dr.Close();
            cmd.Dispose();
            conn.Close();
            return output;

        }
        //Metodo para subir datos en un rango de fechas
        public static string datos()
        {
            SqlConnection conn = new SqlConnection(serverkey);
            conn.Open();
            SqlCommand cmd;
            string insertQuery = "select DESCR, ITEMNUMBER, MACHINENUMBER, TRANSTARTDATETIME, UNITCOST, JOBNUMBER, AUX3 from TransactionLog, Users where CAST(TRANSTARTDATETIME AS Date) >= '2021-08-25' AND CAST(TRANSTARTDATETIME AS Date) < '2021-08-26';";
            cmd = new SqlCommand(insertQuery, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            string output = "";
            while (dr.Read())
            {
                var Maquina = new Maquinas()
                {
                    Fecha = DateTime.Parse(dr.GetValue(3).ToString()),
                    Tecnico = dr.GetValue(0).ToString(),
                    Maquina = dr.GetValue(2).ToString(),
                    Herramienta = dr.GetValue(1).ToString(),
                    Costo = float.Parse(dr.GetValue(4).ToString()),
                    Aux3 = dr.GetValue(6).ToString(),
                    Job = dr.GetValue(5).ToString(),
                    Cantidad = 1
                };
                Console.Write(dr.GetValue(0) + "|" + dr.GetValue(1) + "|" + dr.GetValue(2) + "|" + dr.GetValue(3) + "\n");
                output = output + dr.GetValue(0) +"|" + dr.GetValue(1)+ "|"+ dr.GetValue(2) + "|" + dr.GetValue(3) + "\n";
                var jsonString = JsonConvert.SerializeObject(Maquina);
                var postToPowerBi = HttpPostAsync(powerBiPostUrl, "[" + jsonString + "]");
            }
            Console.Write(output);
            dr.Close();
            cmd.Dispose();
            conn.Close();
            return output;

        }

        //Meotodo para subir un dia en especifico
        public static string todayDate()
        {
            SqlConnection conn = new SqlConnection(serverkey);
            conn.Open();
            SqlCommand cmd;
            string insertQuery = "select DESCR, ITEMNUMBER, MACHINENUMBER, TRANSTARTDATETIME, UNITCOST, JOBNUMBER, AUX3 from TransactionLog, Users where CAST(TRANSTARTDATETIME AS Date) = '2021-08-09';";
            cmd = new SqlCommand(insertQuery, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            string output = "";
            while (dr.Read())
            {
                var Maquina = new Maquinas()
                {
                    Fecha = DateTime.Parse(dr.GetValue(3).ToString()),
                    Tecnico = dr.GetValue(0).ToString(),
                    Maquina = dr.GetValue(2).ToString(),
                    Herramienta = dr.GetValue(1).ToString(),
                    Costo = float.Parse(dr.GetValue(4).ToString()),
                    Aux3 = dr.GetValue(6).ToString(),
                    Job = dr.GetValue(5).ToString(),
                    Cantidad = 1
                };
                Console.Write(dr.GetValue(0) + "|" + dr.GetValue(1) + "|" + dr.GetValue(2) + "|" + dr.GetValue(3) + "\n");
                output = output + dr.GetValue(0) + "|" + dr.GetValue(1) + "|" + dr.GetValue(2) + "|" + dr.GetValue(3) + "\n";
                var jsonString = JsonConvert.SerializeObject(Maquina);
                var postToPowerBi = HttpPostAsync(powerBiPostUrl, "[" + jsonString + "]");
                Console.WriteLine(jsonString);
            }
            Console.Write(output);
            dr.Close();
            cmd.Dispose();
            conn.Close();
            return output;

        }

        //Metodo encargado de subir los datos en tiempo real 
        public static string minuteDate()
        {
            SqlConnection conn = new SqlConnection(serverkey);
            conn.Open();
            SqlCommand cmd;
            string insertQuery = "select DESCR, ITEMNUMBER, MACHINENUMBER, TRANSTARTDATETIME, UNITCOST, JOBNUMBER, AUX3 from TransactionLog, Users where TRANSTARTDATETIME >= dateadd(minute,-10,getdate());";
            cmd = new SqlCommand(insertQuery, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            string output = "";
            while (dr.Read())
            {
                var Maquina = new Maquinas()
                {
                    Fecha = DateTime.Parse(dr.GetValue(3).ToString()),
                    Tecnico = dr.GetValue(0).ToString(),
                    Maquina = dr.GetValue(2).ToString(),
                    Herramienta = dr.GetValue(1).ToString(),
                    Costo = float.Parse(dr.GetValue(4).ToString()),
                    Aux3 = dr.GetValue(6).ToString(),
                    Job = dr.GetValue(5).ToString(),
                    Cantidad = 1
                };
                output = output + dr.GetValue(0) + "|" + dr.GetValue(1) + "|" + dr.GetValue(2) + "|" + dr.GetValue(3) + "\n";
                var jsonString = JsonConvert.SerializeObject(Maquina);
                var postToPowerBi = HttpPostAsync(powerBiPostUrl, "[" + jsonString + "]");
            }
            Console.Write(output);
            dr.Close();
            cmd.Dispose();
            conn.Close();
            return output;

        }

        static async Task<HttpResponseMessage> HttpPostAsync(string url, string data)
        {
            HttpContent content = new StringContent(data);
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return response;
        }

    }
}