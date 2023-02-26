using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for InsertAuthorModal.xaml
    /// </summary>
    public partial class InsertAuthorModal : Window
    {
        Models.Author Author;
        bool forced = true;
        public InsertAuthorModal(Models.Author auth)
        {
            InitializeComponent();
            Author = auth;
            this.Closed += Closed_;
        }

        private void Closed_(object sender, EventArgs cea)
        {
            if(forced)
                Author.Name = string.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Author.Name.Length > 0)
            {
                forced = false;
                foreach (var item in books.Items)
                {
                    Author.Books.Add(new Models.Book() { Title = item.ToString() });
                }
                this.Close();
            }
            else
                MessageBox.Show("Name is a must field!");
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            Author.Name = name.Text;
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
