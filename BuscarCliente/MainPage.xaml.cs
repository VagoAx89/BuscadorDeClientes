using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Button = Xamarin.Forms.Button;
using ImageButton = Xamarin.Forms.ImageButton;

namespace BuscarCliente
{
    public partial class MainPage : Xamarin.Forms.TabbedPage
    {



        // Crea una lista para almacenar los objetos Cliente
        public static List<Cliente> listaClientes = new List<Cliente>();
        public static List<EventosCl> listaEventos = new List<EventosCl>();
        //Funciones funciones = new Funciones();
        public static List<JObject> ListaHistoriales = new List<JObject>();
        public static string TspAhistorial;
        public MainPage()
        {
            InitializeComponent();
            ObtainAdd();





            NavigationPage.SetHasNavigationBar(this, false);


            checkName.IsChecked = true;

            checkName.CheckedChanged += OnCheckBoxCheckedChanged;
            checkTsp.CheckedChanged += OnCheckBoxCheckedChanged;
            checkDirection.CheckedChanged += OnCheckBoxCheckedChanged;


           // ObtainAdd();

        }

        private async void ObtainAdd()
        {
            Funciones funciones = new Funciones();
            //Se limpia el stack para quitar a todo historial habido ahi
            mainStack.Children.Clear();
            //Obtener datos
            bool obtain = false;  // Inicializamos obtain a falso

            try
            {
                // Obtener datos
                obtain = await funciones.OntenerClientesHistorias();
                // await DisplayAlert("Error", "Logrado", "Ok");

               
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Se produjo una excepción al acceder a la BD: {ex.Message}", "Ok");
            }

            if (obtain)
            {
                // Agregar datos
                interfas();
               
            }
            else
            {
             
            }



        }

