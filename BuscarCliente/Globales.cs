using System;
using System.Collections.Generic;
using System.Text;

namespace BuscarCliente
{
    public static class Globales
    {
        public static int ElRegistro { get; set; }

        public static string NombrePasa { get; set; }
        public static string DomicilioPasa { get; set; }
        public static string TelefonoPasa { get; set; }
        public static string StpPasa { get; set; }

        public static List<Cliente> ListaClientesHistoriales { get; set; }
        public static List<int> Registros { get; set; }
       public static Dictionary<int, string> registrosTSP { get; set; }

        public static bool actualizado = false;
        public static bool actualizadoMain = false;
        public static bool BDActualizada = false;

    }
}
