using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Memorygame
{
    /// <summary>
    /// Interaction logic for Spel.xaml
    /// </summary>
    public partial class Spel : Page, INotifyPropertyChanged
    {
        // variabelen voor het aantal de initializeren rijen en kolommen
        private const int aantalRijen = 4;
        private const int aantalKolommen = 4;
        private int _aantalKaartjes = aantalRijen * aantalKolommen;
        int aantalKlik = 0;
        Image openKaart1;
        Image openKaart2;
        string bronKaart1;
        string bronKaart2;
        string naamSpeler1;
        string naamSpeler2;
        private int _huidigeSpeler = 1;
        private string _naamHuidigeSpeler;
        private string _huidigeScore = "0 - 0";
        private int _huidigeScoreSpeler1 = 0;
        private int _huidigeScoreSpeler2 = 0;
        private int _huidigeCombo = 1;

        /// <summary>
        /// int huidigeSpeler wijzigen
        /// </summary>
        public int huidigeSpeler
        {
            get { return _huidigeSpeler; }
            set
            {
                _huidigeSpeler = value;
                // aangezien huidge speler ID wordt gewijzigd, moet ook de naam worden gewijzigd via de wijzigNaamHuidigeSpeler functie
                wijzigNaamHuidigeSpeler(value);
            }
        }

        /// <summary>
        /// String naamHuidigeSpeler wijzigen
        /// </summary>
        public string naamHuidigeSpeler
        {
            get
            {
                return _naamHuidigeSpeler;
            }
            set
            {
                _naamHuidigeSpeler = value;
                PropertyGewijzigd();
            }
        }
        /// <summary>
        /// huidigeScore string wijzigen
        /// </summary>
        public string huidigeScore
        {
            get
            {
                return _huidigeScore;
            }
            set
            {
                _huidigeScore = value;
                PropertyGewijzigd();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int huidigeCombo
        {
            get
            {
                return _huidigeCombo;
            }
            set
            {
                _huidigeCombo = value;
                PropertyGewijzigd();
            }
        }

        public int aantalKaartjes
        {
            get
            {
                return _aantalKaartjes;
            }
            set
            {
                _aantalKaartjes = value;
                if (_aantalKaartjes == 0)
                {
                    if (_huidigeScoreSpeler1 > _huidigeScoreSpeler2)
                    {
                        MessageBox.Show(naamSpeler1 + " heeft gewonnen met " + _huidigeScoreSpeler1 + " punten tegen " + naamSpeler2 + " met " + _huidigeScoreSpeler2 + " punten");
                    } else if (_huidigeScoreSpeler2 > _huidigeScoreSpeler1)
                    {
                        MessageBox.Show(naamSpeler2 + " heeft gewonnen met " + _huidigeScoreSpeler2 + " punten tegen " + naamSpeler1 + " met " + _huidigeScoreSpeler1 + " punten");
                    }
                    else
                    {
                        MessageBox.Show("Gelijk spel!");
                    }
                }
            }
        }

        /// <summary>
        /// Wijzig adv ID nummer de naam van de huidige speler
        /// </summary>
        /// <param name="_huidigeSpelerNummer">ID nummer speler welke aan de beurt is</param>
        private void wijzigNaamHuidigeSpeler(int _huidigeSpelerNummer)
        {
            if (_huidigeSpelerNummer == 1)
            {
                naamHuidigeSpeler = naamSpeler1;
            }
            else if (_huidigeSpelerNummer == 2)
            {
                naamHuidigeSpeler = naamSpeler2;
            }
        }

        /// <summary>
        /// Wijzig punten
        /// </summary>
        /// <param name="_voorSpeler">ID nummer te wijzigen speler. Gebruik ID 0 om scores te resetten</param>
        /// <param name="_aantalPunten">Het aantal op te tellen punten</param>
        private void puntenWijzigen(int _voorSpeler, int _aantalPunten)
        {
            if (_voorSpeler == 1)
            {
                _huidigeScoreSpeler1 += _aantalPunten;
            }
            else if (_voorSpeler == 2)
            {
                _huidigeScoreSpeler2 += _aantalPunten;
            }
            else if (_voorSpeler == 0)
            {
                _huidigeScoreSpeler1 = 0;
                _huidigeScoreSpeler2 = 0;
            }
            // pas string aan met daarin netjes de score verwerkt
            huidigeScore = _huidigeScoreSpeler1 + " - " + _huidigeScoreSpeler2;
        }

        /// <summary>
        /// Start het spel
        /// </summary>
        /// <param name="_speler1">Naam speler 1</param>
        /// <param name="_speler2">Naam speler 2</param>
        public Spel(string _speler1, string _speler2)
        {

            InitializeComponent();
            DataContext = this;
            // roep initialiseerGrid methode aan om Grid aan te maken
            initialiseerGrid();
            // voeg kaartjes toe in grid elementen
            genereerMemoryKaartjes();
            // genereer scoreTelling
            scoreTeller();
            // zet namen in variabelen
            naamSpeler1 = _speler1;
            naamSpeler2 = _speler2;
            _naamHuidigeSpeler = naamSpeler1;
            label_Score_namen.Text = naamSpeler1 + " - " + naamSpeler2; ;
        }

        private void scoreTeller()
        {
            //speler.Text("hier komt scoretelling");
        }


        /// <summary>
        /// Genereer afbeeldingen lijst
        /// </summary>
        /// <returns>Geeft lijst terug met ImageSources</returns>
        private List<ImageSource> genImgLijst()
        {
            // maak lijst aan
            List<ImageSource> afbeeldingen = new List<ImageSource>();
            // bereken hoeveel afbeeldingen nodig is
            for (int i = 0; i < aantalRijen * aantalKolommen; i++)
            {
                // voor elke 2 hokjes is dezelfde afbeelding nogig, berekenen met divider
                int afbeeldingnr = i % 8 + 1;
                // maak een ImageSource aan genaamd bron, vul hier het imagepad in met het adbeeldingsnummer
                ImageSource bron = new BitmapImage(new Uri("images/" + afbeeldingnr + ".png", UriKind.Relative));
                // voeg bron toe aan de lijst
                afbeeldingen.Add(bron);
            }
            // als lijst compleet gevuld is, return deze
            return afbeeldingen;
        }
        /// <summary>
        /// Genereer MemoryKaartjes
        /// </summary>
        private void genereerMemoryKaartjes()
        {
            // genereer random lijst met afbeeldingen
            List<ImageSource> achterkantImg = genImgLijst();
            // loop alle rijen af
            for (int rij = 0; rij < aantalRijen; rij++)
            {
                // loop in betreffende rij alle kolommen af
                for (int kolom = 0; kolom < aantalKolommen; kolom++)
                {
                    //maak Image aan
                    Image kaartje = new Image();
                    // Vul bron met vraagteken image
                    kaartje.Source = new BitmapImage(new Uri("Vraagteken.png", UriKind.Relative));
                    // voeg tag toe aan kaartje met afbeelding achterkant, haal deze van lijst
                    kaartje.Tag = achterkantImg.First();
                    // verwijder afbeelding van lijst
                    achterkantImg.RemoveAt(0);
                    // voeg event toe aan kaartje. Bij klik op kaartje voer 'kaartklik' uit
                    kaartje.MouseDown += new MouseButtonEventHandler(kaartklik);
                    // selecteer juiste grid dmv variabelen in de 2 for lussen
                    Grid.SetColumn(kaartje, kolom);
                    Grid.SetRow(kaartje, rij);
                    // voeg ingestelde waarden kaartje in op betreffende grid
                    MemoryGrid.Children.Add(kaartje);
                }
            }
        }
        /// <summary>
        /// Actie bij klikken op memorykaartje
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e"></param>
        private void kaartklik(object sender, MouseButtonEventArgs e)
        {
            Image kaart = (Image)sender;
            // verhhoog aantalKlik variabele met 1
            aantalKlik++;
            if (aantalKlik == 1)
            {
                // als 1 keer in beurt is geklikt, laat dan de achterkant en noteer de waarde van dit plaatje in bronKaart1
                openKaart1 = kaart;
                ImageSource voorkant = (ImageSource)kaart.Tag;
                kaart.Source = voorkant;
                kaart.MouseDown -= kaartklik;
                bronKaart1 = Convert.ToString(voorkant);
            }
            if (aantalKlik == 2)
            {
                // als 2 keer in beurt is geklikt, laat dan de achterkant en noteer de waarde van dit plaatje in bronKaart2
                openKaart2 = kaart;
                ImageSource voorkant = (ImageSource)kaart.Tag;
                kaart.Source = voorkant;
                bronKaart2 = Convert.ToString(voorkant);
                // controleer bij 2 keer klikken of de waarden van bronKaart1 en bronKaart2 gelijk zijn
                if ((bronKaart1) == (bronKaart2))
                {
                    // 2 kaarten zijn gelijk. Doe iets??? en stel aantalKlik weer op 0
                    puntenWijzigen(huidigeSpeler, huidigeCombo);
                    aantalKlik = 0;
                    // zorg dat de 2 kaartjes niet meer kunnen worden aangeklikt
                    openKaart1.MouseDown -= kaartklik;
                    openKaart2.MouseDown -= kaartklik;
                    // haal 2 af van int aantalKaartjes
                    aantalKaartjes -= 2;
                    // verhoog combo
                    huidigeCombo += 1;
                }
            }
            if (aantalKlik == 3)
            {
                // na een 3e klik beide kaartjes weer omdraaien en van speler wisselen
                aantalKlik = 0;
                openKaart1.Source = new BitmapImage(new Uri("Vraagteken.png", UriKind.Relative));
                openKaart2.Source = new BitmapImage(new Uri("Vraagteken.png", UriKind.Relative));
                // voeg klikevent weer toe aan kaart1
                openKaart1.MouseDown += new MouseButtonEventHandler(kaartklik);
                // wissel van speler
                if (huidigeSpeler == 1)
                {
                    huidigeSpeler = 2;
                }
                else if (huidigeSpeler == 2)
                {
                    huidigeSpeler = 1;
                }
                // zet combo op 1
                huidigeCombo = 1;
            }
        }

        /// <summary>
        /// Initialiseer Grid a.d.v. aantal rijen en kolommen
        /// </summary>
        private void initialiseerGrid()
        {
            // voeg rijen toe aan memorygrid
            for (int i = 0; i < aantalRijen; i++)
            {
                MemoryGrid.RowDefinitions.Add(new RowDefinition());
                // bereken beschikbare hoogte per rij en pas deze toe
                MemoryGrid.RowDefinitions[i].Height = new GridLength((650 / aantalRijen));
            }
            // voeg kolommen toe aan memorygrid
            for (int i = 0; i < aantalKolommen; i++)
            {
                MemoryGrid.ColumnDefinitions.Add(new ColumnDefinition());
                // bereken beschikbare breedte per rij en pas deze toe
                MemoryGrid.ColumnDefinitions[i].Width = new GridLength((1150 / aantalRijen));
            }
        }

        /// <summary>
        /// Reset spel als er op de knop resetknop wordt geklikt
        /// </summary>
        /// <returns></returns>
        private void resetSpel(object sender, RoutedEventArgs e)
        {
            // wis grid
            MemoryGrid.Children.Clear();
            // maak grid aan en genereer kaartjes
            initialiseerGrid();
            genereerMemoryKaartjes();
            // zet speler weer op 1
            huidigeSpeler = 1;
            aantalKlik = 0;
            // reset scores
            puntenWijzigen(0, 0);
            MessageBox.Show("Spel is gereset");
            // reset combo
            huidigeCombo = 1;
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

    }
}
