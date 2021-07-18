using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Doppelgangsters.Pages;

namespace Doppelgangsters
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new LoginPage();
        }
    }
}
