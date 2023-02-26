using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TCP_Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote_App_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectionPage : ContentPage
    {
        public ConnectionPage()
        {
            InitializeComponent();
            this.Connect_Btn.Clicked += OnClick;
        }
        private async void ShowMessage(string message)
        {
            await DisplayAlert("Message", message, "Okay");
        }

        private async void OnClick(object sender, EventArgs ea)
        {
            if(string.IsNullOrWhiteSpace(this.IP.Text)||string.IsNullOrWhiteSpace(this.Port.Text))
            {
                ShowMessage("Fill the IP and Port fields in!");
                return;
            }
            try
            {
                using (Client client = new Client(new IPEndPoint(IPAddress.Parse(this.IP.Text), int.Parse(this.Port.Text))))
                { }
            }
            catch (Exception ex)
            {
                ShowMessage($"An error occured: {ex.Message}");
                await Task.Delay(3000);
                Process.GetCurrentProcess().Kill();
                return;
            }
            Application.Current.Properties["ServerIP"] = IPAddress.Parse(this.IP.Text);
            Application.Current.Properties["ServerPort"] = int.Parse(this.Port.Text);
            Application.Current.MainPage = new AppShell();
        }
    }
}