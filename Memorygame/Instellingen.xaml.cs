using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for Instellingen.xaml
    /// </summary>
    public partial class Instellingen : Window, INotifyPropertyChanged
    {
        int breedte = 4;
        int lengte = 4;
        string _breedteLengte;
        int _aantalSets;
        string _aantalSetsString;
        string _thema;
        string padInstellingen;
        string[] _instellingen;

        /// <summary>
        /// Controleerd of instellingenbestand is ingevuld en leest deze in in _instellingen stringArray.
        /// Indien bestand niet bestaat worden standaard instellingen toegepast.
        /// </summary>
        public Instellingen(string _padInstelingen)
        {
            InitializeComponent();
            DataContext = this;
            padInstellingen = _padInstelingen;
            try { _instellingen = File.ReadAllLines(padInstellingen); }
            catch (Exception) { MessageBox.Show("Het instellingenbestand is niet beschikbaar. Probeer het over enkele seconden opnieuw of probeer het spel opnieuw te starten."); return; };
            
            if (_instellingen.Length == 0)
            {
                standaardInstellingen();
            } else
            {
                changeBreedteLengte(Convert.ToInt32(_instellingen[0]), Convert.ToInt32(_instellingen[1]));
                aantalSets = Convert.ToInt32(_instellingen[2]);
                thema = _instellingen[3];
            } 
        }

        /// <summary>
        /// Indien geen bestanden aanwezig, geef standaard instellingen mee
        /// </summary>
        public Instellingen()
        {

            standaardInstellingen();

        }



        /// <summary>
        /// String met hierin Breedte * Lengte verwerkt tbv instellingenscherm.
        /// String wordt gewijzigd door changeBreedteLengte functie
        /// </summary>
        public string breedteLengte
        {
            get { return _breedteLengte; }
            set
            {
                _breedteLengte = value;
                PropertyGewijzigd();
            }
        }
        /// <summary>
        /// String met aantal Sets tbv instellingen scherm
        /// Wordt automatisch gewijzigd voor aantalSets int
        /// </summary>
        public string aantalSetsString
        {
            get { return _aantalSetsString; }
            set
            {
                _aantalSetsString = value;
                PropertyGewijzigd();
            }
        }
        /// <summary>
        /// Wijzig het aantal te raden sets
        /// Verifieer eerst of het aantal sets geldig is, anders foutmelding weergeven met verdere instructies
        /// Maak daarna string aan voor aantalSetsString tvb instellingenscherm
        /// </summary>
        public int aantalSets
        {
            get { return _aantalSets; }
            set
            {
                if (value < 1)
                    value = _aantalSets;
                if (value > (breedte * lengte) / 2)
                {
                    value = _aantalSets;
                    MessageBox.Show("Je hebt een grid van " + breedteLengte + ". Hierin zijn maximaal " + Convert.ToString((breedte * lengte) / 2) + " combinatie's mogelijk.");
                }
                if (((breedte * lengte - (breedte * lengte % 2)) % value) != 0)
                {
                    value = _aantalSets;
                    MessageBox.Show("Dit aantal sets komt niet uit met het huidige grid van " + breedteLengte);
                }
                _aantalSets = value;
                PropertyGewijzigd();
                aantalSetsString = Convert.ToString(value) + " paar";
            }
        }
        /// <summary>
        /// String met hierin het thema
        /// </summary>
        public string thema
        {
            get
            {
                return _thema;
            } set
            {
                _thema = value;
                PropertyGewijzigd();
            }
        }

        private void standaardInstellingen()
        {
            changeBreedteLengte(4, 4);
            aantalSets = 1;
            thema = "Smiley's";
        }
        /// <summary>
        /// Functie als er op instellingen opslaan knop wordt geklikt.
        /// schijf instellingen weg in volgorde: breedte, lengte, aantalSets, Thema
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Opslaan(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(padInstellingen, string.Format("{0}\n{1}\n{2}\n{3}", lengte, breedte, aantalSets, thema));
            this.Close();
        }

        /// <summary>
        /// Verander breedte en lengte variabelen en pas string aan tbv instellingenscherm
        /// </summary>
        /// <param name="_breedte">Breedte grid</param>
        /// <param name="_lengte">Lengte Grid</param>
        private void changeBreedteLengte(int _breedte, int _lengte)
        {
            if ((_breedte * _lengte) / 2 < aantalSets )
            {
                MessageBox.Show("Dit Grid kan niet gekozen worden in combinatie met het aantal minimale te raden combo's van " + Convert.ToString(aantalSets));
                return;
            }
            breedte = _breedte;
            lengte = _lengte;
            breedteLengte = Convert.ToString(breedte) + " * " + Convert.ToString(lengte);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Update property als content in variabele is veranderd
        /// </summary>
        /// <param name="propertyName">De property met een nieuwe waarde</param>
        protected void PropertyGewijzigd([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {

                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);

            }
        }

        private void setKlik4(object sender, RoutedEventArgs e)
        {
            changeBreedteLengte(4, 4);
        }
        private void setKlik5(object sender, RoutedEventArgs e)
        {
            changeBreedteLengte(5, 5);
        }

        private void setKlik6(object sender, RoutedEventArgs e)
        {
            changeBreedteLengte(6, 6);
        }

        /// <summary>
        /// Genereer string met hierin de instellingen
        /// </summary>
        /// <returns>string Array in volgorde: [0] = breedte [1] = lengte [2] = aantal sets te raden [3] = thema</returns>
        public string[] ophalen()
        {
            string[] _return = new string[5];
            _return[0] = Convert.ToString(breedte);
            _return[1] = Convert.ToString(lengte);
            _return[2] = Convert.ToString(aantalSets);
            _return[3] = thema;
            return _return;
        }

        private void themaKlikSmileys(object sender, RoutedEventArgs e)
        {
            thema = "Smiley's";
        }

        private void themaKlikDieren(object sender, RoutedEventArgs e)
        {
            thema = "Dieren";
        }

        private void themaKlikVoedsel(object sender, RoutedEventArgs e)
        {
            thema = "Voedsel";
        }
    }
}