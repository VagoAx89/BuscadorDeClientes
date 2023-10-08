using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins;
using Rg.Plugins.Popup;
using Acr.UserDialogs;

namespace BuscarCliente
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            //MainPage = new MainPage();
            MainPage = new NavigationPage(new MainPage());
            
            //MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
