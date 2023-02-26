using MessagePacket;
using ModelsDTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TCP_Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote_App_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowAll : ContentPage
    {
        private readonly IPEndPoint point;
        public ShowAll()
        {
            InitializeComponent();
            point = new IPEndPoint((IPAddress)Application.Current.Properties["ServerIP"], (int)Application.Current.Properties["ServerPort"]);
            this.DbType.Toggled += (o, a) => ShowAllBooksAndAuthors();
            this.DeleteBtn.Clicked += DeleteClick;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ShowAllBooksAndAuthors();
        }

        private async void ShowMessage(string message)
        {
            await DisplayAlert("Message", message, "Okay");
        }

        private async void ShowAllBooksAndAuthors()
        {
            try
            {
                using (var client = new Client(point))
                {
                    var response = await client.SendAndReceiveAsync(new MessageQueryPacket()
                    {
                        Type = MessageType.Select,
                        DBType = DbType.IsToggled ? DBType.Authors : DBType.Books
                    });
                    if (response.Type == MessageType.Error)
                    {
                        ShowMessage(Encoding.UTF8.GetString(response.Content));
                        return;
                    }
                    if (response.DBType == DBType.Books)
                    {
                        booklist.ItemsSource = (List<BookDTO>)new BinaryFormatter().Deserialize(new MemoryStream(response.Content));
                        authorlist.IsVisible = false;
                        authorlist.IsEnabled = false;
                        booklist.IsVisible = true;
                        booklist.IsEnabled = true;
                        BookScroll.IsVisible = true;
                        BookScroll.IsEnabled = true;
                        AuthorScroll.IsVisible = false;
                        AuthorScroll.IsEnabled = false;

                    }
                    else
                    {
                        authorlist.ItemsSource = (List<AuthorDTO>)new BinaryFormatter().Deserialize(new MemoryStream(response.Content));
                        booklist.IsVisible = false;
                        booklist.IsEnabled = false;
                        BookScroll.IsEnabled = false;
                        BookScroll.IsVisible = false;
                        authorlist.IsVisible = true;
                        AuthorScroll.IsVisible = true;
                        AuthorScroll.IsEnabled = true;
                        authorlist.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"An error occured: {ex.Message}");
                await Task.Delay(3000);
                Process.GetCurrentProcess().Kill();
            }
        }

        private async void DeleteClick(object sender, EventArgs ea)
        {
            if (DbType.IsToggled && authorlist.SelectedItem == null)
            {
                ShowMessage("Choose one author to delete!");
                await Task.Delay(2000);
                return;
            }
            else if (!DbType.IsToggled && booklist.SelectedItem == null)
            {
                ShowMessage("Choose one book to delete!");
                await Task.Delay(2000);
                return;
            }
            try
            {
                using (var client = new Client(point))
                {
                    if (DbType.IsToggled)
                    {
                        var mqp = new MessageQueryPacket() { DBType = DBType.Authors, Type = MessageType.Delete };
                        var list = new List<AuthorDTO> { authorlist.SelectedItem as AuthorDTO };
                        using (var ms = new MemoryStream())
                        {
                            new BinaryFormatter().Serialize(ms, list);
                            mqp.Content = ms.ToArray();
                        }
                        var response = await client.SendAndReceiveAsync(mqp);
                        ShowMessage(Enum.GetName(typeof(MessageType), response.Type));
                        await Task.Delay(2000);
                    }
                    else
                    {
                        var mqp = new MessageQueryPacket() { DBType = DBType.Books, Type = MessageType.Delete };
                        var list = new List<BookDTO> { booklist.SelectedItem as BookDTO };
                        using (var ms = new MemoryStream())
                        {
                            new BinaryFormatter().Serialize(ms, list);
                            mqp.Content = ms.ToArray();
                        }
                        var response = await client.SendAndReceiveAsync(mqp);
                        ShowMessage(Enum.GetName(typeof(MessageType), response.Type));
                        await Task.Delay(2000);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"An error occured: {ex.Message}");
                await Task.Delay(3000);
                Process.GetCurrentProcess().Kill();
            }
            ShowAllBooksAndAuthors();
        }
    }
}