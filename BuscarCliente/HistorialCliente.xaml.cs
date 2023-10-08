using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BuscarCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistorialCliente : ContentPage
    {
        public HistorialCliente()
        {
            InitializeComponent();
            interfas();
            histo();

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (Globales.BDActualizada)
            {
                scrollStack.Children.Clear();
                mainStack.Children.Clear();
                interfas();
                histo();

            }
            else
            {
                Funciones funciones = new Funciones();
                bool esp = await funciones.OntenerClientesHistorias();
                if (!esp)
                {
                    await DisplayAlert("Error", "Ha ocurrido un error al actualizar datos", "Ok");
                }
                scrollStack.Children.Clear();
                mainStack.Children.Clear();
                interfas();
                histo();
                
            }
          
           
        }
    
        private void interfas()
        {
           

                string Anombre = Globales.NombrePasa;
                string Adomicilio = Globales.DomicilioPasa;
                string Atelefono = Globales.TelefonoPasa;
                string Atsp = Globales.StpPasa;


            // Verificar que ninguno de los strings sea nulo o vacío
            if (!string.IsNullOrEmpty(Anombre) && !string.IsNullOrEmpty(Adomicilio) && !string.IsNullOrEmpty(Atelefono) && !string.IsNullOrEmpty(Atsp))
            {
                // Llamar al método CargarCliente solo si todos los strings son válidos
                CargarCliente(Anombre, Adomicilio, Atelefono, Atsp);
            }
            else
            {
                // Manejar la situación en la que al menos uno de los strings es nulo o vacío
                Console.WriteLine("Uno o más de los strings son nulos o vacíos. No se llama a CargarCliente.");
            }

        }

        public void histo()
        {
            foreach (var cliente in Globales.ListaClientesHistoriales)
            {
                // Accede a los datos de cada objeto Cliente
                if (cliente.TSP == Globales.StpPasa)
                {

                    var eventosOrdenados = cliente.HistorialEventos.OrderByDescending(e => e.Registro).ToList();

                    // Iterar sobre la lista ordenada
                    foreach (var rec in eventosOrdenados)
                    {
                        string hora = rec.Hora;
                        string fecha = rec.Fecha;
                        int registro = rec.Registro;
                        string trabajador = rec.Trabajador;
                        string evento = rec.Descripcion;

                        // Realizar las operaciones que necesitas con los datos ordenados
                        CargarHistorial(evento, fecha, hora, trabajador);
                    }

                }
            }
        }
        public void CargarHistorial(string sEvento, string sFecha, string sHora, string sTrabajador)
        {
            if (sEvento is null)
            {
                sEvento = "Nulo";
            }
            if (sFecha is null)
            {
                sFecha = "Nulo";
            }
            if (sHora is null)
            {
                sHora = "Nulo";
            }
            if (sTrabajador is null)
            {
                sTrabajador = "Nulo";
            }

            Frame frame = new Frame();
            StackLayout frameLayout = new StackLayout();
            StackLayout horiLayout = new StackLayout();
            horiLayout.Orientation = StackOrientation.Horizontal;
            StackLayout horiLayout2 = new StackLayout();
            horiLayout2.Orientation = StackOrientation.Horizontal;

            frame.Margin = new Thickness(0, 0, 0, 00);
           


           


            Label labelFechaHora = new Label
            {
                Text = "Fecha - Hora:",
                FontSize = 16,
                FontAttributes = FontAttributes.Bold

            };

            Label labelShowFechaHora = new Label
            {
                Text = sFecha + " - " + sHora,
                FontSize = 16,
                TextColor = Color.Gray

            };

            


            Label labelPor = new Label
            {
                Text = "Evento generado por:",
                FontSize = 16,

            };

            Label labelShowTrabajador = new Label
            {
                Text = sTrabajador,
                FontSize = 16,
                TextColor = Color.Gray

            };

           

            Label labelShowEvento = new Label
            {
                Text = sEvento,
                FontSize = 14,
                TextColor = Color.Black,
              

            };

            
            horiLayout.Children.Add(labelFechaHora);
            horiLayout.Children.Add(labelShowFechaHora);
            //aqui agrego el boton de editar a cada cliente


            frameLayout.Children.Add(horiLayout);


            horiLayout2.Children.Add(labelPor);
            horiLayout2.Children.Add(labelShowTrabajador);
            frameLayout.Children.Add(horiLayout2);

            
            frameLayout.Children.Add(labelShowEvento);


            frame.Content = frameLayout;
            scrollStack.Children.Add(frame);
        }
        int a;
        public void CargarCliente(string sNombre, string sDomicilio, string sTelefono, string sTsp)
        {
            a++;

            Frame frame = new Frame();

            //var tapGestureRecognizerFr = new TapGestureRecognizer();
            //tapGestureRecognizerFr.Tapped += async (s, e) =>
            //{
            //    await Navigation.PushAsync(new HistorialCliente());
            //};
            //frame.GestureRecognizers.Add(tapGestureRecognizerFr);
            StackLayout frameLayout = new StackLayout();
            StackLayout horiLayout = new StackLayout();
            horiLayout.Orientation = StackOrientation.Horizontal;

            StackLayout horiLayout2 = new StackLayout();
            horiLayout2.Orientation = StackOrientation.Horizontal;

            StackLayout horiLayout3 = new StackLayout();
            horiLayout3.Orientation = StackOrientation.Horizontal;

            StackLayout horiLayout4 = new StackLayout();
            horiLayout4.Orientation = StackOrientation.Horizontal;

            frame.CornerRadius = 0;
            frame.Margin = new Thickness(0, 0, 0, 00);
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

            Button addHistoria = new Button
            {
                Text = "Agregar Evento",
                CornerRadius = 15

            };

            addHistoria.Clicked += OnAddHistoriaClicked;
            addHistoria.HorizontalOptions = LayoutOptions.EndAndExpand;
            // Método que se ejecutará cuando se haga clic en el botón


            //Button botonBuscar = new Button
            //{
            //    ImageSource = "edit2.png",

            //    WidthRequest = 30, // Establecer el ancho deseado en píxeles
            //    HeightRequest = 30, // Establecer la altura deseada en píxeles

            //};
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


            //horiLayout2.Children.Add(labeldireccion);
            horiLayout2.Children.Add(labelshowdireccion);

            frameLayout.Children.Add(horiLayout2);

            horiLayout3.Children.Add(labelTelefono);
            horiLayout3.Children.Add(labelShowTelefono);

            frameLayout.Children.Add(horiLayout3);

            horiLayout4.Children.Add(labelTSP);
            horiLayout4.Children.Add(labelShowTSP);
            horiLayout4.Children.Add(addHistoria);

            frameLayout.Children.Add(horiLayout4);
            // frameLayout.Children.Add(botonBuscar);


            frame.Content = frameLayout;
            mainStack.Children.Add(frame);

            //Content = mainStack;
        }

        private async void OnAddHistoriaClicked(object sender, EventArgs e)
        {
            //Boton de agregar evento
            
            await Navigation.PushModalAsync(new AgregarEvento());
            
        }

        private void AbrirGoogleMapsConDireccion(string direccion)
        {
            // Construir la URL para abrir Google Maps con la dirección
            string url = $"https://www.google.com/maps/search/?api=1&query={Uri.EscapeDataString(direccion)}";

            // Abrir la URL en el navegador
            Device.OpenUri(new Uri(url));
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
        private void OnBuscarClicked(object sender, EventArgs e)
        {
            abrirsubmenu();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
           
        }
    }
}