using Microsoft.Win32;
using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _RemoteDbAccess
{
    /// <summary>
    /// Interaction logic for UpdateBookModal.xaml
    /// </summary>
    public partial class UpdateBookModal : Window
    {
        private readonly Book RealBook;
        private readonly Book Temporary;
        public UpdateBookModal(Book book)
        {
            InitializeComponent();
            RealBook = book;
            Temporary = BookCopy(book);
            title.Text = book.Title;
            description.Text = book.Description;
            book.Authors?.ToList().ForEach(x => authors.Items.Add(x.Name));
        }

        private Book BookCopy(Book auth)
        {
            var temp = new Book() { Id = auth.Id, Title = auth.Title, Description = auth.Description, Authors = auth.Authors.Select(x => new Author() { Name = x.Name }).ToList() };
            if (auth.Content != null)
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


        private void title_TextChanged(object sender, TextChangedEventArgs e)
        {
            Temporary.Title = title.Text;
        }

        private void description_TextChanged(object sender, TextChangedEventArgs e)
        {
            Temporary.Description = description.Text;
        }

        private void coverchoose_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "Images(*.png,*.jpg,*.bmp)|*.png;*.jpg;*.bmp";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                var br = new BinaryReader(ofd.OpenFile());
                Temporary.Cover = br.ReadBytes((int)br.BaseStream.Length);
            }
        }

        private void contentchoose_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "Text files(*.txt,*.pdf)|*.txt;*.pdf;";
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == true)
            {
                var br = new BinaryReader(ofd.OpenFile());
                Temporary.Content = br.ReadBytes((int)br.BaseStream.Length);
            }
        }

        private void addauthor_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tempauthor.Text) && !authors.Items.Contains(tempauthor.Text))
            {
                authors.Items.Add(tempauthor.Text);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Temporary.Title.Length > 0 && Temporary.Description.Length > 0)
            {
                RealBook.Id = Temporary.Id;
                RealBook.Title = Temporary.Title;
                RealBook.Description = Temporary.Description;
                RealBook.Authors = authors.Items.Cast<string>().Select(x => new Author() { Name = x }).ToList();
                if (Temporary.Cover != null)
                {
                    RealBook.Cover = new byte[Temporary.Cover.Length];
                    Array.Copy(Temporary.Cover, RealBook.Cover, Temporary.Cover.Length);
                }
                if (Temporary.Content != null)
                {
                    RealBook.Content = new byte[Temporary.Content.Length];
                    Array.Copy(Temporary.Content, RealBook.Content, Temporary.Content.Length);
                }
                this.Close();
            }
            else
                MessageBox.Show("Title and description are must fields!");
        }

        private void delauthor_Click(object sender, RoutedEventArgs e)
        {
            if (authors.SelectedItem != null)
            {
                authors.Items.Remove(authors.SelectedItem);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
