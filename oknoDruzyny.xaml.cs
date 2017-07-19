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

//sortowanie
using System.ComponentModel;


namespace FootballMetrics
{
    /// <summary>
    /// Interaction logic for oknoDruzyny.xaml
    /// </summary>
    public partial class oknoDruzyny : Window
    {
        string dbConnectionString = @"Data Source=database.sqlite";
        private SQLiteDataAdapter sqlDataAdapter = null;
        private DataSet sqlDataSet = null;
        private DataTable sqlDataTable = null;

        private void InitBinding()
        {
            SQLiteConnection sqlCon = new SQLiteConnection(dbConnectionString);
            SQLiteCommand sqlCommand = sqlCon.CreateCommand();
            sqlCommand.CommandText = "SELECT Nazwa, Kontynent FROM Druzyny";
            sqlDataAdapter = new SQLiteDataAdapter(sqlCommand.CommandText, sqlCon);
            SQLiteCommandBuilder sqliteCommandBuilder = new SQLiteCommandBuilder(sqlDataAdapter);
            sqlDataSet = new DataSet();            
            sqlDataAdapter.Fill(sqlDataSet); 
            sqlDataTable = sqlDataSet.Tables[0];
            listaDruzyn.DataContext = sqlDataTable.DefaultView;

            //sortowanie
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(listaDruzyn.DataContext);
            view.SortDescriptions.Add(new SortDescription("Nazwa", ListSortDirection.Ascending));            
        }

        public oknoDruzyny()
        {            
            InitializeComponent();
            InitBinding();
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            oknoEdytujDruzyny dialog = new oknoEdytujDruzyny();
            dialog.ShowDialog();

            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
            {
                DataRow sqlDataRow = sqlDataTable.NewRow();
                sqlDataRow[0] = dialog.NazwaDruzyny;
                sqlDataRow[1] = dialog.Kontynent;
                sqlDataTable.Rows.Add(sqlDataRow);
                sqlDataAdapter.Update(sqlDataSet);
                InitBinding();
            }
                 
        }

        private void btnEdytuj_Click(object sender, RoutedEventArgs e)
        {
            if (listaDruzyn.SelectedItems.Count != 1) // Jeśli użytkownik nie zaznaczył nic lub zaznaczył więcej niż jedną pozycję
            {
                MessageBox.Show("Zaznacz wiersz, który chcesz edytować.", "Informacja");
                return;
            }
            oknoEdytujDruzyny dialog = new oknoEdytujDruzyny();
            dialog.ShowDialog();
            if (dialog.DialogResult.HasValue && dialog.DialogResult.Value)
            {
                DataRow sqlDataRow = ((DataRowView)listaDruzyn.SelectedItem).Row;
                sqlDataRow.BeginEdit();
                sqlDataRow[0] = dialog.NazwaDruzyny;
                sqlDataRow[1] = dialog.Kontynent;
                sqlDataRow.EndEdit();
                sqlDataAdapter.Update(sqlDataSet);
                InitBinding();
            }
                   
        }

        private void btnUsun_Click(object sender, RoutedEventArgs e)
        {
            if (0 == listaDruzyn.SelectedItems.Count)
            {
                return;
            }
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Czy na pewno chcesz usunąć rekord?", "Potwierdzenie usunięcia", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                DataRow sqlDataRow = ((DataRowView)listaDruzyn.SelectedItem).Row;
                sqlDataRow.Delete();
                sqlDataAdapter.Update(sqlDataSet);
            }return;
        }
    }
}
