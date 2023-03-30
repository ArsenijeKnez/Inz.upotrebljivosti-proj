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
using System.Windows.Shapes;
using Klase;

namespace Papuce
{
    /// <summary>
    /// Interaction logic for Pregled.xaml
    /// </summary>
    public partial class Pregled : Window
    {
        public Papuca Papuca { get; set; }
        public Pregled(Papuca papuca)
        {
            InitializeComponent();
            TextRange range;
            FileStream fStream;
            if (File.Exists(papuca.Opis))
            {
                range = new TextRange(Opis.Document.ContentStart, Opis.Document.ContentEnd);
                fStream = new FileStream(papuca.Opis, FileMode.OpenOrCreate);
                range.Load(fStream, DataFormats.XamlPackage);
                fStream.Close();
            }
            Papuca = papuca;
            NazivObj.Content = Papuca.Ime;
            BR.Content = Papuca.Kolicina;
            dat.Content = Papuca.DatumKacenja;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri(Papuca.Slika, UriKind.RelativeOrAbsolute);
            image.EndInit();
            slika.Source = image;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
