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
using System.IO;

namespace Memorygame
{
    public partial class Spel : Page, INotifyPropertyChanged
    {
        // variabelen voor het aantal de initializeren rijen en kolommen
        int aantalRijen; // aantal Rijen
        int aantalKolommen; // aantal Kolommen
        int aantalVakjes;
        int _aantalKaartjes; // Aantal open kaartjes. Wijzigen via Get Set
        int aantalKlik = 0; // aantal huidige klikken
        List<Image> openKaarten = new List<Image>(); // Lijst met open kaarten binnen beurt
        string bronKaart1; // Afbeelding pad openKaart1 in huidige beurt
        string bronKaart2; // Afbeelding pad openKaart2 in huidige beurt
        int aantalComboGoed; // hoeveel combo's goed in huidige beurt.
        int hoeveelComboSetting; // hoeveel combo's moeten goed zijn voor punten 
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
        string thema;
        string locatieSavBestand;
        string locatieHighscoreBestand;
        string locatieInstellingenBestand;
        bool spelHervatten; // bool, indien true dan is het een hervat spel
        List<String> basisGegevens = new List<String>();
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
                    if (locatieHighscoreBestand != null)
                    {
                        highscoreWegschrijven();
                        Highscores highscores = new Highscores(locatieHighscoreBestand);
                        highscores.Show();
                    }
                    else
                    {
                        MessageBox.Show("Highscore niet opgeslagen door ontbreken highscore bestand.");
                    }
            resetSpel();

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
        /// Start nieuw spel zonder instellingen bestand en zonder herladen
        /// </summary>
        /// <param name="_naamSpeler1"></param>
        /// <param name="_naamSpeler2"></param>

        public Spel(string _naamSpeler1, string _naamSpeler2)
        {
            InitializeComponent();
            DataContext = this;
            Instellingen instellingen = new Instellingen();
            string[] instellingenArray = instellingen.ophalen();
            // variabelen invullen
            aantalKolommen = Convert.ToInt32(instellingenArray[0]);
            aantalRijen = Convert.ToInt32(instellingenArray[1]);
            naamSpeler1 = _naamSpeler1;
            naamSpeler2 = _naamSpeler2;
            huidigeSpeler = _huidigeSpeler;
            // stel punten in voor spelers
            puntenWijzigen(0, 0);
            // vul bool of het een hervat spel betreft
            hoeveelComboSetting = Convert.ToInt32(instellingenArray[2]);
            aantalComboTeGaan = Convert.ToInt32(instellingenArray[2]);
            huidigeMultiplier = 1;
            thema = instellingenArray[3];
            initialiseerSpel();
        }

        /// <summary>
        /// Start nieuw spel met instellingen bestand en zonder herladen
        /// </summary>
        /// <param name="_naamSpeler1"></param>
        /// <param name="_naamSpeler2"></param>
        /// <param name="_locatieInstellingenBestand"></param>

        public Spel(string _naamSpeler1, string _naamSpeler2, string _locatieInstellingenBestand, string _locatieHighscoreBestand, string _locatieSavBestand)
        {
            InitializeComponent();
            DataContext = this;
            locatieHighscoreBestand = _locatieHighscoreBestand;
            locatieInstellingenBestand = _locatieInstellingenBestand;
            locatieSavBestand = _locatieSavBestand;
            Instellingen instellingen = new Instellingen(_locatieInstellingenBestand);
            string[] instellingenArray = instellingen.ophalen();
            // variabelen invullen
            aantalKolommen = Convert.ToInt32(instellingenArray[0]);
            aantalRijen = Convert.ToInt32(instellingenArray[1]);
            naamSpeler1 = _naamSpeler1;
            naamSpeler2 = _naamSpeler2;
            huidigeSpeler = 1;
            // stel punten in voor spelers
            puntenWijzigen(0, 0);
            // vul bool of het een hervat spel betreft
            hoeveelComboSetting = Convert.ToInt32(instellingenArray[2]);
            aantalComboTeGaan = Convert.ToInt32(instellingenArray[2]);
            huidigeMultiplier = 1;
            thema = instellingenArray[3];
            initialiseerSpel();
        }

