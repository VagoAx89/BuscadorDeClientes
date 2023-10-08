using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace BuscarCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddClient : ContentPage
    {
        public AddClient()
        {
            InitializeComponent();
        }


        private async void volve()
        {
            await Navigation.PopModalAsync();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            volve();
        }
        private async void guarda()
        {
            //Agregar cliente
            string a = NombreEntry.Text;
            string b = DireccionEntry.Text;
            string c = TelefonoEntry.Text;
            string d = TspEntry.Text;
            if (string.IsNullOrWhiteSpace(a) ||
                string.IsNullOrWhiteSpace(b) ||
                string.IsNullOrWhiteSpace(c) ||
                string.IsNullOrWhiteSpace(d))
            {
                // Mostrar un mensaje de error o realizar alguna acción en caso de campos vacíos
                await DisplayAlert("Error", "Por favor, completa todos los campos.", "Aceptar");
                return; // Detener la ejecución si hay campos vacíos
            }
            Globales.BDActualizada = false;
          
            AgregarCliente(a, b, c, d);
            await this.DisplayToastAsync("Datos guardados correctamente", 2000);
            volve();


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
            //botton para guardar al cliente en la base de datos
            guarda();
        }
    }
}