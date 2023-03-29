using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            Startup("Guest");
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
            Papuca kroksi = new Papuca("kroks", 12, "Slike/crocs.jpg", null, DateTime.Now);
            Papuce.Add(kroksi);
            Papuca crne = new Papuca("Vitapur", 7, "Slike/1.jpg", null, DateTime.Now);
            Papuce.Add(crne);
            Papuca plave = new Papuca("Abidas", 23, "Slike/4.jpg", null, DateTime.Now);
            Papuce.Add(plave);
            Papuca adi = new Papuca("Adidas", 12, "Slike/adidas.jpg", null, DateTime.Now);
            Papuce.Add(adi);

        }

        private void OtvoriDodaj(object sender, RoutedEventArgs e)
        {
            Dodaj dodaj = new Dodaj();
            dodaj.ShowDialog();
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
            }
            oznacene.Clear();
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
            if (Role)
            {
                Dodaj dodaj = new Dodaj(papuca);
                dodaj.ShowDialog();
            }
            else {
                Pregled pregled = new Pregled(papuca);
            pregled.ShowDialog();
            }
        }
    }
}
