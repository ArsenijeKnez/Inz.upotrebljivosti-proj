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
using System.IO;

namespace Papuce
{
    /// <summary>
    /// Interaction logic for Dodaj.xaml
    /// </summary>
    public partial class Dodaj : Window
    {
        private ImageSource img;
        private Papuca StaraPapuca;
        public Dodaj()
        {
            InitializeComponent();
            int[] FS = { 1, 2, 3, 6, 8, 10, 14, 18};
            Velicina.ItemsSource = FS;
            string[] TC = { "black", "red", "green", "blue" };
            Boja.ItemsSource = TC;
            StaraPapuca = null;

            ImeTB.Text = "unesite ime papuče";
            ImeTB.Foreground = Brushes.LightSlateGray;

            BrojTB.Text = "unesite broj papuča";
            BrojTB.Foreground = Brushes.LightSlateGray;
        }

        public Dodaj(Papuca papuca)
        {
            InitializeComponent();
            int[] FS = { 1, 2, 3, 6, 8, 10, 14, 18 };
            Velicina.ItemsSource = FS;
            string[] TC = { "black", "red", "green", "blue" };
            Boja.ItemsSource = TC;
            StaraPapuca = papuca;
            DodajIzmeni.Content = "Izmeni";
            this.Title = "Izmeni";
            DodajLabel.Content = "Promeni papuče";
            ImeTB.Text = papuca.Ime;
            BrojTB.Text = papuca.Kolicina.ToString();
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(papuca.Slika, UriKind.RelativeOrAbsolute);
            image.EndInit();
            img = image;
            UcitanaSlika.Source = img;
            TextRange range;
            FileStream fStream;
            if (File.Exists(papuca.Opis))
            {
                range = new TextRange(OpisTB.Document.ContentStart, OpisTB.Document.ContentEnd);
                fStream = new FileStream(papuca.Opis, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.XamlPackage);
                fStream.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Dojaj_Novog(object sender, RoutedEventArgs e)
        {
            string file = ImeTB.Text.Trim();
            while(File.Exists(file + ".xaml"))
                file = file + "Copy";
            file = file + ".xaml";
            TextRange tr;
            FileStream fs;
            tr = new TextRange(OpisTB.Document.ContentStart, OpisTB.Document.ContentEnd);
            fs = new FileStream(file, FileMode.Create);
            tr.Save(fs, DataFormats.XamlPackage);
            fs.Close();
            if (Validate())
            {
                Papuca nova = new Papuca(ImeTB.Text, int.Parse(BrojTB.Text.Trim()), img.ToString(), file, DateTime.Now);
                if (MainWindow.Role == true && StaraPapuca != null)
                {
                    nova.DatumKacenja = StaraPapuca.DatumKacenja;
                    MainWindow.Papuce.Remove(StaraPapuca);
                    StaraPapuca = nova;
                }
                if (MessageBox.Show("Potvrdite unos nove papuce: " + ImeTB.Text.Trim(), Title = "Potvrda", button: MessageBoxButton.OKCancel, icon: MessageBoxImage.Question) == MessageBoxResult.OK)
                {
                    MainWindow.Papuce.Add(nova);
                }
            }
        }

        private bool Validate()
        {
            if(ImeTB.Foreground == Brushes.LightSlateGray || ImeTB.Text.Trim() == "")
            {
                MessageBox.Show("Niste uneli ime!", Title = "Loše uneti podaci", button:MessageBoxButton.OK , icon:MessageBoxImage.Warning);
                return false;
            }
            else if (BrojTB.Foreground == Brushes.LightSlateGray || BrojTB.Text.Trim() == "")
            {
                MessageBox.Show("Niste uneli broj!", Title = "Loše uneti podaci", button: MessageBoxButton.OK, icon: MessageBoxImage.Warning);
                return false;
            }
            else if (!int.TryParse(BrojTB.Text.Trim(), out int br))
            {
                MessageBox.Show("Niste uneli validan broj!", Title = "Loše uneti podaci", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
                return false;
            }
            else if(img == null)
            {
                MessageBox.Show("Niste učitali sliku!", Title = "Loše uneti podaci", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void Ucitaj_Sliku(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {
                string filePath = openFileDialog.FileName;
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.UriSource = new Uri(filePath, UriKind.RelativeOrAbsolute);
                image.EndInit();
                img = image;
                UcitanaSlika.Source = image;
            }
        }

        private void OpisTB_SelectionChanged(object sender, RoutedEventArgs e)
        {
            object temp = OpisTB.Selection.GetPropertyValue(Inline.FontWeightProperty);
            Bold.IsChecked = (temp != DependencyProperty.UnsetValue) && (temp.Equals(FontWeights.Bold));

            temp = OpisTB.Selection.GetPropertyValue(Inline.FontSizeProperty);
            Velicina.SelectedItem = temp;

            temp = OpisTB.Selection.GetPropertyValue(Inline.ForegroundProperty);
            Boja.SelectedItem = temp;
        }

        private void Velicina_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Velicina.SelectedItem != null && !OpisTB.Selection.IsEmpty)
            {
                OpisTB.Selection.ApplyPropertyValue(Inline.FontSizeProperty, Velicina.SelectedItem.ToString());
            }
        }

        private void Boja_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Boja.SelectedItem != null && !OpisTB.Selection.IsEmpty)
            {
                OpisTB.Selection.ApplyPropertyValue(Inline.ForegroundProperty, Boja.SelectedItem);
            }
        }

        private void ImeTB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ImeTB.Text == "unesite ime papuče" && ImeTB.Foreground == Brushes.LightSlateGray)
            {
                ImeTB.Text = "";
                ImeTB.Foreground = Brushes.Black;
            }
        }

        private void ImeTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ImeTB.Text.Trim() == "")
            {
                ImeTB.Text = "unesite ime papuče";
                ImeTB.Foreground = Brushes.LightSlateGray;
            }
        }
        private void BrojTB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (BrojTB.Text == "unesite broj papuča" && BrojTB.Foreground == Brushes.LightSlateGray)
            {
                BrojTB.Text = "";
                BrojTB.Foreground = Brushes.Black;
            }
        }

        private void BrojTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (BrojTB.Text.Trim() == "")
            {
                BrojTB.Text = "unesite broj papuča";
                BrojTB.Foreground = Brushes.LightSlateGray;
            }
        }
    }
}
