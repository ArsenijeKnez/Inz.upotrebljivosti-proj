using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using Klase;

namespace Papuce
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataIO serializer = new DataIO();
        public static BindingList<Papuca> Papuce { get; set; }
        private List<Papuca> oznacene = new List<Papuca>();
        public static bool Role;

        public MainWindow()
        {
           
            InitializeComponent();
            Startup("Korisnik");
        }

        public MainWindow(bool rol)
        {

            InitializeComponent();
            Role = rol;
            string ime = "Admin";
            if (!rol)
                ime = "Korisnik";
            Startup(ime);
        }

        private void Startup(string ime)
        {
            Dobrodoslica.Content = "Dobrodošao, " + ime;
            Papuce = serializer.DeSerializeObject<BindingList<Papuca>>("Papuce.xml");
            if (Papuce == null)
            {
                Papuce = new BindingList<Papuca>();
            }
            DataContext = this;
            Muzika.MediaEnded += (sender, e) =>
            {
                Muzika.Position = TimeSpan.Zero;
                Muzika.Play();
            };
            Muzika.Play();
        }

        private void OtvoriDodaj(object sender, RoutedEventArgs e)
        {
            Dodaj dodaj = new Dodaj();
            BlurEffect blur = new BlurEffect();
            this.Effect = blur;
            dodaj.Left = 100 + this.Left;
            dodaj.Top = 100 + this.Top;
            dodaj.ShowDialog();
            this.Effect = null;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LogIn log = new LogIn();
            log.Show();
            this.Close();
        }

        private void Brisi(object sender, RoutedEventArgs e)
        {
           foreach(Papuca pap in oznacene)
            {
                Papuce.Remove(pap);
                if (File.Exists(pap.Opis)){
                    File.Delete(pap.Opis);
                }
            }
            oznacene.Clear();
            XmlSerializer serializer = new XmlSerializer(typeof(BindingList<Papuca>));
            using (TextWriter writer = new StreamWriter("Papuce.xml"))
            {
                serializer.Serialize(writer, Papuce);
            }
        }

        private void CheckBoxPapuce_Checked(object sender, RoutedEventArgs e)
        {
            var obj = Tabela.SelectedItem;
            DataGridRow row = Tabela.ItemContainerGenerator.ContainerFromItem(obj) as DataGridRow;
            Papuca oznacena = row.DataContext as Papuca;
            oznacene.Add(oznacena);
        }

        private void CheckBoxPapuce_Unchecked(object sender, RoutedEventArgs e)
        {
            var obj = Tabela.SelectedItem;
            DataGridRow row = Tabela.ItemContainerGenerator.ContainerFromItem(obj) as DataGridRow;
            Papuca oznacena = row.DataContext as Papuca;
            oznacene.Remove(oznacena);
        }
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var obj = Tabela.SelectedItem;
            DataGridRow row = Tabela.ItemContainerGenerator.ContainerFromItem(obj) as DataGridRow;
            Papuca papuca = row.DataContext as Papuca;
            BlurEffect myBlurEffect = new BlurEffect();
            this.Effect = myBlurEffect;
            if (Role)
            {
                Dodaj dodaj = new Dodaj(papuca);
                dodaj.Left = 100 + this.Left;
                dodaj.Top = 100 + this.Top;
                dodaj.ShowDialog();
                this.Effect = null;
            }
            else {
                Pregled pregled = new Pregled(papuca);
                pregled.Left = 378 + this.Left;
                pregled.Top = 120 + this.Top;
                pregled.ShowDialog();
                this.Effect = null;
            }
        }
    }
}
