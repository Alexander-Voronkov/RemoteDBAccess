using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BookContext = Context.Context;

namespace _RemoteDbAccess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Mutex mutex;
        private readonly BookContext context;
        public MainWindow()
        {
            InitializeComponent();
            if(Mutex.TryOpenExisting("DBServerMutex", out Mutex m) == true)
            {
                MessageBox.Show("You can't use admin DB access application while the server is running!");
                throw new Exception("You can't use admin DB access application while the server is running!");
            }
            mutex = new Mutex(false,"DBAdminMutex");
            this.Closing += Close;
            this.Insert_Btn.Click += Insert;
            this.Delete_Btn.Click += Delete;
            context = new BookContext();
            context.Authors.Load();
            authortable.DataContext = context.Authors.Local;
            context.Books.Load();
            booktable.DataContext = context.Books.Local;
        }

        private async void Delete(object sender, EventArgs ea)
        {
            switch(DBTabControl.SelectedIndex)
            {
                case 0:
                    if (booktable.SelectedIndex == -1)
                    {
                        MessageBox.Show("Choose a book!");
                        return;
                    };
                    context.Books.Remove(booktable.SelectedItem as Book);
                    await context.SaveChangesAsync();
                    break;
                case 1:
                    if (authortable.SelectedIndex == -1)
                    {
                        MessageBox.Show("Choose an author!");
                        return;
                    };
                    context.Authors.Remove(authortable.SelectedItem as Author); 
                    await context.SaveChangesAsync(); 
                    break;
            }
        }

        private void Close(object sender, CancelEventArgs cea)
        {
            context.Dispose();
        }

        private async void Insert(object sender, EventArgs cea)
        {
            switch (DBTabControl.SelectedIndex)
            {
                case 0:
                    var book = new Book() { Authors = new List<Author>() }; 
                    new InsertModal(book).ShowDialog();
                    if (book.Title != string.Empty && book.Description != string.Empty)
                    {
                        if (book.Authors.Count > 0)
                        {
                            var auths = book.Authors.Select(x => x.Name);
                            var temp = from a in context.Authors where auths.Contains(a.Name) select a;
                            book.Authors = await temp.ToListAsync();
                        }
                        context.Books.Add(book);
                        try
                        {
                            await context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            context.Books.Load();
                        }
                    }
                    break;
                case 1:
                    var auth = new Author() { Books = new List<Book>() };
                    new InsertAuthorModal(auth).ShowDialog();
                    if(auth.Name != string.Empty)
                    {
                        if (auth.Books.Count > 0)
                        {
                            var books = auth.Books.Select(x => x.Title);
                            var temp = from a in context.Books where books.Contains(a.Title) select a;
                            auth.Books = await temp.ToListAsync();
                        }
                        context.Authors.Add(auth);
                        try
                        {
                            await context.SaveChangesAsync();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            context.Authors.Load();
                        }
                    }
                    break;
            }
        }

        private async void Update_Btn_Click(object sender, RoutedEventArgs e)
        {
            switch (DBTabControl.SelectedIndex)
            {
                case 0:
                    if (booktable.SelectedItem != null)
                    {
                        var copy = BookCopy(booktable.SelectedItem as Book);
                        new UpdateBookModal(copy).ShowDialog();
                        context.Books.AddOrUpdate(copy);
                        await context.SaveChangesAsync();
                        context.Books.Load();
                        booktable.ItemsSource = null;
                        booktable.ItemsSource = context.Books.Local;
                    }
                    else
                        MessageBox.Show("Select a book!");
                    break;
                case 1:
                    if (authortable.SelectedItem != null)
                    {
                        var copy = AuthorCopy(authortable.SelectedItem as Author);
                        new UpdateAuthorModal(copy).ShowDialog();
                        context.Authors.AddOrUpdate(copy);
                        await context.SaveChangesAsync();
                        context.Authors.Load();
                        authortable.ItemsSource = null;
                        authortable.ItemsSource = context.Authors.Local;
                    }
                    else
                        MessageBox.Show("Select an author!");
                    break;
            }   
        }

        private Author AuthorCopy(Author auth)
        {
            return new Author() { Id = auth.Id, Name = auth.Name, Books = auth.Books.Select(x => new Book() { Title = x.Title }).ToList() };
        }

        private Book BookCopy(Book auth)
        {
            var temp = new Book() { Id = auth.Id, Title = auth.Title,Description = auth.Description, Authors = auth.Authors.Select(x => new Author() { Name = x.Name }).ToList() };
            if(auth.Content!=null)
            {
                temp.Content = new byte[auth.Content.Length];
                Array.Copy(auth.Content, temp.Content, auth.Content.Length);
            }

            if (auth.Cover != null)
            {
                temp.Cover = new byte[auth.Cover.Length];
                Array.Copy(auth.Cover, temp.Cover, auth.Cover.Length);
            }
            return temp;
        }

    }
    public class ImageConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || (value as byte[]).Length == 0)
                return null;
            var arr = new byte[(value as byte[]).Length];
            Array.Copy(value as byte[],arr,(value as byte[]).Length);
            var ms = new MemoryStream(arr)
            {
                Position = 0
            };
            BitmapImage Image = new BitmapImage();
            Image.BeginInit();
            Image.StreamSource = ms;
            Image.EndInit();
            return Image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return null;
            var source = value as BitmapImage;
            byte[] buff = new byte[source.StreamSource.Length];
            source.StreamSource.Read(buff, 0, (int)source.StreamSource.Length);
            return buff;
        }
    }
}
