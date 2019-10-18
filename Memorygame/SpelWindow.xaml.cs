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

namespace Memorygame
{
    /// <summary>
    /// Interaction logic for SpelWindow.xaml
    /// </summary>
    public partial class SpelWindow : Window
    {
        public SpelWindow(int optie)
        {
            InitializeComponent();
            DataContext = this;
            // als SpelWindow wordt aangeroepen met 1 dan betekend dit een bestaand spel laden
            if (optie == 1)
            {
                Save spelophalen = new Save();
                if (spelophalen.controleerBestand())
                {
                    // gebruik omgedraaideKaartjes funtie om lijst met omgedraaide kaartjes te vullen
                    List<String> omgedraaideKaartjes = spelophalen.gegevens_omgedraaideKaartjes();
                    // gebruik positieKaartjes funtie om lijst met positie kaartjes te vullen
                    List<String> positieKaartjes = spelophalen.gegevens_positieKaartjes();
                    // vul array met basisgegevens
                    string[] basisGegevens = spelophalen.gegevens_basis();
                    // maak spel aan
                    Spel spel = new Spel();
                    // hervat Spel met bovenstaande uitgelezen gegevens
                    spel.hervatSpel(Convert.ToInt32(basisGegevens[0]), Convert.ToInt32(basisGegevens[1]), basisGegevens[2], basisGegevens[3], Convert.ToInt32(basisGegevens[4]), omgedraaideKaartjes, positieKaartjes);
                    // toon spel
                    this.Content = spel;
                } else
                {
                    MessageBox.Show("Geen spel gevonden. Zorg dat de map \"C:\\MemoryGame\" bestaat. But don't worry! Er wordt een nieuw spel gestart!");
                }
                
            }
        }

        private string _Speler1 = "Speler 1";
        private string _Speler2 = "Speler 2";
        public string Speler1
        {
            get
            {
                return _Speler1;
            }
            set
            {
                _Speler1 = value;
            }
        }

        public string Speler2
        {
            get
            {
                return _Speler2;
            }
            set
            {
                _Speler2 = value;
            }
        }

        private void Spel_starten(object sender, RoutedEventArgs e)
        {
            Spel spel = new Spel();
            spel.nieuwSpel(Speler1, Speler2);
            this.Content = spel;
        }
    }
}
