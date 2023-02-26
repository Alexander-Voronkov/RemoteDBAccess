using Models;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for UpdateAuthorModal.xaml
    /// </summary>
    public partial class UpdateAuthorModal : Window
    {
        readonly Models.Author RealAuthor;
        readonly Author Temporary = new Author();
        public UpdateAuthorModal(Models.Author auth)
        {
            InitializeComponent();
            RealAuthor = auth;
            Temporary = AuthorCopy(auth);
            name.Text = RealAuthor.Name;
            auth.Books?.ToList().ForEach(x => books.Items.Add(x.Title));
        }

        private Author AuthorCopy(Author auth)
        {
            return new Author() { Id = auth.Id, Name = auth.Name, Books = auth.Books.Select(x => new Book() { Title = x.Title }).ToList() };
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Temporary.Name.Length > 0)
            {
                RealAuthor.Id = Temporary.Id;
                RealAuthor.Books = books.Items.Cast<string>().Select(x => new Book() { Title = x }).ToList();
                RealAuthor.Name = Temporary.Name;
                this.Close();
            }
            else
                MessageBox.Show("Name is a must field!");
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            Temporary.Name = name.Text;
        }

        private void addbook_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tempbook.Text) && !books.Items.Contains(tempbook.Text))
            {
                books.Items.Add(tempbook.Text);
            }
        }

        private void delbook_Click(object sender, RoutedEventArgs e)
        {
            if (books.SelectedItem != null)
            {
                books.Items.Remove(books.SelectedItem);
            }
        }
    }
}
