using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BuscarCliente
{
    public class Funciones
    {
        private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1); // 1 indica el número máximo de hilos simultáneos


        FirebaseClient firebase = new FirebaseClient(Setting.FireBaseDatabaseUrl, new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => Task.FromResult(Setting.FireBaseSecret)
        });


        public async void AddOrUpdateEmployee(InfoCliente employeeModel)
        {
            if (!string.IsNullOrWhiteSpace(employeeModel.Key))
            {
                try
                {
                    await firebase.Child(nameof(InfoCliente)).Child(employeeModel.Key).PutAsync(employeeModel);

                }
                catch (Exception ex)
                {

                }
            }
            else
            {
                var response = await firebase.Child(nameof(InfoCliente)).PostAsync(employeeModel);
                if (response.Key != null)
                {

                }
                else
                {

                }
            }

        }
        public string berta { set; get; }

        public async Task<bool> AgregarEvento(string histo, string histTsp, string fecha, string hora, int reg, string trabajador)
        {
            //Boton AgregarEvento




            try
            {
                // Obtén los valores de las entradas


                // Crea un objeto para almacenar los datos
                var datos = new
                {
                    Evento = histo,
                    Fecha = fecha,
                    Hora = hora,
                    Registro = reg,
                    Trabajador = trabajador
                };

                // Inicializa FirebaseClient con tu configuración
                FirebaseClient firebase = new FirebaseClient(Setting.FireBaseDatabaseUrl, new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(Setting.FireBaseSecret)
                });

                // Guarda los datos en la base de datos Firebase
                // Obtén una referencia al nodo "01"
                string numeroIdentificacionCliente = histTsp; // Reemplaza con el TSP del cliente que deseas buscar

                // Obtén una referencia al nodo "01"
                var nodo01 = firebase.Child("01");

                // Realiza una consulta para encontrar al cliente con el TSP proporcionado
                //var clientes = await nodo01.OnceAsync<Dictionary<string, string>>();
                var clientes = await nodo01.OnceAsync<Cliente>();

                if (clientes != null)
                {
                    foreach (var cliente in clientes)
                    {
                        if (cliente.Object != null && cliente.Object.TSP == numeroIdentificacionCliente)
                        {
                            // El cliente con el TSP proporcionado se encontró en la base de datos
                            // Ahora, actualiza el campo "Evento" dentro del nodo "Historial"

                            await nodo01
                                .Child(cliente.Key) // Usamos la clave del cliente encontrado
                                .Child("Historial")
                                .PostAsync(datos);

                            return true; // Terminamos el ciclo ya que hemos encontrado y actualizado al cliente
                        }
                    }
                }

                return true;




            }
            catch (Exception ex)
            {
                return false;
            }



        }

        int beta = 0;
        int alpha = 0;
        public async Task<bool> OntenerClientesHistorias()
        {
            await semaphore.WaitAsync();
            try
            {
                beta++;
                Globales.registrosTSP = new Dictionary<int, string>();

                //Se inicializa la lista de clientes e historial
                Globales.ListaClientesHistoriales = new List<Cliente>();
                try
                {


                    Globales.ListaClientesHistoriales.Clear();
                }
                catch
                {

                }
                //inicializo la lista de los registros
                Globales.Registros = new List<int>();
                //Limpio la lista de clientes e historial para evitar que haya algun repetido
                if (Globales.ListaClientesHistoriales.Count > 0)
                {
                    Globales.ListaClientesHistoriales.Clear();
                }

                // Inicializa FirebaseClient con tu configuración
                FirebaseClient firebase = new FirebaseClient(Setting.FireBaseDatabaseUrl, new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(Setting.FireBaseSecret)
                });

                //obtener toda la informacino de la base de datos y agregarla al objeto registroJson
                var ClientesHistorias = await firebase.Child("01").OnceAsJsonAsync();
                //obtener la informacion del numero de registro actual
                string ah = "sd";
                var Registro = await firebase.Child("02").Child("Registros").OnceAsJsonAsync();
                string sas = "sd";



                if (ClientesHistorias == null)
                {


                    return false;
                }

                else
                {
                    if (Registro == null)
                    {
                        return false;
                    }

                    string asdiajs = "so";

                    try
                    {
                        JObject parsedClientesHistorias = JObject.Parse(ClientesHistorias);
                        // Si llegamos aquí, entonces parsedClientesHistorias contiene un objeto JSON válido

                        // Ahora puedes acceder a las propiedades del objeto JSON según sea necesario.
                        // Por ejemplo:
                        // var valor = parsedClientesHistorias["algunaPropiedad"];

                        // Continúa con el procesamiento de parsedClientesHistorias si es necesario.

                        // ...

                    }
                    catch (JsonReaderException)
                    {
                        Console.WriteLine("El JSON de ClientesHistorias no es válido");
                        return false;
                    }
                    string berere = "sd";
                    //obtener todos los clientes y agregarlos al objeto customers
                    JObject customers = JObject.Parse(ClientesHistorias);
                    JObject regis = JObject.Parse(Registro);

                    //agrego el numero de registro actual a la variable 'el registro'
                    int registroValue = (int)regis["Registro"];
                    Globales.ElRegistro = registroValue;

                    //si customer no esta vacio entonces
                    if (customers != null)
                    {
                        //obtener cada cliente y agregarlo a customer
                        foreach (var customer in customers)
                        {
                            //Declaro al cliente y su historia
                            Cliente client = new Cliente();


                            //obtener la informacion del cliente
                            string customerId = customer.Key;
                            string customerNombre = (string)customer.Value["Nombre"];
                            string customerDomicilio = (string)customer.Value["Domicilio"];
                            string customerTsp = (string)customer.Value["TSP"];
                            string customerTelefono = (string)customer.Value["Telefono"];

                            //hacer agregar la informacion del cliente a la instancia de la clase cliente
                            client.Key = customerId;
                            client.Nombre = customerNombre;
                            client.Domicilio = customerDomicilio;
                            client.TSP = customerTsp;
                            client.Telefono = customerTelefono;


                            //obtener todo el historial de eventos del cliente y agregarlo al JObect customerHistorial
                            JObject customerHistorial = (JObject)customer.Value["Historial"];


                            //si customerHistorial no esta vacio entonces...
                            if (customerHistorial != null)
                            {
                                foreach (var evento in customerHistorial)
                                {
                                    //Declaro clase de eeventoHistorico
                                    EventoHistorico eventoHistorico = new EventoHistorico();
                                    string ne = client.Nombre;
                                    string Descripcion = (string)evento.Value["Evento"];
                                    string Fecha = (string)evento.Value["Fecha"];
                                    string Hora = (string)evento.Value["Hora"];
                                    int rege = (int)evento.Value["Registro"];
                                    string Trabajador = (string)evento.Value["Trabajador"];

                                    eventoHistorico.Descripcion = Descripcion;
                                    eventoHistorico.Fecha = Fecha;
                                    eventoHistorico.Hora = Hora;
                                    eventoHistorico.Registro = rege;
                                    eventoHistorico.Trabajador = Trabajador;

                                    string be = "as";
                                    //agregar un par ordenado de Registro, Tsp

                                    // Verifica si la clave ya existe en el diccionario
                                    if (Globales.registrosTSP.ContainsKey((int)evento.Value["Registro"]))
                                    {
                                        // Actualiza el valor asociado a la clave existente
                                        Globales.registrosTSP[(int)evento.Value["Registro"]] = client.TSP;
                                    }
                                    else
                                    {
                                        // Agrega un nuevo elemento al diccionario si la clave no existe
                                        Globales.registrosTSP.Add(rege, client.TSP);
                                    }

                                    //La instancia de evento historico se agrega a la lista de clientes.HistorialEventos
                                    client.HistorialEventos.Add(eventoHistorico);
                                    Globales.Registros.Add(eventoHistorico.Registro);


                                }


                            }

                            //una vez que se han recorrido todos los clientes
                            //y se han guardado sus eventosHistoricos, se procede a agregar cada cliente 
                            //a la lista de clientes e historiales
                            alpha++;
                            Globales.ListaClientesHistoriales.Add(client);
                            string charger = "d";


                        }
                    }

                    Globales.BDActualizada = true;
                    return true;
                }




            }
            catch (Exception ex)
            {

                throw ex;
                //return false;


            }
            finally
            {
                semaphore.Release();
            }
        }

        public async Task<bool> AgregarRegistro(string histTsp, int reg)
        {
            //Boton AgregarEvento




            try
            {
                // Obtén los valores de las entradas


                // Crea un objeto para almacenar los datos
                var datos = new
                {
                    Registro = reg,
                    TSP = histTsp
                };

                // Inicializa FirebaseClient con tu configuración
                FirebaseClient firebase = new FirebaseClient(Setting.FireBaseDatabaseUrl, new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(Setting.FireBaseSecret)
                });

                // Guarda los datos en la base de datos Firebase
                // Obtén una referencia al nodo "01"
                string numeroIdentificacionCliente = histTsp; // Reemplaza con el TSP del cliente que deseas buscar

                // Obtén una referencia al nodo "01"
                var nodo01 = firebase.Child("02").Child("Registros");

                // Realiza una consulta para encontrar al cliente con el TSP proporcionado


                // El cliente con el TSP proporcionado se encontró en la base de datos
                // Ahora, actualiza el campo "Evento" dentro del nodo "Historial"

                await nodo01.PutAsync(datos);

                return true; // Terminamos el ciclo ya que hemos encontrado y actualizado al cliente






            }
            catch (Exception ex)
            {
                return false;
            }



        }


    }
}
