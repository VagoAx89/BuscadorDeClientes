using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BuscarCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ClientesForm : ContentPage
    {

        

        
        public ClientesForm()
        {
            InitializeComponent();

            //inicio();




            checkName.IsChecked = true;

            checkName.CheckedChanged += OnCheckBoxCheckedChanged;
            checkTsp.CheckedChanged += OnCheckBoxCheckedChanged;
            checkDirection.CheckedChanged += OnCheckBoxCheckedChanged;

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

           

            if (Globales.BDActualizada)
            {
                //si la base de datos esta actualizada entonces cargar datos a la interface
                mainStack.Children.Clear();
               
                interfas();
              
            }
            else
            {
                //de lo contrario entonces actualizar base de datos y luego cargar datos a la interface
               mainStack.Children.Clear();

                Funciones funciones = new Funciones();

               bool obten = await funciones.OntenerClientesHistorias();
                interfas();
             
            }
        }
        private void OnFabButtonClicked(object sender, EventArgs e)
        {
            var agregarClientePopup = new AddClient();
            Navigation.PushModalAsync(agregarClientePopup);
        }

        int a = 0;

     
        private void interfas()
        {
            var listaOrdenada = Globales.ListaClientesHistoriales.OrderBy(cliente => cliente.Nombre).ToList();

            foreach (var cliente in listaOrdenada)
            {
                // Accede a los datos de cada objeto Cliente
                string Anombre = cliente.Nombre;
                string Adomicilio = cliente.Domicilio;
                string Atelefono = cliente.Telefono;
                string Atsp = cliente.TSP;
                //  string ev = cliente.Historial.Evento;
                CrearInterfazDinamica(Anombre, Adomicilio, Atelefono, Atsp);
                //  DisplayAlert("prueba", ev, "ok");
            }
        }


        public void CrearInterfazDinamica(string sNombre, string sDomicilio, string sTelefono, string sTsp)
        {
            a++;

            Frame frame = new Frame();

            var tapGestureRecognizerFr = new TapGestureRecognizer();
            tapGestureRecognizerFr.Tapped += async (s, e) =>
            {
                Globales.NombrePasa = sNombre;
                Globales.DomicilioPasa = sDomicilio;
                Globales.TelefonoPasa = sTelefono;
                Globales.StpPasa = sTsp;
                //Abre el historial del cliente
                await Navigation.PushAsync(new HistorialCliente());

                MainPage.TspAhistorial = sTsp;            };
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
                Text = "Direccion:",
                FontSize = 16,

            };

            Label labelshowdireccion = new Label
            {
                Text = sDomicilio,
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
                Text = "Telefono:",
                FontSize = 16,

            };

            Label labelShowTelefono = new Label
            {
                Text = sTelefono,
                FontSize = 16,
                TextColor = Color.Gray

            };

            Label labelTSP = new Label
            {
                Text = "TSP:",
                FontSize = 14,

            };

            Label labelShowTSP = new Label
            {
                Text = sTsp,
                FontSize = 14,
                TextColor = Color.Black
            };

            //Button botonBuscar = new Button
            //{
            //    ImageSource = "edit2.png",

            //    WidthRequest = 30, // Establecer el ancho deseado en píxeles
            //    HeightRequest = 30, // Establecer la altura deseada en píxeles

            //};
            ImageButton botonBuscar = new ImageButton
            {
                WidthRequest = 40,
                HeightRequest = 40,
                BackgroundColor = Color.White,
                Aspect = Aspect.AspectFit,
                Source = "edit2.png",
                Scale = .6

            };
            botonBuscar.HorizontalOptions = LayoutOptions.EndAndExpand;
            botonBuscar.AutomationId = "Boton" + a.ToString();
            botonBuscar.Clicked += OnBuscarClicked;

            // horiLayout.Children.Add(labelNombre);
            horiLayout.Children.Add(labelShowNombre);

            //aqui agrego el boton de editar a cada cliente
            horiLayout.Children.Add(botonBuscar);

            frameLayout.Children.Add(horiLayout);


            //horiLayout2.Children.Add(labeldireccion);
            horiLayout2.Children.Add(labelshowdireccion);

            frameLayout.Children.Add(horiLayout2);

            horiLayout3.Children.Add(labelTelefono);
            horiLayout3.Children.Add(labelShowTelefono);

            frameLayout.Children.Add(horiLayout3);

            horiLayout4.Children.Add(labelTSP);
            horiLayout4.Children.Add(labelShowTSP);







            frameLayout.Children.Add(horiLayout4);
            // frameLayout.Children.Add(botonBuscar);


            frame.Content = frameLayout;
            mainStack.Children.Add(frame);

            //Content = mainStack;
        }
        string automationId;

        private async void abrirsubmenu()
        {
            var action = await DisplayActionSheet("Título del Menú", "Cancelar", null, "Fotografias", "Documentos", "Editar","Eliminar");

            // Aquí puedes manejar la opción seleccionada por el usuario
            if (action == "Opción 1")
            {
                // Realiza la acción correspondiente a la Opción 1
            }
            else if (action == "Opción 2")
            {
                // Realiza la acción correspondiente a la Opción 2
            }
            if (action == "Opción 3")
            {
                // Realiza la acción correspondiente a la Opción 1
            }
            else if (action == "Opción 4")
            {
                // Realiza la acción correspondiente a la Opción 2
            }
            else if (action == "Cancelar")
            {
                // El usuario seleccionó "Cancelar" o tocó fuera del menú emergente
            }
        }
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

        
        public async void AgregarCliente(string nombre, string domicilio, string telefono, string tsp)
        {
            //Boton Agregar




            try
            {
                // Obtén los valores de las entradas


                // Crea un objeto para almacenar los datos
                var datos = new
                {
                    Nombre = nombre,
                    Domicilio = domicilio,
                    Telefono = telefono,
                    TSP = tsp
                };

                // Inicializa FirebaseClient con tu configuración
                FirebaseClient firebase = new FirebaseClient(Setting.FireBaseDatabaseUrl, new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(Setting.FireBaseSecret)
                });

                // Guarda los datos en la base de datos Firebase
                var resultado = await firebase.Child("01").PostAsync(datos);

                if (resultado.Object != null)
                {
                    // Éxito: Los datos se guardaron correctamente
                    await DisplayAlert("Éxito", "Los datos se guardaron correctamente en Firebase.", "Aceptar");
                }
                else
                {
                    // Error: No se pudieron guardar los datos
                    await DisplayAlert("Error", "No se pudieron guardar los datos en Firebase.", "Aceptar");
                }
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                await DisplayAlert("Error", "Ocurrió un error: " + ex.Message, "Aceptar");
            }



        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            //    string a = NombreEntry.Text;
            //    string b = DireccionEntry.Text;
            //    string c = TelefonoEntry.Text;
            //    string d = TspEntry.Text;
            //    AgregarCliente(a, b, c, d);
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

        private async void Button_Clicked_4(object sender, EventArgs e)
        {
            //Boton de agregar un cliente nuevo

            var agregarClientePopup = new AddClient();
            Navigation.PushModalAsync(agregarClientePopup);
        }
    }
}