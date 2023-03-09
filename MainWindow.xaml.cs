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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp8
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string[] tantargyak = {"Matematika", "Irodalom", "Nyelvtan", "Történelem", "Angol"};
        List<Osztalyzat> jegyek;

        public MainWindow()
        {
            InitializeComponent();
            cboTantargy.ItemsSource = tantargyak;
            cboTantargy.SelectedIndex = 0;
            dpDatum.Text = DateTime.Today.ToString();
        }

        private void sliJegy_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblJegy.Content = sliJegy.Value;
        }

        private void btnRogzit_Click(object sender, RoutedEventArgs e)
        {
            if (txtNev.Text == "")
            {
                MessageBox.Show("Adj meg egy nevet!");
                return;
            }
            if (DateTime.Parse(dpDatum.Text) > DateTime.Today) {
                MessageBox.Show("Nem adhatsz meg jövőbeli dátumot!");
                return;
            }

            string[] mezok = {
                txtNev.Text,
                cboTantargy.SelectedItem.ToString(),
                dpDatum.Text,
                sliJegy.Value.ToString()
            };
            string sor = String.Join(';', mezok);

            StreamWriter iro = new StreamWriter("naplo.csv", true);
            iro.WriteLine(sor);
            iro.Close();
            MessageBox.Show("Sikeresen rögzítve!");
        }

        private void btnBetolt_Click(object sender, RoutedEventArgs e)
        {
            jegyek = new List<Osztalyzat>();

            StreamReader olvaso = new StreamReader("naplo.csv");
            while (!olvaso.EndOfStream)
            {
                string[] mezok = olvaso.ReadLine().Split(';');
                Osztalyzat jegy = new Osztalyzat(mezok[0], mezok[1], mezok[2], int.Parse(mezok[3]));
                jegyek.Add(jegy);
            }
            olvaso.Close();
            dgJegyek.ItemsSource = jegyek;
            MessageBox.Show("Betöltés sikeres.");
        }
    }
}
