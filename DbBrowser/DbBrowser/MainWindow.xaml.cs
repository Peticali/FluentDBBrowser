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
using System.Diagnostics;
using System.Data;
using System.Globalization;

namespace DbBrowser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
      
        public MainWindow()
        {
            System.Diagnostics.PresentationTraceSources.DataBindingSource.Switch.Level = System.Diagnostics.SourceLevels.Critical;
            InitializeComponent();
        }

        

        private SQLiteDataAdapter runSql(String sql)
        {
            var cmd = sqliteConnection.CreateCommand();
            cmd.CommandText = sql;

            //SQLiteDataReader sqlExecuted = cmd.ExecuteReader();

            SQLiteDataAdapter dataAdapter = new SQLiteDataAdapter();
            dataAdapter = new SQLiteDataAdapter(cmd.CommandText, sqliteConnection);

            return dataAdapter;

        }



        private void listTables()
        {

            var cmd = sqliteConnection.CreateCommand();
            cmd.CommandText = "SELECT name FROM sqlite_master WHERE type='table'";

            SQLiteDataReader tables = cmd.ExecuteReader();


            List<string> ImportedFiles = new List<string>();


            while (tables.Read())
            {
                ImportedFiles.Add(Convert.ToString(tables["name"]));
            }


            ImportedFiles.ForEach(item => Debug.Write(item));

            Combobox1.ItemsSource = ImportedFiles;
            Combobox1.SelectedIndex = 1;


        }
        
        private static SQLiteConnection sqliteConnection;
        private void CreateConnection(object sender, MouseButtonEventArgs e)
        {
             
            
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".db"; 
            Nullable<bool> result = dlg.ShowDialog(); // Diag choose DB

            if (result == true)
            {
               
                string filename = dlg.FileName;
                Debug.WriteLine(filename);

                sqliteConnection = new SQLiteConnection("Data Source=" + filename);
                sqliteConnection.Open();

                listTables();
                

                

            }

        }

        private void ChangeTable(object sender, SelectionChangedEventArgs e)
        {

   

            SQLiteDataAdapter dataAdapter = runSql("SELECT * FROM " + Combobox1.SelectedItem);

            DataTable dt = new DataTable();

            try
            {
                dataAdapter.Fill(dt);

                DataGri.ItemsSource = dt.DefaultView;
            }catch{
                
            }

        }



        private void ListViewItem_MouseUp(object sender, MouseButtonEventArgs e)
        {
            String sql = sqlBox.Text;
            try
            {
                
                var cmd = sqliteConnection.CreateCommand();
                cmd.CommandText = sql;

            
                cmd.ExecuteNonQuery();
            }catch{

            }

            ChangeTable(null,null);
        }
    }
}
