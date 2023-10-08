using System.Collections.Generic;
using System;
namespace BuscarCliente
{

    public class EventoHistorico
    {
        public string Descripcion { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public int Registro { get; set; }
        public string Trabajador { get; set; }
    }
    public class Cliente
    {
        public string Domicilio { get; set; }
        public string Nombre { get; set; }
        public string TSP { get; set; }
        public string Telefono { get; set; }
        public string Key { get; set; }
        public List<EventoHistorico> HistorialEventos { get; set; }

        public Historial Historial { get; set; }


        public Cliente()
        {
            HistorialEventos = new List<EventoHistorico>();
        }
    }

    public class Historial
    {
        public string Key { get; set; }
        // Agrega propiedades para mapear los datos dentro del nodo "Historial"
        public string Evento { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public string Registro { get; set; }
        public string Trabajador { get; set; }

        // public string Evento2 { get; set; }
        // Otras propiedades necesarias
    }
}
