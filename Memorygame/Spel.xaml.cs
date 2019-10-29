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
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;

namespace Memorygame
{
    /// <summary>
    /// Interaction logic for Spel.xaml
    /// </summary>
    public partial class Spel : Page, INotifyPropertyChanged
    {
        // variabelen voor het aantal de initializeren rijen en kolommen
        int aantalRijen; // aantal Rijen
        int aantalKolommen; // aantal Kolommen
        int _aantalKaartjes; // Aantal open kaartjes. Wijzigen via Get Set
        int aantalKlik = 0; // aantal huidige klikken
        List<Image> openKaarten = new List<Image>(); // Lijst met open kaarten binnen beurt
        string bronKaart1; // Afbeelding pad openKaart1 in huidige beurt
        string bronKaart2; // Afbeelding pad openKaart2 in huidige beurt
        int aantalComboGoed; // hoeveel combo's goed in huidige beurt.
        int hoeveelComboSetting; // hoeveel combo's moeten goed zijn voor punten 
        int hoeveelComboRaden; // gelijk aan hoeveelComboSetting. Maar wordt gebruikt voor vergelijking
        int _aantalComboTeGaan; // hoeveel combo's te gaan. Get Set
        string _aantalComboTeGaanString; // hoeveel combo's te gaan string. Auto.
        string naamSpeler1; //naam speler 1
        string naamSpeler2; // naam speler 2
        private int _huidigeSpeler; // huidige speler ID. Wijzigen via Get Set
        private string _naamHuidigeSpeler; // Naam huidige speler. Wordt automatisch aangepast als huidigeSpeler ID wordt aangepast
        private string _huidigeScore; // String met huidige score. Wordt automatisch aangepast
        private int huidigeScoreSpeler1; // Huidige score speler 1. Aanpassen via functie PuntenWijzigen
        private int huidigeScoreSpeler2; // Huidige score speler2. Aanpassen via functie PuntenWijzigen
        private int _huidigeMultiplier; // Huidige Multiplier. Aanpassen via Get Set
        private string _huigeMultiplierString;  //String huidige multiplier. Wordt automatisch aangepast
        bool spelHervatten; // bool, indien true dan is het een hervat spel
        List<String> ImgLijstString = new List<String>(); // lijst met afbeeldingen paden op volgorde (tbv save functie)
        List<String> omgedraaieKaartjes = new List<String>(); // lijst met adfbeeldingen paden welke zijn opgedraaid (tbv save functie)

        /// <summary>
        /// int huidigeSpeler wijzigen
        /// </summary>
        public int huidigeSpeler
        {
            get { return _huidigeSpeler; }
            set
            {
                _huidigeSpeler = value;
                naamHuidigeSpeler = value == 1 ? naamSpeler1 : naamSpeler2;
            }
        }

        /// <summary>
        /// String naamHuidigeSpeler wijzigen
        /// </summary>
        public string naamHuidigeSpeler
        {
            get { return _naamHuidigeSpeler; }
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
            get { return _huidigeScore; }
            set
            {
                _huidigeScore = value;
                PropertyGewijzigd();
            }
        }

        /// <summary>
        /// Aantal combo's te gaan int wijzigen
        /// </summary>
        public int aantalComboTeGaan
        {
            get { return _aantalComboTeGaan; }
            set
            {
                // als het aantal te raden combo's hoger is dan de overgbleven kaarten, pas combo's dan hier op aan
                if (value >= aantalKaartjes / 2)
                {
                    value = aantalKaartjes / 2;
                    hoeveelComboRaden = value;
                }
                _aantalComboTeGaan = value;
                aantalComboTeGaanString = "Nog " + Convert.ToString(value) + " combo's te gaan";
            }
        }

        /// <summary>
        /// Huidge Combo's te gaan string wijzigen
        /// </summary>
        public string aantalComboTeGaanString
        {
            get { return _aantalComboTeGaanString; }
            set
            {
                _aantalComboTeGaanString = value;
                PropertyGewijzigd();
            }
        }


        /// <summary>
        /// Aantal Multiplier
        /// </summary>
        public int huidigeMultiplier
        {
            get { return _huidigeMultiplier; }
            set
            {
                _huidigeMultiplier = value;
                huigeMultiplierString = "De multiplier staat op " + Convert.ToString(value);
            }
        }