        private async void actualizar()
        {
            mainStack.Children.Clear();
            interfas();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

           
            if (Globales.BDActualizada)
            {
                mainStack.Children.Clear();
                interfas();
             
            }
            else
            {
                mainStack.Children.Clear();
                interfas();
            }
        }
        int a = 0;

       
        private void interfas()
        {
            
            Globales.Registros.Sort();
            List<string> adentro = new List<string>();
            int maxAb = Globales.ElRegistro;
           

            var registrosOrdenados = Globales.registrosTSP.OrderByDescending(pair => pair.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            foreach (var kvp in registrosOrdenados)
            {
                int registro = kvp.Key;
                string tsp = kvp.Value;
                foreach (var cliente in Globales.ListaClientesHistoriales)
                {
                    // Accede a los datos de cada objeto Cliente
                    string Anombre = cliente.Nombre;
                    string Adomicilio = cliente.Domicilio;
                    string Atelefono = cliente.Telefono;
                    string Atsp = cliente.TSP;
                   
                    //si la lista 'adentro' no contiene al tsp (cliente) entonces se
                    //puede agregar a la internfaz y se agrega a la lista 'adentro' para que en la otra vuelta no se 
                    //vuelva  a agregar este mismo cliente
                    if (!adentro.Contains(Atsp))
                    {
                        //debido a que foreach cliente in listaclienteshistoriales esta iterando
                        //este if es para que se detenga una vez que el cliente sea el mismo
                        //que esta en la lista de registros ordenados
                        if (tsp == Atsp)
                        {
                            foreach (var evento in cliente.HistorialEventos)
                            {
                                if (evento.Registro == registro)
                                {
                                    string Afecha = evento.Fecha;
                                    string Aevent = evento.Descripcion;
                                    string Atrabajador = evento.Trabajador;
                                    CrearInterfazDinamica(Anombre, Adomicilio, Atelefono, Atsp, Afecha, Aevent, Atrabajador);
                                    adentro.Add(Atsp);
                                }
                            }
                        }
                    }
                  
                }
            }


           
          


        }


        public void CrearInterfazDinamica(string sNombre, string sDomicilio, string sTelefono, string sTsp, string sFecha, string sEvent, string trabajador)
        {
            a++;

            Frame frame = new Frame();

            var tapGestureRecognizerFr = new TapGestureRecognizer();
            tapGestureRecognizerFr.Tapped += async (s, e) =>
            {
                TspAhistorial = sTsp;

                Globales.NombrePasa = sNombre;
                Globales.DomicilioPasa = sDomicilio;
                Globales.TelefonoPasa = sTelefono;
                Globales.StpPasa = sTsp;

                await Navigation.PushAsync(new HistorialCliente());
            };
            frame.GestureRecognizers.Add(tapGestureRecognizerFr);
            StackLayout frameLayout = new StackLayout();
            StackLayout horiLayout = new StackLayout();
            horiLayout.Orientation = StackOrientation.Horizontal;

            StackLayout horiLayout2 = new StackLayout();
            horiLayout2.Orientation = StackOrientation.Horizontal;

            StackLayout horiLayout3 = new StackLayout();
            horiLayout3.Orientation = StackOrientation.Horizontal;

            StackLayout horiLayout4 = new StackLayout();
            horiLayout4.Orientation = StackOrientation.Horizontal;

            frame.CornerRadius = 15;
            frame.Margin = new Thickness(10, 0, 10, 00);
            Label labelNombre = new Label
            {
                Text = "Nombre:",
                FontSize = 16
            };


            Label labelShowNombre = new Label
            {


                Text = sNombre,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                TextColor = Color.Accent,
                HorizontalOptions = LayoutOptions.Start
            };


            Label labeldireccion = new Label
            {
                Text = "TSP:",
                FontSize = 16,

            };

            Label labelshowdireccion = new Label
            {
                Text = sTsp,
                FontSize = 16,
                TextColor = Color.Gray

            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (s, e) =>
            {
                // Copiar el texto al portapapeles
                var labelText = ((Label)s).Text;

                Clipboard.SetTextAsync(labelText);

                AbrirGoogleMapsConDireccion(labelText);
                //DisplayAlert("Copiado al portapapeles", labelText, "OK");
            };

            labelshowdireccion.GestureRecognizers.Add(tapGestureRecognizer);


            Label labelTelefono = new Label
            {
                Text = "Fecha:",
                FontSize = 16,

            };

            Label labelShowTelefono = new Label
            {
                Text = sFecha,
                FontSize = 16,
                TextColor = Color.Gray

            };

            Label labelTSP = new Label
            {
                Text = "Evento:",
                FontSize = 14,

            };

            Label labelShowTSP = new Label
            {
                Text = trabajador + ": " + sEvent,
                FontSize = 14,
                TextColor = Color.Black,
                //maximo 3 renglones
                MaxLines = 3,
                //agrega ... al final
                LineBreakMode = LineBreakMode.TailTruncation,

            };

            ImageButton botonBuscar = new ImageButton
            {
                WidthRequest = 20,
                HeightRequest = 20,
                BackgroundColor = Color.White,
                Aspect = Aspect.AspectFit,
                Source = "edit2.png"

            };
            botonBuscar.HorizontalOptions = LayoutOptions.EndAndExpand;
            botonBuscar.AutomationId = "Boton" + a.ToString();
            botonBuscar.Clicked += OnBuscarClicked;

            // horiLayout.Children.Add(labelNombre);
            horiLayout.Children.Add(labelShowNombre);

            //aqui agrego el boton de editar a cada cliente
            horiLayout.Children.Add(botonBuscar);

            frameLayout.Children.Add(horiLayout);

            //este es el TSP
            horiLayout2.Children.Add(labeldireccion);
            horiLayout2.Children.Add(labelshowdireccion);

            frameLayout.Children.Add(horiLayout2);

            //en realidad es la fecha
            horiLayout3.Children.Add(labelTelefono);
            //en realidad es la fecha
            horiLayout3.Children.Add(labelShowTelefono);
            //agrega la fecha
            frameLayout.Children.Add(horiLayout3);

            //en realidad es el evento
            //frameLayout.Children.Add(labelTSP);
            frameLayout.Children.Add(labelShowTSP);







            frameLayout.Children.Add(horiLayout4);
            // frameLayout.Children.Add(botonBuscar);


            frame.Content = frameLayout;
            mainStack.Children.Add(frame);

            //Content = mainStack;
        }
        string automationId;
        private void OnBuscarClicked(object sender, EventArgs e)
        {
            abrirsubmenu();
            if (sender is Button boton)
            {
                // Obtener el número del botón a partir del AutomationId

                automationId = boton.AutomationId;
                //  int numeroBoton = int.Parse(automationId); // "BotonX" -> X

                // Mostrar el mensaje con el número del botón
                // DisplayAlert("Botón Presionado", automationId, "Aceptar");
                

            }

        }
        private async void abrirsubmenu()
        {
            var action = await DisplayActionSheet("Título del Menú", "Cancelar", null, "Fotografias", "Documentos", "Editar", "Eliminar");

            // Aquí puedes manejar la opción seleccionada por el usuario
            if (action == "Fotografias")
            {
                // Realiza la acción correspondiente a la Opción 1
            }
            else if (action == "Documentos")
            {
                // Realiza la acción correspondiente a la Opción 2
            }
            if (action == "Editar")
            {
                // Realiza la acción correspondiente a la Opción 1
            }
            else if (action == "Eliminar")
            {
                // Realiza la acción correspondiente a la Opción 2
            }
            else if (action == "Cancelar")
            {
                // El usuario seleccionó "Cancelar" o tocó fuera del menú emergente
            }
        }

        private void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            // Desactivar los otros CheckBox cuando uno se marque
            if (sender == checkName && checkName.IsChecked)
            {
                checkTsp.IsChecked = false;
                checkDirection.IsChecked = false;
            }
            else if (sender == checkTsp && checkTsp.IsChecked)
            {
                checkName.IsChecked = false;
                checkDirection.IsChecked = false;
            }
            else if (sender == checkDirection && checkDirection.IsChecked)
            {
                checkName.IsChecked = false;
                checkTsp.IsChecked = false;
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            

        }
        private void AbrirGoogleMapsConDireccion(string direccion)
        {
            // Construir la URL para abrir Google Maps con la dirección
            string url = $"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(direccion)}";

            // Abrir la URL en el navegador
            Device.OpenUri(new Uri(url));
        }

        public static Dictionary<string, object> ObtenerPropiedadesYValores(object objeto)
        {
            Dictionary<string, object> propiedadesYValores = new Dictionary<string, object>();
            if (objeto != null)
            {
                Type tipoObjeto = objeto.GetType();
                foreach (var propiedadInfo in tipoObjeto.GetProperties())
                {
                    string nombrePropiedad = propiedadInfo.Name;
                    object valorPropiedad = propiedadInfo.GetValue(objeto, null);
                    propiedadesYValores[nombrePropiedad] = valorPropiedad;
                }
            }
            return propiedadesYValores;
        }

        private async Task ObtenerHistorias()
        {
            try
            {
                if (listaClientes.Count > 0)
                {
                    listaClientes.Clear();
                }
                // Inicializa FirebaseClient con tu configuración
                FirebaseClient firebase = new FirebaseClient(Setting.FireBaseDatabaseUrl, new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(Setting.FireBaseSecret)
                });

                //obtener toda la informacino de la base de datos y agregarla al objeto registroJson
                var registroJson = await firebase.Child("01").OnceAsJsonAsync();
                var obtenerRegistros = await firebase.Child("01").Child("Registros").OnceAsJsonAsync();
                //obtener todos los clientes y agregarlos al objeto customers
                JObject customers = JObject.Parse(registroJson);
                JObject regis = JObject.Parse(obtenerRegistros);

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
                        client.Historial = new Historial();

                        //obtener la informacion del cliente
                        string customerId = customer.Key;
                        string customerNombre = (string)customer.Value["Nombre"];
                        string customerDomicilio = (string)customer.Value["Domicilio"];
                        string customerTsp = (string)customer.Value["TSP"];
                        string customerTelefono = (string)customer.Value["Telefono"];


                        client.Key = customerId;
                        client.Nombre = customerNombre;
                        client.Domicilio = customerDomicilio;
                        client.TSP = customerTsp;
                        client.Telefono = customerTelefono;

                        //listaClientes.Add(client);
                        //obtener todo el historial de eventos del cliente y agregarlo al JObect customerHistorial
                        JObject customerHistorial = (JObject)customer.Value["Historial"];


                        //si customerHistorial no esta vacio entonces...
                        if (customerHistorial != null)
                        {


                            foreach (var reg in customerHistorial)
                            {
                                EventosCl even = new EventosCl();

                                try
                                {

                                    int otroReg = (int)reg.Value["Registro"];

                                    if (otroReg == Globales.ElRegistro)
                                    {



                                        string descripcion = (string)reg.Value["Evento"];
                                        string fecha = (string)reg.Value["Fecha"];
                                        string hora = (string)reg.Value["Hora"];
                                        int registro = (int)reg.Value["Registro"];
                                        string trabajador = (string)reg.Value["Trabajador"];
                                        string eventoId = (string)reg.Value["Key"];
                                        client.Historial.Fecha = fecha;
                                        client.Historial.Hora = hora;
                                        //client.Historial.Registro = registro;
                                        client.Historial.Evento = descripcion;
                                        client.Historial.Key = "Sin key";
                                        client.Historial.Trabajador = trabajador;
                                        listaClientes.Add(client);

                                        //even.Registro = registro;


                                        ListaHistoriales.Add(customerHistorial);

                                    }
                                    else
                                    {
                                        string descripcion = (string)reg.Value["Evento"];
                                        string fecha = (string)reg.Value["Fecha"];
                                        string hora = (string)reg.Value["Hora"];
                                        int registro = (int)reg.Value["Registro"];
                                        string trabajador = (string)reg.Value["Trabajador"];
                                        string eventoId = (string)reg.Value["Key"];

                                        even.Fecha = fecha;
                                        even.Hora = hora;
                                        even.Evento = descripcion;
                                        even.Key = eventoId;
                                        even.Trabajador = trabajador;
                                        even.TspH = customerTsp;
                                        listaEventos.Add(even);
                                    }
                                }
                                catch
                                {

                                }

                            }


                        }
                        else
                        {


                        }

                    }
                }
                else
                {
                    await DisplayAlert("Error", "No se encontro ningun cliente registrado", "Cancel");
                }

            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                await DisplayAlert("Error", "No fue posible conectarse a la base de datos", "Cancel");

            }
        }


