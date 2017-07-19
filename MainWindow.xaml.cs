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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Data.SQLite;

namespace FootballMetrics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_wyjdz_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void btn_edytujDruzyny_Click(object sender, RoutedEventArgs e)
        {
            oknoDruzyny oknoDruzyny = new oknoDruzyny();
            oknoDruzyny.Show();            
        }

        private void btn_edytujMecze_Click(object sender, RoutedEventArgs e)
        {
            oknoMecze oknoMecze = new oknoMecze();
            oknoMecze.Show();
        }

        private void btn_stworzRanking_Click(object sender, RoutedEventArgs e)
        {
            oknoRanking oknoRanking = new oknoRanking();
            oknoRanking.Show();
        }
    }
}
