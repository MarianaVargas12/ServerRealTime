using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeServer.Models
{
    class Maquinas
    {
        public DateTime Fecha { get; set; }
        public string Tecnico { get; set; }
        public string Maquina { get; set; }
        public string Herramienta { get; set; }
        public float Costo { get; set; }
        public string Aux3 { get; set; }
        public string Job { get; set; }
        public int Cantidad { get; set; }
        
    }
}
