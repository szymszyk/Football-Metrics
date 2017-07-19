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
    /// Logika interakcji dla klasy oknoRanking.xaml
    /// </summary>
    public partial class oknoRanking : Window
    {
        string dbConnectionString = @"Data Source=database.sqlite";
        private SQLiteDataAdapter sqlDataAdapter = null;
        private DataSet sqlDataSet = null;
        private DataTable sqlDataTable = null;       
        
        private static DateTime data;
        public DateTime Data { get { return data; } }

        //Klasa drużyny potrzebna do wygenerowania pierwszych ratingów
        public class DruzynaPR
        {
            public string nazwa { get; set; }
            private int rating = 2500;
            private double performanceRating = 2500;
            public int meczeRozegrane = 0;
            public int Rating
            {
                get { return rating; }
                set { rating = value; }
            }
            public double PerformanceRating
            {
                get { return performanceRating; }
                set { performanceRating = value; }
            }

            public double wynik { get; set; }
            public DruzynaPR(string nazwa)
            {
                this.nazwa = nazwa;
            }
        }

        private List<DruzynaPR> StworzListeDruzyn()
        {
            SQLiteConnection sqlCon = new SQLiteConnection(dbConnectionString);
            SQLiteCommand sqlCommand = sqlCon.CreateCommand();         

            sqlCommand.CommandText = "SELECT Nazwa FROM Druzyny ORDER BY Nazwa asc";
            sqlDataAdapter = new SQLiteDataAdapter(sqlCommand.CommandText, sqlCon);
            SQLiteCommandBuilder sqliteCommandBuilder = new SQLiteCommandBuilder(sqlDataAdapter);
            sqlDataSet = new DataSet();
            sqlDataAdapter.Fill(sqlDataSet);
            sqlDataTable = sqlDataSet.Tables[0];
            List<DruzynaPR> listaDruzyn = new List<DruzynaPR>();

            foreach (DataRowView item in sqlDataTable.DefaultView)
            {
                listaDruzyn.Add(new DruzynaPR(item["Nazwa"].ToString()));
            }
            sqlCon.Close();
            return listaDruzyn;
        }
       
        // wyliczamy ratingi dla wszystkich druzyn, 1 rok w przeszlości - waga meczu o 2% mniejsza
        
            // W JAKI SPOSÓB WPROWADZIĆ WAGI MECZÓW, ŻEBY WPŁYWAŁY NA RATINGI?
            
        private void btnStworzRanking_Click(object sender, RoutedEventArgs e)
        {
            if (kalendarz.SelectedDate.HasValue)
            {
                double sredniRatingPrzeciwnikow = 0;
                double sumaRatingow = 0;

                string datastr = PodajDatę(data);
                int liczbaMeczy = 0;
                List<DruzynaPR> wszystkieDruzyny = StworzListeDruzyn();
                List<DruzynaPR> listaDruzyn = new List<DruzynaPR>();
                //łączenie z tabelą mecze
                SQLiteConnection sqlCon = new SQLiteConnection(dbConnectionString);
                SQLiteCommand sqlCommand = sqlCon.CreateCommand();

                sqlCommand.CommandText = "SELECT * FROM Mecze WHERE Data < '" + datastr + "' ORDER BY Data asc";
                sqlDataAdapter = new SQLiteDataAdapter(sqlCommand.CommandText, sqlCon);
                SQLiteCommandBuilder sqliteCommandBuilder = new SQLiteCommandBuilder(sqlDataAdapter);
                sqlDataSet = new DataSet();
                sqlDataAdapter.Fill(sqlDataSet);
                sqlDataTable = sqlDataSet.Tables[0];

                liczbaMeczy = sqlDataTable.DefaultView.Count;

                //mamy mecze i druzyny sprawdzamy, kazda z druzyn zagrała jakikolwiek mecz, jesli nie to usuwamy ją z listy
                foreach (DruzynaPR druzyna in wszystkieDruzyny)
                {
                    int zwyciestwa = 0;
                    int remisy = 0;
                    for (int i = 0; i < liczbaMeczy; i++)
                    {
                        if (sqlDataSet.Tables[0].Rows[i]["Nazwa_Gosp"].ToString() == druzyna.nazwa)
                        {
                            druzyna.meczeRozegrane++;
                            if (Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosp"]) > Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosci"]))
                            {

                                zwyciestwa++;
                            }
                            else if (Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosp"]) == Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosci"]))
                            {
                                remisy++;
                            }
                        }
                        else if (sqlDataSet.Tables[0].Rows[i]["Nazwa_Gosc"].ToString() == druzyna.nazwa)
                        {
                            druzyna.meczeRozegrane++;
                            if (Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosci"]) > Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosp"]))
                            {

                                zwyciestwa++;
                            }
                            else if (Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosci"]) == Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosp"]))
                            {
                                remisy++;
                            }
                        }
                    }
                    if (druzyna.meczeRozegrane != 0)
                    {
                        druzyna.wynik = ((double)zwyciestwa + 0.5 * (double)remisy) / (double)druzyna.meczeRozegrane;
                        listaDruzyn.Add(druzyna); //tworzę nową listę druzyn zeby nie było pustych elementów na liście
                    }
                }

                ZaktualizujRatingiNaLiście(listaDruzyn);
                //na liscie są ratingi z pierwszego rankingu, teraz trzeba liczyć reszte
                foreach (DruzynaPR druzyna in listaDruzyn)
                {
                    sumaRatingow += druzyna.PerformanceRating;
                }
                foreach (DruzynaPR druzyna in listaDruzyn)
                {
                    sredniRatingPrzeciwnikow = (sumaRatingow - druzyna.PerformanceRating) / (listaDruzyn.Count - 1);
                    druzyna.PerformanceRating = sredniRatingPrzeciwnikow + ((druzyna.wynik - 0.5) * 850);
                    druzyna.Rating = Convert.ToInt32(43 + ((druzyna.PerformanceRating * druzyna.meczeRozegrane) + (sredniRatingPrzeciwnikow * 4) + (2300 * 3)) / (druzyna.meczeRozegrane + 7));
                }
                InitBinding(listaDruzyn);
                MessageBox.Show("Ranking został zaktualizowany na dzień: " + datastr);
            }
        }

        private List<DruzynaPR> StworzPierwszyRanking()
        {
            int liczbaMeczy = 0;
            List<DruzynaPR> wszystkieDruzyny = StworzListeDruzyn();
            List<DruzynaPR> listaDruzyn = new List<DruzynaPR>();
            //łączenie z tabelą mecze
            SQLiteConnection sqlCon = new SQLiteConnection(dbConnectionString);
            SQLiteCommand sqlCommand = sqlCon.CreateCommand();

            sqlCommand.CommandText = "SELECT * FROM Mecze WHERE Data < '1931-01-01' ORDER BY Data asc"; //pierwsze MŚ, które mam były w 1930 roku
            sqlDataAdapter = new SQLiteDataAdapter(sqlCommand.CommandText, sqlCon);
            SQLiteCommandBuilder sqliteCommandBuilder = new SQLiteCommandBuilder(sqlDataAdapter);
            sqlDataSet = new DataSet();
            sqlDataAdapter.Fill(sqlDataSet);
            sqlDataTable = sqlDataSet.Tables[0];
            
            liczbaMeczy = sqlDataTable.DefaultView.Count;
            
            foreach (DruzynaPR druzyna in wszystkieDruzyny)
            {
                int zwyciestwa = 0;
                int remisy = 0;                
                for (int i = 0; i < liczbaMeczy; i++)
                {
                    if (sqlDataSet.Tables[0].Rows[i]["Nazwa_Gosp"].ToString() == druzyna.nazwa)
                    {
                        druzyna.meczeRozegrane++;
                        if (Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosp"]) > Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosci"]))
                        {
                            
                            zwyciestwa++;
                        }
                        else if (Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosp"]) == Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosci"]))
                        {
                            remisy++;
                        }
                    }
                    else if (sqlDataSet.Tables[0].Rows[i]["Nazwa_Gosc"].ToString() == druzyna.nazwa)
                    {
                        druzyna.meczeRozegrane++;
                        if (Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosci"]) > Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosp"]))
                        {

                            zwyciestwa++;
                        }
                        else if (Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosci"]) == Convert.ToInt32(sqlDataSet.Tables[0].Rows[i]["Gole_Gosp"]))
                        {
                            remisy++;
                        }
                    }                    
                }
                //Console.WriteLine(" mecze rozegrane: " + meczeRozegrane + " zwyciestwa: " + zwyciestwa + " remisy: " + remisy);
                if (druzyna.meczeRozegrane != 0)
                {
                    druzyna.wynik = ((double)zwyciestwa + 0.5 * (double)remisy) / (double)druzyna.meczeRozegrane;
                    listaDruzyn.Add(druzyna);
                    //Console.WriteLine("wynik: " + druzyna.wynik);
                }
            }

            //tworzenie pierwszych ratingów
            double sredniRatingPrzeciwnikow = 0;
            double sumaRatingow = 0;

            // suma ratingów maleje z każdą iteracją przez co ratingi są coraz mniejsze
            
            for (int i = 0; i < 15; i++)
            {
                sumaRatingow = 0;
                foreach (DruzynaPR druzyna in listaDruzyn)
                {
                    //if(druzyna.meczerozegrane != 0) 
                    sumaRatingow += druzyna.Rating;                    
                }
                foreach (DruzynaPR druzyna in listaDruzyn)
                {
                    sredniRatingPrzeciwnikow = (sumaRatingow - druzyna.Rating) / (listaDruzyn.Count - 1);

                    //sredniRatingPrzeciwnikow = (sumaRatingow - druzyna.PerformanceRating) / (listaDruzyn.Count - 1);
                    
                    //Console.WriteLine("sumaRat: {0}, druzynaRat: {1}, liczba druzyn: {2} ",sumaRatingow,druzyna.PerformanceRating,listaDruzyn.Count());
                    //Console.WriteLine("sredni rating: " + sredniRatingPrzeciwnikow);
                    druzyna.PerformanceRating = sredniRatingPrzeciwnikow + ((druzyna.wynik - 0.5) * 850);
                    druzyna.Rating = Convert.ToInt32(43 + ((druzyna.PerformanceRating * druzyna.meczeRozegrane)+(sredniRatingPrzeciwnikow * 4) + (2300*3)) / (druzyna.meczeRozegrane + 7));
                }
            }
            
            /*foreach (DruzynaPR druzyna in listaDruzyn)
            {
                druzyna.Rating = Convert.ToInt32(druzyna.PerformanceRating);                
            }*/

            ZaktualizujTabelęDrużyny(listaDruzyn);
            return listaDruzyn;            
        }
        
        private void InitBinding(List<DruzynaPR> lista)
        {
            Ranking.ItemsSource = lista;
            

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(Ranking.ItemsSource);
            view.SortDescriptions.Add(new SortDescription("Rating", ListSortDirection.Descending));
        }

        private void ZaktualizujTabelęDrużyny(List<DruzynaPR> listaDruzyn)
        {
            SQLiteConnection sqlCon = new SQLiteConnection(dbConnectionString);
            SQLiteCommand sqlCommand = sqlCon.CreateCommand();

            sqlCommand.CommandText = "SELECT Nazwa, Rating FROM Druzyny ORDER BY Nazwa asc";
            sqlDataAdapter = new SQLiteDataAdapter(sqlCommand.CommandText, sqlCon);
            SQLiteCommandBuilder sqliteCommandBuilder = new SQLiteCommandBuilder(sqlDataAdapter);
            sqlDataSet = new DataSet();
            sqlDataAdapter.Fill(sqlDataSet);
            sqlDataTable = sqlDataSet.Tables[0];

            foreach (DruzynaPR druzyna in listaDruzyn)
            {
                foreach (DataRowView item in sqlDataTable.DefaultView)
                {
                    if (druzyna.nazwa == item["Nazwa"].ToString())
                    {
                        DataRow sqlDataRow = item.Row;
                        sqlDataRow.BeginEdit();
                        sqlDataRow["Rating"] = druzyna.Rating;
                        sqlDataRow.EndEdit();
                        sqlDataAdapter.Update(sqlDataSet);
                    }
                }                
            }
            sqlCon.Close();
        }
        private void ZaktualizujRatingiNaLiście(List<DruzynaPR> listaDruzyn)
        {
            SQLiteConnection sqlCon = new SQLiteConnection(dbConnectionString);
            SQLiteCommand sqlCommand = sqlCon.CreateCommand();

            sqlCommand.CommandText = "SELECT Nazwa, Rating FROM Druzyny ORDER BY Nazwa asc";
            sqlDataAdapter = new SQLiteDataAdapter(sqlCommand.CommandText, sqlCon);
            SQLiteCommandBuilder sqliteCommandBuilder = new SQLiteCommandBuilder(sqlDataAdapter);
            sqlDataSet = new DataSet();
            sqlDataAdapter.Fill(sqlDataSet);
            sqlDataTable = sqlDataSet.Tables[0];

            foreach (DruzynaPR druzyna in listaDruzyn)
            {
                foreach (DataRowView item in sqlDataTable.DefaultView)
                {
                    if (druzyna.nazwa == item["Nazwa"].ToString())
                    {
                        druzyna.Rating = Convert.ToInt32(item["Rating"]);
                    }
                }
            }
            sqlCon.Close();
        }
        public oknoRanking()
        {                        
            InitializeComponent();          
            InitBinding(StworzPierwszyRanking());            
        }

        private string PodajDatę(DateTime data)
        {
            string format = "yyyy-MM-dd";            
            return data.ToString(format);
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
