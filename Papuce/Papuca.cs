using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;

namespace Papuce
{
    [Serializable]
    public class Papuca
    {
        public Papuca()
        {
        }

        public string Ime { get; set; }
        public int Kolicina { get; set; }
        public string Slika { get; set; } 
        public string Opis { get; set; }
        public DateTime DatumKacenja { get; set;}


        public Papuca(string ime, int kolicina, string slika, string opis, DateTime datumKacenja)
        {
            Ime = ime;
            Kolicina = kolicina;
            Slika = slika;
            Opis = opis;
            DatumKacenja = datumKacenja;
        }
    }
}
