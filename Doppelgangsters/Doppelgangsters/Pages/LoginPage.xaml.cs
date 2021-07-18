using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Doppelgangsters.Pages
{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private Thread ConnectonThread;
        private MobileClient client;
        private bool connected = false;
        public LoginPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CheckConnection();
        }

        private async void ConnectButtonClick(object sender, System.EventArgs e)
        {
            if (UserNameBox.Text == null || UserNameBox.Text == "")
            {
                ErrorLabel.Text = "Введите имя";
                ErrorLabel.IsVisible = true;
                UserNameBox.Focus();
                return;
            }

            LoginInterface.IsVisible = false;
            Label ConnectionLabel = new Label() {
                Text = "Подключение...",
                FontSize = 20,
                TextColor = Color.White,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions=LayoutOptions.Center,
                Padding = 40
            };
            MainLayout.Children.Add(ConnectionLabel);

            client = new MobileClient(UserNameBox.Text);
            await ConnectToServer();

            if (connected)
            {
                await Navigation.PushModalAsync(new MainPage(client));
            }
            else
            {
                ErrorLabel.Text = "Не удается подключиться к серверу.";
                ErrorLabel.IsVisible = true;
            }

            MainLayout.Children.Remove(ConnectionLabel);
            LoginInterface.IsVisible = true;
        }

        private void CheckConnection()
        {
            var current = Connectivity.NetworkAccess;
            if (current != NetworkAccess.Internet)
            {
                ErrorLabel.Text = "Нет доступа к интернету. Проверьте подключение к сети.";
                ErrorLabel.IsVisible = true;
                ConnectButton.IsEnabled = false;
                UserNameBox.IsEnabled = false;
            }
        }

        private async Task ConnectToServer()
        {
            try
            {
                await client.Connection();
                connected = true;
            }
            catch
            {
                ErrorLabel.Text = "Не удается подключиться к серверу.";
                ErrorLabel.IsVisible = true;
            }
        }
    }
}