        /// <summary>
        /// Start nieuw spel met instellingen bestand en met herladen
        /// </summary>
        /// <param name="_naamSpeler1"></param>
        /// <param name="_naamSpeler2"></param>
        /// <param name="_locatieInstellingenBestand"></param>
        /// <param name="_locatieSavBestand"></param>

        public Spel(bool _herladen, string _locatieInstellingenBestand, string _locatieHighScoreBestand, string _locatieSavBestand)
        {
            InitializeComponent();
            DataContext = this;
            spelHervatten = true;
            locatieSavBestand = _locatieSavBestand;
            locatieHighscoreBestand = _locatieHighScoreBestand;
            locatieInstellingenBestand = _locatieSavBestand;
            leesSavBestandUit();
            // variabelen invullen
            /// Volgorde: [0]Score speler 1, [1]Score speler 2, [2]Naam speler 1, [3]Naam speler 2, [4]huidige speler, [5]Rijen,
            /// [6]Kolommen, [7]Aantal sets instellingen, [8]Aantal kaarten over, [9]Huidige multiplier
            aantalKolommen = Convert.ToInt32(basisGegevens[6]);
            aantalRijen = Convert.ToInt32(basisGegevens[5]);
            hoeveelComboSetting = Convert.ToInt32(basisGegevens[7]);
            aantalComboTeGaan = Convert.ToInt32(basisGegevens[7]);
            huidigeMultiplier = Convert.ToInt32(basisGegevens[9]);
            aantalKaartjes = Convert.ToInt32(basisGegevens[8]);
            naamSpeler1 = basisGegevens[2];
            naamSpeler2 = basisGegevens[3];
            huidigeSpeler = Convert.ToInt32(basisGegevens[4]);
            puntenWijzigen(1, Convert.ToInt32(basisGegevens[0]));
            puntenWijzigen(2, Convert.ToInt32(basisGegevens[1]));
            thema = basisGegevens[10];
            initialiseerSpel();
        }

        private void initialiseerSpel()
        {
            aantalVakjes = aantalRijen * aantalKolommen - (aantalRijen * aantalKolommen % 2);
            if (aantalKaartjes < 1)
                aantalKaartjes = aantalVakjes;
            initialiseerGrid();
            genereerMemoryKaartjes();
        }

        private void leesSavBestandUit()
        {
            int status = 1;
            foreach (string line in File.ReadLines(locatieSavBestand, Encoding.UTF8))
            {
                // als lijn BEGIN OMGEDRAAIDE KAARTJES is bereikt zet status op true zodat het toevoegen aan lijst kan beginnen
                if (line == "EIND BASISINSTELLINGEN")
                {
                    status = 0;
                    continue;
                }
                if (line == "BEGIN OMGEDRAAIDE KAARTJES")
                {
                    status = 2;
                    continue;
                }
                if (line == "EIND OMGEDRAAIDE KAARTJES")
                {
                    status = 0;
                    continue;
                }
                if (line == "KAARTJES_POSITIE")
                {
                    status = 3;
                    continue;
                }
                if (line == "EIND KAARTJES_POSITIE")
                {
                    status = 0;
                    continue;
                }
                if (status == 1)
                {
                    basisGegevens.Add(line);
                    continue;
                }
                if (status == 2)
                {
                    omgedraaieKaartjes.Add(line);
                    continue;
                }
                if (status == 3)
                {
                    ImgLijstString.Add(line);
                }
            }
        }


