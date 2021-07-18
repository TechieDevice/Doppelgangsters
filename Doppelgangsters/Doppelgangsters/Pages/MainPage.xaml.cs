using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
//using Doppelgangsters.ViewModels;

namespace Doppelgangsters.Pages
{
    public partial class MainPage : ContentPage
    {
        private MobileClient client;
        public MainPage(MobileClient mobileClient)
        {
            InitializeComponent();
            client = mobileClient;
        }

        private async void ConnectToRoomButtonClick(object sender, System.EventArgs e)
        {
            if (RoomCodeBox.Text == null || RoomCodeBox.Text == "")
            {
                ErrorLabel.Text = "Введите 4-хзначный код комнаты";
                ErrorLabel.IsVisible = true;
                RoomCodeBox.Focus();
                return;
            }

            await client.RoomConnect(RoomCodeBox.Text);
        }

        private async void CreateRoomButtonClick(object sender, System.EventArgs e)
        {
            await client.RoomCreate();
        }

        private async void DisconnectButtonClick(object sender, System.EventArgs e)
        {
            client.Disconnection();
            await Navigation.PopModalAsync();
        }
    }
}
