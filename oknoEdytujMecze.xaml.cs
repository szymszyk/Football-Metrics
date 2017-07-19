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
using System.Data;
using System.Data.SQLite;
using System.ComponentModel;

namespace FootballMetrics
{
    /// <summary>
    /// Interaction logic for oknoEdytujMecze.xaml
    /// </summary>
    public partial class oknoEdytujMecze : Window
    {
        private static string nazwaGosp, nazwaGosc, turniej;
        private static int goleGosp, goleGosc;
        private static DateTime data;

        public string NazwaGosp { get { return nazwaGosp; } }
        public string NazwaGosc { get { return nazwaGosc; } }
        public string Turniej { get { return turniej; } }
        public int GoleGosp { get { return goleGosp; } }
        public int GoleGosc { get { return goleGosc; } }
        public DateTime Data { get { return data; } }

        string dbConnectionString = @"Data Source=database.sqlite";
        private SQLiteDataAdapter sqlDataAdapter = null;
        private DataSet sqlDataSet = null;
        private DataTable sqlDataTable = null;

        public oknoEdytujMecze()
        {
            InitializeComponent();
            UzupelnijComboBox();
            cb_Gosp.ItemsSource = UzupelnijComboBox();
            cb_Goscie.ItemsSource = cb_Gosp.ItemsSource;
        }

        
        private List<string> UzupelnijComboBox()
        {
            SQLiteConnection sqlCon = new SQLiteConnection(dbConnectionString);
            SQLiteCommand sqlCommand = sqlCon.CreateCommand();
            sqlCommand.CommandText = "SELECT Nazwa FROM Druzyny ORDER BY Nazwa asc";
            sqlDataAdapter = new SQLiteDataAdapter(sqlCommand.CommandText, sqlCon);
            SQLiteCommandBuilder sqliteCommandBuilder = new SQLiteCommandBuilder(sqlDataAdapter);
            sqlDataSet = new DataSet();
            sqlDataAdapter.Fill(sqlDataSet);
            sqlDataTable = sqlDataSet.Tables[0];
            List<string> listaDruzyn = new List<string>();

            foreach (DataRowView item in sqlDataTable.DefaultView)
            {
                listaDruzyn.Add(item["Nazwa"].ToString());
            }

            return listaDruzyn;
            //sortowanie
            //CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaDruzyn.DataContext);
            //view.SortDescriptions.Add(new SortDescription("Nazwa", ListSortDirection.Ascending));

        }
        
        private void btnZatwierdz_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(cb_Gosp.Text)) //Sprawdzenie czy nasz textBox czasem nie jest pusty.
            {
                MessageBox.Show("Wybierz drużynę gospodarzy!", "Informacja");
                return;
            }
            else if (string.IsNullOrEmpty(txtBox_goleGosp.Text)) 
            {
                MessageBox.Show("Wpisz liczbę bramek strzelonych przez gospodarzy!", "Informacja");
                return;
            }
            else if (string.IsNullOrEmpty(cb_Goscie.Text))
            { 
                MessageBox.Show("Wybierz drużynę gości!", "Informacja");
                return;
            }
            else if (string.IsNullOrEmpty(txtBox_goleGosci.Text))
            {
                MessageBox.Show("Wpisz liczbę bramek strzelonych przez gości!", "Informacja");
                return;
            }
            else if (string.IsNullOrEmpty(txtBox_turniej.Text))
            {
                MessageBox.Show("Nazwa turnieju nie może być pusta!", "Informacja");
                return;
            }
            else if (!kalendarz.SelectedDate.HasValue)
            {
                MessageBox.Show("Wybierz dzień, w którym był rozegrany mecz.", "Informacja");
                return;
            }
            else if (cb_Gosp.Text == cb_Goscie.Text)
            {
                MessageBox.Show("Błędnie wybrane drużyny.", "Informacja");
                return;
            }

            nazwaGosp = cb_Gosp.Text;
            nazwaGosc = cb_Goscie.Text;
            goleGosp = int.Parse(txtBox_goleGosp.Text);
            goleGosc = int.Parse(txtBox_goleGosci.Text);
            turniej = txtBox_turniej.Text;

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Czy wszystkie podane dane są na pewno poprawne?", "Potwierdzenie", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.DialogResult = true;
                this.Close();
            }
            return;            

        }

        private void btnAnuluj_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void kalendarz_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (kalendarz.SelectedDate.HasValue)
            {
                data = kalendarz.SelectedDate.Value;
            }
        }
    }
}
