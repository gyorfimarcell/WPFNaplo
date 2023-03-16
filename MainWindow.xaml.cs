using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace WpfOsztalyzas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string fajlNev;
        //Így minden metódus fogja tudni használni.
        ObservableCollection<Osztalyzat> jegyek = new ObservableCollection<Osztalyzat>();

        public MainWindow()
        {
            InitializeComponent();
            FajlValasztas();
        }

        private void FajlValasztas() {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                fajlNev = openFileDialog.FileName;
            else
            {
                fajlNev = "naplo.txt";
            }
            lblFajlnev.Content = fajlNev;

            JegyekBetolt();
        }

        private void btnRogzit_Click(object sender, RoutedEventArgs e)
        {
            string[] tagok = txtNev.Text.Split(' ');
            if (tagok.Length < 2)
            {
                MessageBox.Show("A névnek több szóból kell álnia!");
                return;
            }
            foreach (string tag in tagok)
            {
                if (tag.Length < 3)
                {
                    MessageBox.Show("A szavaknak minimum 3 karakterből kell állniuk!");
                    return;
                }
            }

            if (DateTime.Parse(datDatum.Text) > DateTime.Today)
            {
                MessageBox.Show("Nem lehet jövőbeli dátumot megadni!");
                return;
            }


            //A CSV szerkezetű fájlba kerülő sor előállítása
            string csvSor = $"{txtNev.Text};{datDatum.Text};{cboTantargy.Text};{sliJegy.Value}";
            //Megnyitás hozzáfűzéses írása (APPEND)
            StreamWriter sw = new StreamWriter(fajlNev, append: true);
            sw.WriteLine(csvSor);
            sw.Close();
            JegyekBetolt();
        }

        private void btnBetolt_Click(object sender, RoutedEventArgs e)
        {
            FajlValasztas();
        }

        private void JegyekBetolt() {
            jegyek.Clear();  //A lista előző tartalmát töröljük
            StreamReader sr = new StreamReader(fajlNev); //olvasásra nyitja az állományt
            while (!sr.EndOfStream) //amíg nem ér a fájl végére
            {
                string[] mezok = sr.ReadLine().Split(";"); //A beolvasott sort feltördeli mezőkre
                //A mezők értékeit felhasználva létrehoz egy objektumot
                Osztalyzat ujJegy = new Osztalyzat(mezok[0], mezok[1], mezok[2], int.Parse(mezok[3]));
                jegyek.Add(ujJegy); //Az objektumot a lista végére helyezi
            }
            sr.Close(); //állomány lezárása

            //A Datagrid adatforrása a jegyek nevű lista lesz.
            //A lista objektumokat tartalmaz. Az objektumok lesznek a rács sorai.
            //Az objektum nyilvános tulajdonságai kerülnek be az oszlopokba.
            if (rbNevVK.IsChecked == true)
            {
                dgJegyek.ItemsSource = jegyek;
            }
            else
            {
                  dgJegyek.ItemsSource = jegyek.Select(osztalyzat => { osztalyzat.Nev = osztalyzat.ForditottNev(); return osztalyzat; }) ;  
            }
            
            lblJegyek.Content = jegyek.Count;
            lblAtlag.Content = Math.Round(jegyek.Average(osztalyzat => osztalyzat.Jegy), 1);
        }

        private void sliJegy_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            lblJegy.Content = sliJegy.Value; //Több alternatíva van e helyett! Legjobb a Data Binding!
        }

        private void radio_Checked(object sender, RoutedEventArgs e)
        {
            if (fajlNev != null)
            {
                JegyekBetolt();
            }
        }
    }
}

