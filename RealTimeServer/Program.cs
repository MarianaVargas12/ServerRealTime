using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeServer
{
    public class Program
    {
        public static string getKey()
        {
            //return "Server = 10.25.34.138\\SQLEXPRESS;Initial Catalog=sps;Persist Security Info = False;User ID=sa;Password=sps12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30";
            return "Data Source=10.25.34.138\\SQLEXPRESS;Initial Catalog=sps;User ID=sa;Password=sps12345";
        }
        static void Main(string[] args)
        {
            /*
            string ne = Controller.MachineController.datos();
            Console.WriteLine(ne);
            */
            
            DateTime now = DateTime.Now;
            DateTime last = DateTime.Now;
            string ne = Controller.MachineController.minuteDate();
            Console.WriteLine(ne);
            while (true)
            {
                TimeSpan ts = now - last;
                if (ts.TotalSeconds>= 60) 
                {
                    ne = Controller.MachineController.minuteDate();
                    Console.WriteLine(ne);
                    last = DateTime.Now;
                }

                now = DateTime.Now;
                Console.WriteLine(ts.TotalSeconds);
            }
        }
    }
}