        /// <summary>
        /// Genereer afbeeldingen lijst aan de hand van aantal kaartjes en thema
        /// </summary>
        /// <returns>Geeft lijst terug met ImageSources</returns>
        private List<ImageSource> genImgLijst()
        {
            List<ImageSource> _gen = new List<ImageSource>();
            List<ImageSource> _lijst = new List<ImageSource>();
            // als het een hervat spel betreft, haal opgeslagen lijst op en zet deze om in ImageSource's in _gen lijst
            if (spelHervatten)
            {
                foreach (string _kaartje in ImgLijstString)
                {
                    // maak een ImageSource aan genaamd bron, vul hier het imagepad in met het adbeeldingsnummer
                    ImageSource bron = new BitmapImage(new Uri(_kaartje, UriKind.Relative));
                    // voeg bron toe aan de lijst
                    _lijst.Add(bron);
                }
            }
            else
            // als het een nieuw spel betreft, genereer een compleet nieuwe lijst
            {
                for (int i = 1; i <= aantalVakjes / 2; i++)
                {
                    // maak een ImageSource aan genaamd bron, vul hier het imagepad in met het adbeeldingsnummer
                    ImageSource bron = new BitmapImage(new Uri("images/" + thema + "/" + i + ".png", UriKind.Relative));
                    // voeg bron 2x toe aan de lijst
                    _gen.Add(bron);
                    _gen.Add(bron);
                }
                // shuffle de lijst en voeg toe aan _lijst en aan _kaartjesLijst
                // Nieuwe random functie
                Random r = new Random();
                // Begint de index op nul 
                int randomIndex = 0;
                while (_gen.Count > 0)
                {
                    randomIndex = r.Next(0, _gen.Count); //Kies een willekeurig object uit de lijst
                    _lijst.Add(_gen[randomIndex]); //Voeg het toe aan de nieuwe lijst
                    ImgLijstString.Add(Convert.ToString(_gen[randomIndex]));
                    _gen.RemoveAt(randomIndex); //Verwijderd element om duplicaten te voorkomen
                }

            }
            return _lijst;
        }

