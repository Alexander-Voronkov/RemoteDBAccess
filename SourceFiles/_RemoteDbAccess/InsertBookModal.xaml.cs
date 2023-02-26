using Microsoft.Win32;
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
    /// Interaction logic for InsertModal.xaml
    /// </summary>
    public partial class InsertModal : Window
    {
        Models.Book Book;
        bool forced = true;
        public InsertModal(Models.Book book)
        {
            InitializeComponent();
            Book = book;
            this.Closed += Closed_;
        }

        private void Closed_(object sender, EventArgs cea)
        {
            if (forced)
            {
                Book.Title = string.Empty;
                Book.Description = string.Empty;
            }
        }

        private void addauthor_Click(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(tempauthor.Text)&&!authors.Items.Contains(tempauthor.Text))
            {
                authors.Items.Add(tempauthor.Text);
            }
        }

        private void delauthor_Click(object sender, RoutedEventArgs e)
        {
            if (authors.SelectedItem != null)
            {
                authors.Items.Remove(authors.SelectedItem);
            }
        }

        private void coverchoose_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.Filter = "Images(*.png,*.jpg,*.bmp)|*.png;*.jpg;*.bmp";
            ofd.Multiselect = false;
            if(ofd.ShowDialog() == true)
            {
                var br = new BinaryReader(ofd.OpenFile());
                Book.Cover = br.ReadBytes((int)br.BaseStream.Length);
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
                Book.Content = br.ReadBytes((int)br.BaseStream.Length);
            }
        }

        private void description_TextChanged(object sender, TextChangedEventArgs e)
        {
            Book.Description = description.Text;
        }

        private void title_TextChanged(object sender, TextChangedEventArgs e)
        {
            Book.Title = title.Text;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Book.Title.Length > 0 && Book.Description.Length > 0)
            {
                forced = false;
                foreach (var item in authors.Items)
                {
                    Book.Authors.Add(new Models.Author() { Name = item.ToString() });
                }
                this.Close();
            }
            else
                MessageBox.Show("Title and description are must fields!");
        }
    }
}