        private void Button_Clicked_1(object sender, EventArgs e)
        {

        }

        private async void addclientscrren()
        {

            var agregarClientePopup = new AddClient();
            Navigation.PushModalAsync(agregarClientePopup);


        }
        private void Button_Clicked_2(object sender, EventArgs e)
        {
            //Show add cliente
            addclientscrren();
        }
        public async void AgregarCliente2(string histo, string fecha, string hora, string reg)
        {
            //Boton Agregar




            try
            {
                // Obtén los valores de las entradas


                // Crea un objeto para almacenar los datos
                var datos = new
                {
                    Evento = histo,
                    Fecha = fecha,
                    Hora = hora,
                    Registro = reg
                };

                // Inicializa FirebaseClient con tu configuración
                FirebaseClient firebase = new FirebaseClient(Setting.FireBaseDatabaseUrl, new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(Setting.FireBaseSecret)
                });

                // Guarda los datos en la base de datos Firebase
                // Obtén una referencia al nodo "01"
                string numeroIdentificacionCliente = "2"; // Reemplaza con el TSP del cliente que deseas buscar

                // Obtén una referencia al nodo "01"
                var nodo01 = firebase.Child("01");

                // Realiza una consulta para encontrar al cliente con el TSP proporcionado
                //var clientes = await nodo01.OnceAsync<Dictionary<string, string>>();
                var clientes = await nodo01.OnceAsync<Cliente>();

                foreach (var cliente in clientes)
                {
                    if (cliente.Object != null && cliente.Object.TSP == numeroIdentificacionCliente)
                    {
                        // El cliente con el TSP proporcionado se encontró en la base de datos
                        // Ahora, actualiza el campo "Evento" dentro del nodo "Historial"
                        await DisplayAlert("Éxito", "Se va hacer el intento", "Aceptar");
                        await nodo01
                            .Child(cliente.Key) // Usamos la clave del cliente encontrado
                            .Child("Historial")
                            .PostAsync(datos);
                        await DisplayAlert("Éxito", "Se pudo", "Aceptar");
                        // Éxito: El dato se guardó correctamente en el campo "Evento" del cliente
                        await DisplayAlert("Éxito", "El dato se guardó correctamente en Firebase.", "Aceptar");
                        return; // Terminamos el ciclo ya que hemos encontrado y actualizado al cliente
                    }
                }

                // Si llegamos aquí, significa que no se encontró un cliente con el TSP proporcionado
                await DisplayAlert("Error", "No se encontró un cliente con el TSP proporcionado.", "Aceptar");


            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                await DisplayAlert("Error", "Ocurrió un error: " + ex.Message, "Aceptar");
            }



        }
        private void Button_Clicked_3(object sender, EventArgs e)
        {
            //Agregar 2
            string historia = "Fui a ver al cliente2";
            string fecha = DateTime.Now.ToString("M/d/yyyy");
            string hora = DateTime.Now.ToString("HH:mm:ss");
            string reg = "1";
            AgregarCliente2(historia, fecha, hora, reg);
        }

        private void Button_Clicked_4(object sender, EventArgs e)
        {
            ObtainAdd();
        }
    }
}