        /// <summary>
        /// Genereer MemoryKaartjes
        /// </summary>
        public void genereerMemoryKaartjes()
        {
            // genereer random lijst met afbeeldingen
            List<ImageSource> _afbeeldingen = genImgLijst();
            // loop alle rijen af
            int rij = 0;
            int kolom = 0;
                // loop in betreffende rij alle kolommen af
                for (int i = 0; i < aantalVakjes; i++)
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
                kolom++;
                if (kolom == aantalKolommen)
                {
                    kolom = 0;
                    rij++;
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
                    if (aantalComboGoed == hoeveelComboSetting)
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
        private void resetSpelKnop(object sender, RoutedEventArgs e)
        {
            resetSpel();
                }

        private void resetSpel()
        {
            spelHervatten = false;
            // wis grid
            MemoryGrid.Children.Clear();
            // maak grid aan en genereer kaartjes
            ImgLijstString.Clear();
            omgedraaieKaartjes.Clear();
            // aantal open combo's op 0
            aantalComboGoed = 0;
            // reset aantal kaarten
            aantalKaartjes = aantalVakjes;
            // zet speler weer op 1
            huidigeSpeler = 1;
            aantalKlik = 0;
            // reset scores
            puntenWijzigen(0, 0);
            // reset multiplier
            huidigeMultiplier = 1;
            // reset combo
            aantalComboTeGaan = hoeveelComboSetting;
            // reset huidige speler
            huidigeSpeler = 1;
            // reset aantal klik
            aantalKlik = 0;
            // maak lijst open kaarten leeg
            openKaarten.Clear();
            initialiseerGrid();
            genereerMemoryKaartjes();
            MessageBox.Show("Spel is gereset");
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
        /// Sla spel op in SAV bestand
        /// Volgorde: [0]Score speler 1, [1]Score speler 2, [2]Naam speler 1, [3]Naam speler 2, [4]huidige speler, [5]Rijen, [6]Kolommen, [7]Aantal sets instellingen, [8]Aantal kaarten over, [9]Huidige multiplier
        /// Daarna lisjt met kaartjes positie, gevolgd door lijst met omgedraaide kaartjes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void opslaan(object sender, RoutedEventArgs e)
        {
            if (locatieSavBestand != null)
            {
                // schrijf alle waardes weg, op lijsten na
                File.WriteAllText(locatieSavBestand, string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}\n{9}\n{10}\n", huidigeScoreSpeler1, huidigeScoreSpeler2, naamSpeler1, naamSpeler2, huidigeSpeler, aantalRijen, aantalKolommen, hoeveelComboSetting, aantalKaartjes, _huidigeMultiplier, thema));
                File.AppendAllText(locatieSavBestand, "EIND BASISINSTELLINGEN\n");
                // genereer lijst met kaartjes positie
                File.AppendAllText(locatieSavBestand, "KAARTJES_POSITIE\n");
                foreach (String _kaartje in ImgLijstString)
                {
                    File.AppendAllText(locatieSavBestand, _kaartje + "\n");
                }
                File.AppendAllText(locatieSavBestand, "EIND KAARTJES_POSITIE\n");
                // genereer lijst met omgedraaide kaartjes
                File.AppendAllText(locatieSavBestand, "BEGIN OMGEDRAAIDE KAARTJES\n");
                foreach (String _kaartje in omgedraaieKaartjes)
                {
                    File.AppendAllText(locatieSavBestand, _kaartje + "\n");
                }
                File.AppendAllText(locatieSavBestand, "EIND OMGEDRAAIDE KAARTJES");
                MessageBox.Show("Spel is opgeslagen");
            } else
            {
                MessageBox.Show("Opslaan niet mogelijk door het ontbreken van het SAV bestand");
            }
        }

        private void highscoreWegschrijven()
        {
            Highscores highscores = new Highscores(locatieHighscoreBestand);
            Dictionary<string, int> _highScores = highscores.highscoreUitlezen();
            // controleer of speler 1 bestaat
            if (_highScores.ContainsKey(naamSpeler1))
            {
                // indien ja, controleer of nieuwe score hoger is dan reeds bekende score
                if (_highScores[naamSpeler1] < huidigeScoreSpeler1)
                    _highScores[naamSpeler1] = huidigeScoreSpeler1;
            }
            else
            {
                // indien naam nog niet bekend, voeg dan toe
                _highScores.Add(naamSpeler1, huidigeScoreSpeler1);
            }

            // controleer of speler 2 bestaat
            if (_highScores.ContainsKey(naamSpeler2))
            {
                // indien ja, controleer of nieuwe score hoger is dan reeds bekende score
                if (_highScores[naamSpeler2] < huidigeScoreSpeler2)
                    _highScores[naamSpeler2] = huidigeScoreSpeler2;
            }
            else
            {
                // indien naam nog niet bekend, voeg dan toe
                _highScores.Add(naamSpeler2, huidigeScoreSpeler2);
            }

            // tel aantal scores er in de lijst staan, maar tel nooit meer dan 10
            int _aantalScores = _highScores.Count() > 10 ? 10 : _highScores.Count();
            // maak een var aan met hierin de highscores uit _highScores Dic gesorteerd
            var _highScoresGesorteerd = from entry in _highScores orderby entry.Value descending select entry;
            // leeg het huidige highscore bestand
            File.WriteAllText(locatieHighscoreBestand, string.Empty);
            // schijf gesorteerde lijst weg naar highscore bestand
            foreach (KeyValuePair<string, Int32> entry in _highScoresGesorteerd)
            {
                File.AppendAllText(locatieHighscoreBestand, string.Format("{0}\n{1}\n", entry.Key, entry.Value));
                _aantalScores--;
                // als _aantalScore var leeg is (nooit meer dan 10) stop dan met uitvoeren zodat er max 10 highscores in de lijst staan
                if (_aantalScores == 0)
                    break;
            }
        }
    }
}
