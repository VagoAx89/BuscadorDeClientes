using Acr.UserDialogs;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.CommunityToolkit.Extensions;
namespace BuscarCliente
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgregarEvento : ContentPage
    {
        public AgregarEvento()
        {
            InitializeComponent();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            //agregar evento

            Funciones funciones = new Funciones();

            string fecha = DateTime.Now.ToString("MM/dd/yyyy");
            string hora = DateTime.Now.ToString("HH:mm:ss");
            string Rsegundos = DateTime.Now.ToString("ss");
            string Rminutos = DateTime.Now.ToString("mm");
            string Rhoras = DateTime.Now.ToString("HH");
            string Rdias = DateTime.Now.ToString("dd");
            string Rmes = DateTime.Now.ToString("MM");
            string Rano = DateTime.Now.ToString("yyyy");

            string trabajador = "Carlos";
            string evento = editor1.Text;
            string tsp = MainPage.TspAhistorial;
            // string registro = Rano + Rmes+ Rdias+ Rhoras+ Rminutos+ Rsegundos;
            int registro;
            registro = Globales.ElRegistro;
          
            bool respuesta = await funciones.AgregarEvento(evento, tsp, fecha, hora, registro +1, trabajador);
            if (respuesta)
            {
                //se ha agregado con exito el evento
                Globales.actualizado = true;
                Globales.actualizadoMain = true;
                Globales.BDActualizada = false;

                await this.DisplayToastAsync("Datos guardados correctamente", 1500);
              
                //string usuario = await this.DisplayPromptAsync("Inicio de sesión", "Ingrese su nombre de usuario:", "Aceptar", "Cancelar", "Nombre de usuario");

                //if (usuario != null)
                //{
                //    string contraseña = await this.DisplayPromptAsync("Inicio de sesión", "Ingrese su contraseña:", "Aceptar", "Cancelar", "Contraseña");

                //    if (contraseña != null)
                //    {
                //            await this.DisplayAlert("Inicio de sesión exitoso", "Bienvenido, " + usuario + "!", "Aceptar");
                      
                //    }
                //    else
                //    {
                //        await this.DisplayAlert("Cancelado", "Inicio de sesión cancelado.", "Aceptar");
                //    }
                //}
                //else
                //{
                //    await this.DisplayAlert("Cancelado", "Inicio de sesión cancelado.", "Aceptar");
                //}






                //bool obtain = await funciones.OntenerClientesHistorias();
                //if (!obtain)
                //{
                //    await DisplayAlert("Error", "Ocurrio un error al actualizar los datos", "Ok");
                //}
            }
            else
            {
               await DisplayAlert("Error", "Ha ocurrido un error al intentar guardar este evento", "Ok");
            }

            bool respuesta2 = await funciones.AgregarRegistro(tsp, registro +1);
            if (respuesta)
            {
                //await DisplayAlert("Logrado", "Se ha guardado exitosamente el registro", "Ok");
            }
            else
            {
                await DisplayAlert("Error", "Ha ocurrido un error al intentar registrar este evento", "Ok");
            }

        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            //boton bolver
         Navigation.PopModalAsync();
        }
    }
}