        /// <summary>
        /// Huidge multiplier string
        /// </summary>
        public string huigeMultiplierString
        {
            get { return _huigeMultiplierString; }
            set
            {
                _huigeMultiplierString = value;
                PropertyGewijzigd();
            }
        }
        /// <summary>
        /// Aantal kaartjes int wijzigen. Check direct voor winnaar als kaartjes 0 is
        /// </summary>
        public int aantalKaartjes
        {
            get { return _aantalKaartjes; }
            set
            { 
                _aantalKaartjes = value;
                if (_aantalKaartjes == 0)
                {
                    if (huidigeScoreSpeler1 > huidigeScoreSpeler2)
                    {
                        MessageBox.Show(naamSpeler1 + " heeft gewonnen met " + huidigeScoreSpeler1 + " punten tegen " + naamSpeler2 + " met " + huidigeScoreSpeler2 + " punten");
                    }
                    else if (huidigeScoreSpeler2 > huidigeScoreSpeler1)
                    {
                        MessageBox.Show(naamSpeler2 + " heeft gewonnen met " + huidigeScoreSpeler2 + " punten tegen " + naamSpeler1 + " met " + huidigeScoreSpeler1 + " punten");
                    }
                    else
                    {
                        MessageBox.Show("Gelijk spel!");
                    }
                    // highscore opslaan
                    saven saven = new saven();
                    if (saven.controleerHighscoresBestandAanwezig())
                        saven.highscoreWegscrijven(huidigeScoreSpeler1, huidigeScoreSpeler2, naamSpeler1, naamSpeler2);
                }
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
                huidigeScoreSpeler1 += _aantalPunten;
            }
            else if (_voorSpeler == 2)
            {
                huidigeScoreSpeler2 += _aantalPunten;
            }
            else if (_voorSpeler == 0)
            {
                huidigeScoreSpeler1 = 0;
                huidigeScoreSpeler2 = 0;
            }
            // pas string aan met daarin netjes de score verwerkt
            huidigeScore = huidigeScoreSpeler1 + " - " + huidigeScoreSpeler2;
        }

        /// <summary>
        /// Start het spel
        /// </summary>
        /// <param name="_beginscoreSpeler1">Beginscore speler 1</param>
        /// <param name="_beginscoreSpeler2">Beginscore speler 2</param>
        /// <param name="_naamSpeler1">Naam speler 1</param>
        /// <param name="_naamSpeler2">naam speler 2</param>
        /// <param name="_combo">huidige combo</param>
        /// <param name="_huidigeSpeler">huidige speler</param>
        /// <param name="_spelHervatten">betreft het een herstart spel?</param>
        /// <param name="_breedte">aantal kolommen</param>
        /// <param name="_lengte">aantal Rijen</param>
        /// <param name="_aantalCombo">aantal te raden combo's voor punt</param>
        /// <param name="_aantalKaartjes">Aantal kaartjes dat nog over is. Gebruik voor nieuw spel hier een 0</param>
        /// <param name="_multiplier">Multiplier status</param>

        public Spel(int _beginscoreSpeler1, int _beginscoreSpeler2, string _naamSpeler1, string _naamSpeler2, int _combo, int _huidigeSpeler, bool _spelHervatten, int _breedte, int _lengte, int _aantalCombo, int _aantalKaartjes, int _multiplier)
        {

            InitializeComponent();
            DataContext = this;
            // variabelen invullen
            aantalRijen = _lengte;
            aantalKolommen = _breedte;
            // als aantalKaartjes 0 is (ingegeven voor nieuw spel) bereken dan het aantal gesloten kaartjes aan de hand van Grid size
            if (_aantalKaartjes == 0)
                aantalKaartjes = _lengte * _breedte;
            else
                aantalKaartjes = _aantalKaartjes;
            naamSpeler1 = _naamSpeler1;
            naamSpeler2 = _naamSpeler2;
            huidigeSpeler = _huidigeSpeler;
            // stel punten in voor spelers
            puntenWijzigen(1, _beginscoreSpeler1);
            puntenWijzigen(2, _beginscoreSpeler2);
            // vul bool of het een hervat spel betreft
            spelHervatten = _spelHervatten;
            hoeveelComboRaden = _aantalCombo;
            hoeveelComboSetting = _aantalCombo;
            aantalComboTeGaan = _aantalCombo;
            huidigeMultiplier = _multiplier;
            // als herstart spel, haal kaartjesLijst positie op
            if (spelHervatten)
            {
                saven ophalen = new saven();
                ImgLijstString = ophalen.kaartjesLijst();
                omgedraaieKaartjes = ophalen.omgedraaideKaartjes();
            }
            // roep initialiseerGrid methode aan om Grid aan te maken
            initialiseerGrid();
            // voeg kaartjes toe in grid elementen
            genereerMemoryKaartjes();
            // genereer scoreTelling
            scoreTeller();
            // als nieuw spel, maar save gevonden, verwijder save
            if (!spelHervatten)
            {
                saven saven = new saven();
                saven.resetSav();
            }
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
            List<ImageSource> _gen = new List<ImageSource>();
            // als het een hervat spel betreft, haal opgeslagen lijst op en zet deze om in ImageSource's in _gen lijst
            if (spelHervatten)
            {
                saven ophalen = new saven();
                List<String> _kaartjesLijst = ophalen.kaartjesLijst();
                foreach (string _kaartje in _kaartjesLijst)
                {
                    // maak een ImageSource aan genaamd bron, vul hier het imagepad in met het adbeeldingsnummer
                    ImageSource bron = new BitmapImage(new Uri(_kaartje, UriKind.Relative));
                    // voeg bron toe aan de lijst
                    _gen.Add(bron);
                }
            }
            else
            // als het een nieuw spel betreft, genereer een compleet nieuwe lijst
            {
                for (int i = 0; i < aantalRijen * aantalKolommen; i++)
                {
                    // voor elke 2 hokjes is dezelfde afbeelding nogig, berekenen met divider
                    int afbeeldingnr = i % 8 + 1;
                    // maak een ImageSource aan genaamd bron, vul hier het imagepad in met het adbeeldingsnummer
                    ImageSource bron = new BitmapImage(new Uri("images/" + afbeeldingnr + ".png", UriKind.Relative));
                    // voeg bron toe aan de lijst
                    ImgLijstString.Add(Convert.ToString(bron));
                    _gen.Add(bron);
                }
            }
            return _gen;
        }

