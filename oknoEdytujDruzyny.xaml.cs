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

namespace FootballMetrics
{
    /// <summary>
    /// Interaction logic for oknoEdytujDruzyny.xaml
    /// </summary>
    public partial class oknoEdytujDruzyny : Window
    {
        private static string nazwaDruzyny, kontynent;
        

        public string NazwaDruzyny { get { return nazwaDruzyny; } }
        public string Kontynent { get { return kontynent; } }

        public oknoEdytujDruzyny()
        {
            InitializeComponent();
        }

        private void btnAnuluj_Click(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }

        private void btnZatwierdz_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBox_NazwaDruzyny.Text)) //Sprawdzenie czy nasz textBox czasem nie jest pusty.
            {
                MessageBox.Show("Nazwa drużyny nie może być pusta!", "Informacja");
                return;
            }
            else if (string.IsNullOrEmpty(txtBox_Kontynent.Text)) //Sprawdzenie czy nasz textBox czasem nie jest pusty.
            {
                MessageBox.Show("Nazwa kontynentu nie może być pusta!", "Informacja");
                return;
            }

            nazwaDruzyny = txtBox_NazwaDruzyny.Text;
            kontynent = txtBox_Kontynent.Text;

            
            this.DialogResult = true;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
