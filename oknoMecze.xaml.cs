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
    /// Interaction logic for oknoMecze.xaml
    /// </summary>
    public partial class oknoMecze : Window
    {
        string dbConnectionString = @"Data Source=database.sqlite";
        private SQLiteDataAdapter sqlDataAdapter = null;
        private DataSet sqlDataSet = null;
        private DataTable sqlDataTable = null;

        private void InitBinding()
        {
            SQLiteConnection sqlCon = new SQLiteConnection(dbConnectionString);
            SQLiteCommand sqlCommand = sqlCon.CreateCommand();
            sqlCommand.CommandText = "SELECT * FROM Mecze";
            sqlDataAdapter = new SQLiteDataAdapter(sqlCommand.CommandText, sqlCon);
            SQLiteCommandBuilder sqliteCommandBuilder = new SQLiteCommandBuilder(sqlDataAdapter);
            sqlDataSet = new DataSet();
            sqlDataAdapter.Fill(sqlDataSet);
            sqlDataTable = sqlDataSet.Tables[0];
            listaMeczy.DataContext = sqlDataTable.DefaultView;

            //sortowanie
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaMeczy.DataContext);
            view.SortDescriptions.Add(new SortDescription("Data", ListSortDirection.Descending));

        }
        public oknoMecze()
        {
            InitializeComponent();
            InitBinding();
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            oknoEdytujMecze dialog = new oknoEdytujMecze();
            dialog.ShowDialog();

            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
            {
                DataRow sqlDataRow = sqlDataTable.NewRow();
                sqlDataRow[1] = dialog.NazwaGosp;
                sqlDataRow[2] = dialog.NazwaGosc;
                sqlDataRow[3] = dialog.GoleGosp;
                sqlDataRow[4] = dialog.GoleGosc;
                sqlDataRow[5] = dialog.Data;
                sqlDataRow[6] = dialog.Turniej;
                
                sqlDataTable.Rows.Add(sqlDataRow);
                sqlDataAdapter.Update(sqlDataSet);
                InitBinding();
            }

        }

        private void btnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (listaMeczy.SelectedItems.Count != 1) // Jeśli użytkownik nie zaznaczył nic lub zaznaczył więcej niż jedną pozycję
            {
                MessageBox.Show("Zaznacz wiersz, który chcesz edytować.", "Informacja");
                return;
            }

            oknoEdytujMecze dialog = new oknoEdytujMecze();
            dialog.ShowDialog();

            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
            {
                DataRow sqlDataRow = ((DataRowView)listaMeczy.SelectedItem).Row;
                sqlDataRow[1] = dialog.NazwaGosp;
                sqlDataRow[2] = dialog.NazwaGosc;
                sqlDataRow[3] = dialog.GoleGosp;
                sqlDataRow[4] = dialog.GoleGosc;
                sqlDataRow[5] = dialog.Data;
                sqlDataRow[6] = dialog.Turniej;

                sqlDataTable.Rows.Add(sqlDataRow);
                sqlDataAdapter.Update(sqlDataSet);
                InitBinding();
            }
        }

        private void btnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (0 == listaMeczy.SelectedItems.Count)
            {
                return;
            }
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Czy na pewno chcesz usunąć rekord?", "Potwierdzenie usunięcia", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DataRow sqlDataRow = ((DataRowView)listaMeczy.SelectedItem).Row;
                sqlDataRow.Delete();
                sqlDataAdapter.Update(sqlDataSet);
            }
            return;
        }
    }
}
