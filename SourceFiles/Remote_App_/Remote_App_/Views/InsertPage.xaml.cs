using MessagePacket;
using ModelsDTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using TCP_Client;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Remote_App_.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InsertPage : ContentPage
    {
        private readonly IPEndPoint point;
        private BookDTO tempBook;
        private AuthorDTO tempAuthor;
        public InsertPage()
        {
            InitializeComponent();
            point = new IPEndPoint((IPAddress)Application.Current.Properties["ServerIP"], (int)Application.Current.Properties["ServerPort"]);
            this.InsertAuthor.Clicked += (o, ea) => InsertAuthorClick();
            this.InsertBook.Clicked += (o, ea) => InsertBookClick();
            this.BookCoverBtn.Clicked += (o, ea) => PickCover();
            this.BookContentBtn.Clicked += (o, ea) => PickContent();
            this.BookTitle.TextChanged += (o, ea) => BookTitleChanged();
            this.AuthorName.TextChanged += (o, ea) => AuthorNameChanged();
            this.BookDescription.TextChanged += (o, ea) => BookDescriptionChanged();
            this.AddAuthorBtn.Clicked += (o, ea) => AddAuthorBtnClick();
            this.AddBookBtn.Clicked += (o, ea) => AddBookBtnClick();
            this.DeleteAuthorBtn.Clicked += (o, ea) => DeleteTempAuthor();
            this.DeleteBookBtn.Clicked += (o, ea) => DeleteTempBook();
            this.DbType.Toggled += (o, ea) =>
            {
                if(ea.Value)
                {
                    this.AuthorInsert.IsVisible= true;
                    this.AuthorInsert.IsEnabled = true;
                    this.BookInsert.IsVisible = false;
                    this.BookInsert.IsEnabled = false;
                }
                else
                {
                    this.AuthorInsert.IsVisible = false;
                    this.AuthorInsert.IsEnabled = false;
                    this.BookInsert.IsVisible = true;
                    this.BookInsert.IsEnabled = true;
                }
            };
        }

        private void DeleteTempBook()
        {
            if (books.SelectedItem == null)
                return;
            this.tempAuthor.Books.Remove(books.SelectedItem as BookDTO);
            this.books.ItemsSource = null;
            this.books.ItemsSource = this.tempAuthor.Books;
        }

        private void DeleteTempAuthor()
        {
            if (authors.SelectedItem == null)
                return;
            this.tempBook.Authors.Remove(authors.SelectedItem as AuthorDTO);
            this.authors.ItemsSource = null;
            this.authors.ItemsSource = this.tempBook.Authors;
        }

        private void AddBookBtnClick()
        {
            if (string.IsNullOrWhiteSpace(AuthorTempBook.Text))
                return;
            if (this.tempAuthor != null)
                if(this.tempAuthor.Books != null) 
                    if (this.tempAuthor.Books.Where(x => x.Title == AuthorTempBook.Text).Count() != 0)
                            return;

            if (tempAuthor == null)
                tempAuthor = new AuthorDTO();
            if(tempAuthor.Books==null)
                tempAuthor.Books = new List<BookDTO>();
            tempAuthor.Books.Add(new BookDTO() { Title = AuthorTempBook.Text });
            this.books.ItemsSource = null;
            this.books.ItemsSource = tempAuthor.Books;
        }

        private void AddAuthorBtnClick()
        {
            if (string.IsNullOrWhiteSpace(BookTempAuthor.Text))
                return;

            if (this.tempBook != null)
                if (this.tempBook.Authors != null)
                    if (this.tempBook.Authors.Where(x => x.Name == BookTempAuthor.Text).Count() != 0)
                        return;

            if (tempBook == null)
                tempBook = new BookDTO();
            if(tempBook.Authors == null)
                tempBook.Authors = new List<AuthorDTO>();
            tempBook.Authors.Add(new AuthorDTO() { Name = BookTempAuthor.Text });
            this.authors.ItemsSource = null;
            this.authors.ItemsSource = tempBook.Authors;
        }

        private void AuthorNameChanged()
        {
            if (tempAuthor == null)
                tempAuthor = new AuthorDTO();
            tempAuthor.Name = this.AuthorName.Text;
        }

        private void BookTitleChanged()
        {
            if(tempBook == null)
                tempBook = new BookDTO();
            tempBook.Title = this.BookTitle.Text;
        }

        private void BookDescriptionChanged()
        {
            if(tempBook == null)
                tempBook = new BookDTO();
            tempBook.Description = this.BookDescription.Text;
        }

        private async void PickContent()
        {
            var perm = await Permissions.RequestAsync<Permissions.StorageRead>();
            if (perm != PermissionStatus.Granted)
                return;
            if (tempBook == null)
                tempBook = new BookDTO();
            var res = await FilePicker.PickAsync(new PickOptions()
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new []{"application/pdf","text/plain"} }
                }),
                PickerTitle = "Choose a cover for the book:"
            });

            if (res == null)
                return;

            var stream = await res.OpenReadAsync();
            byte[] buff = new byte[stream.Length];
            await stream.ReadAsync(buff, 0, buff.Length);
            tempBook.Content = buff;
        }

        private async void PickCover()
        {
            var perm = await Permissions.RequestAsync<Permissions.StorageRead>();
            if (perm != PermissionStatus.Granted)
                return;
            if (tempBook == null)
                tempBook = new BookDTO();
            var res = await FilePicker.PickAsync(new PickOptions()
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new []{"image/jpeg","image/png"} }
                }),
                PickerTitle = "Choose a cover for the book:"
            });

            if (res == null)
                return;

            var stream = await res.OpenReadAsync();
            byte[] buff = new byte[stream.Length];
            await stream.ReadAsync(buff, 0, buff.Length);
            tempBook.Cover = buff;
        }

        private async void InsertBookClick()
        {
            if(string.IsNullOrWhiteSpace(BookTitle.Text)||string.IsNullOrWhiteSpace(BookDescription.Text))
            {
                ShowMessage("You must fill the title and description fields in!");
                await Task.Delay(2000);
                return;
            }
            try
            {
                using (Client client = new Client(point))
                {
                    var packet = new MessageQueryPacket() { DBType = DBType.Books, Type = MessageType.InsertUpdate};
                    using (var ms = new MemoryStream())
                    {
                        var list = new List<BookDTO> { tempBook };
                        new BinaryFormatter().Serialize(ms, list);
                        packet.Content = ms.ToArray();
                    }
                    var response = await client.SendAndReceiveAsync(packet);
                    if(response.Type == MessageType.Error)
                    {
                        ShowMessage(Encoding.UTF8.GetString(response.Content));
                        await Task.Delay(2000);
                        ClearFields();
                        return;
                    }
                    ShowMessage(Enum.GetName(typeof(MessageType),response.Type));
                    await Task.Delay(2000);
                    ClearFields();
                }
            }
            catch(Exception ex)
            {
                ShowMessage(ex.Message);
                await Task.Delay(2000);
                ClearFields();
            }
        }

        private async void InsertAuthorClick()
        {
            if (string.IsNullOrWhiteSpace(AuthorName.Text))
            {
                ShowMessage("You must fill the name field in!");
                await Task.Delay(2000);
                return;
            }
            try
            {
                using (Client client = new Client(point))
                {
                    var packet = new MessageQueryPacket() { DBType = DBType.Authors, Type = MessageType.InsertUpdate };
                    using (var ms = new MemoryStream())
                    {
                        var list = new List<AuthorDTO> { tempAuthor };
                        new BinaryFormatter().Serialize(ms, list); 
                        packet.Content = ms.ToArray();
                    }
                    var response = await client.SendAndReceiveAsync(packet);
                    if (response.Type == MessageType.Error)
                    {
                        ShowMessage(Encoding.UTF8.GetString(response.Content));
                        await Task.Delay(2000);
                        ClearFields();
                        return;
                    }
                    ShowMessage(Enum.GetName(typeof(MessageType), response.Type));
                    await Task.Delay(2000);
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
                await Task.Delay(2000);
                ClearFields();
            }
        }

        private async void ShowMessage(string message)
        {
            await DisplayAlert("Message", message, "Okay");
        }

        private void ClearFields()
        {
            this.tempAuthor = null;
            this.tempBook = null;
            this.BookTitle.Text = string.Empty;
            this.BookDescription.Text = string.Empty;
            this.authors.ItemsSource = null;
            this.books.ItemsSource = null;
            this.AuthorName.Text = string.Empty;
        }
    }
}