        /// <summary>
        /// Maakt de de lijst van afbeeldingen random
        /// </summary>
        /// <returns></returns>

        private List<ImageSource> ShuffleList()
        {
            // Haalt de lijst met afbeeldingen op 
            List<ImageSource> _afbeeldingenR = genImgLijst();
            // Nieuwe lijst met willekeurige afbeeldingen
            List<ImageSource> randomList = new List<ImageSource>();

            // Nieuwe random functie
            Random r = new Random();
            // Begint de index op nul 
            int randomIndex = 0;
            while (_afbeeldingenR.Count > 0)
            {
                randomIndex = r.Next(0, _afbeeldingenR.Count); //Kies een willekeurig object uit de lijst
                randomList.Add(_afbeeldingenR[randomIndex]); //Voeg het toe aan de nieuwe lijst
                _afbeeldingenR.RemoveAt(randomIndex); //Verwijderd element om duplicaten te voorkomen
            }

            return randomList; //return de nieuwe willekeurige lijst
        }
        /// <summary>
        /// Genereer MemoryKaartjes
        /// </summary>
        public void genereerMemoryKaartjes()
        {
            // genereer random lijst met afbeeldingen
            List<ImageSource> _afbeeldingen = ShuffleList();
            // loop alle rijen af
            for (int rij = 0; rij < aantalRijen; rij++)
            {
                // loop in betreffende rij alle kolommen af
                for (int kolom = 0; kolom < aantalKolommen; kolom++)
                {
                    //maak Image aan
                    Image kaartje = new Image();
                    // als het een hervat spel betreft, controleer of deze afbeelding al is omgedraaid
                    if (spelHervatten)
                    {
                        // als het kaartje als is omgedraaid, laat dan de onderkant zien
                        if (omgedraaieKaartjes.Contains(Convert.ToString(_afbeeldingen[0])))
                        {
                            kaartje.Source = new BitmapImage(new Uri(Convert.ToString(_afbeeldingen[0]), UriKind.Relative));
                        }
                        // als het kaartje nog niet is omgedraaid
                        else
                        {
                            // Vul bron met vraagteken image
                            kaartje.Source = new BitmapImage(new Uri("Vraagteken.png", UriKind.Relative));
                            // voeg event toe aan kaartje. Bij klik op kaartje voer 'kaartklik' uit
                            kaartje.MouseDown += new MouseButtonEventHandler(kaartklik);
                            
                        }
                    }
                    // als het een nieuw spel is, voeg de kaartjes dan normaal toe allemaal gesloten en met MouseDown event
                    else
                    {
                        // Vul bron met vraagteken image
                        kaartje.Source = new BitmapImage(new Uri("Vraagteken.png", UriKind.Relative));
                        // voeg event toe aan kaartje. Bij klik op kaartje voer 'kaartklik' uit
                        kaartje.MouseDown += new MouseButtonEventHandler(kaartklik);
                    }
                    // voeg tag toe aan kaartje met afbeelding achterkant, haal deze van lijst
                    kaartje.Tag = _afbeeldingen[0];
                    // verwijder bovenstaande imgsrc
                    _afbeeldingen.RemoveAt(0);
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
                openKaarten.Add(kaart);
                ImageSource achterkant = (ImageSource)kaart.Tag;
                kaart.Source = achterkant;
                kaart.MouseDown -= kaartklik;
                bronKaart1 = (Convert.ToString(kaart.Tag));
            }
            if (aantalKlik == 2)
            {
                // als 2 keer in beurt is geklikt, laat dan de achterkant en noteer de waarde van dit plaatje in bronKaart2
                openKaarten.Add(kaart);
                ImageSource achterkant = (ImageSource)kaart.Tag;
                kaart.Source = achterkant;
                kaart.MouseDown -= kaartklik;
                bronKaart2 = (Convert.ToString(kaart.Tag));
                // controleer bij 2 keer klikken of de waarden van bronKaart1 en bronKaart2 gelijk zijn
                if ((bronKaart1) == (bronKaart2))
                {
                    aantalKlik = 0;
                    // verhoog combo
                    aantalComboGoed++;
                    aantalComboTeGaan--;
                    // zorg dat de kaartjes niet meer kunnen worden aangeklikt
                    if (aantalComboGoed == hoeveelComboRaden)
                    {
                        // Kaarten zijn gelijk. Verhoog punten met huidige combo stand en stel aantalKlik weer op 0
                        puntenWijzigen(huidigeSpeler, huidigeMultiplier);
                        // haal 2 af van int aantalKaartjes
                        aantalKaartjes -= openKaarten.Count();
                        // Voeg tag van omgedraaide kaartjes toe aan lijst met omgedraaide kaartjes
                        foreach (Image i in openKaarten)
                        {
                            string _string = Convert.ToString(i.Tag);
                            omgedraaieKaartjes.Add(_string.Substring(_string.IndexOf("images")));
                        }
                        openKaarten.Clear();
                        aantalComboGoed = 0;
                        aantalComboTeGaan = hoeveelComboSetting;
                        huidigeMultiplier++;
                    }
                }
            }
            if (aantalKlik == 3)
            {
                    // wissel van speler
                    huidigeSpeler = huidigeSpeler == 1 ? 2 : 1;
                    // na een 3e klik beide kaartjes weer omdraaien en van speler wisselen
                    aantalKlik = 0;
                // zet multiplier weer op 1
                huidigeMultiplier = 1;
                foreach (Image x in openKaarten)
                {
                    x.Source = new BitmapImage(new Uri("Vraagteken.png", UriKind.Relative));
                    x.MouseDown += new MouseButtonEventHandler(kaartklik);
                }
                openKaarten.Clear();
                // zet combo op orgineel
                aantalComboGoed = 0;
                aantalComboTeGaan = hoeveelComboSetting;
            }
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            Image kaart = (Image)sender;
            if (e.KeyData == (Keys.A | Keys.D1)) 
            {
                Grid.SetColumn(kaart, 1);
                Grid.SetRow(kaart, 1);
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
            ImgLijstString.Clear();
            omgedraaieKaartjes.Clear();
            initialiseerGrid();
            genereerMemoryKaartjes();
            // aantal open combo's op 0
            aantalComboGoed = 0;
            // reset aantal kaarten
            aantalKaartjes = aantalRijen * aantalKolommen;
            // zet speler weer op 1
            huidigeSpeler = 1;
            aantalKlik = 0;
            // reset scores
            puntenWijzigen(0, 0);
            MessageBox.Show("Spel is gereset");
            // reset multiplier
            huidigeMultiplier = 1;
            // reset combo
            aantalComboTeGaan = hoeveelComboSetting;
            hoeveelComboRaden = hoeveelComboSetting;
            // reset huidige speler
            huidigeSpeler = 1;
            // reset aantal klik
            aantalKlik = 0;
            // maak lijst open kaarten leeg
            openKaarten.Clear();
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

        /// <summary>
        /// Spel opslaan klik
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void opslaan(object sender, RoutedEventArgs e)
        {
            saven saven = new saven();
            // controleer of Sav bestand aanwezig is
            if (saven.controleerSavBestandAanwezig())
            {
                saven.savebestandWegscrijven(huidigeScoreSpeler1, huidigeScoreSpeler2, naamSpeler1, naamSpeler2, huidigeMultiplier, huidigeSpeler, ImgLijstString, omgedraaieKaartjes, aantalRijen, aantalKolommen, hoeveelComboSetting, aantalKaartjes, huidigeMultiplier);
                MessageBox.Show("Opslaan gelukt");
            } else
            // sav bestand niet aanwezig!
            {
                MessageBox.Show("Het opslaan is niet beschikbaar doordat het SAV bestand mist");
            }
            
        }
    }
}
