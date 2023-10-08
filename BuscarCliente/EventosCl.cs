using System;
using System.Collections.Generic;
using System.Text;

namespace BuscarCliente
{
    public class EventosCl
    {
        public string Key { get; set; }
        // Agrega propiedades para mapear los datos dentro del nodo "Historial"
        public string Evento { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Registro { get; set; }
        public string Trabajador { get; set; }
        public string TspH { get; set; }
    }
}
