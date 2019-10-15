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

namespace Memorygame
{
    /// <summary>
    /// Interaction logic for Spel.xaml
    /// </summary>
    public partial class Spel : Page
    {
        // variabelen voor het aantal de initializeren rijen en kolommen
        private const int aantalRijen = 4;
        private const int aantalKolommen= 4;
        int aantalKlik = 0;
        Image openKaart1;
        Image openKaart2;
        string bronKaart1;
        string bronKaart2;
        int huidigeSpeler = 1;
        public Spel()
        {
            
            InitializeComponent();
            // roep initialiseerGrid methode aan om Grid aan te maken
            initialiseerGrid();
            // voeg titel toe
            voegTitelToe();
            // voeg kaartjes toe in grid elementen
            genereerMemoryKaartjes();
            // genereer scoreTelling
            scoreTeller();
        }

        private void scoreTeller()
        {
            TextBlock speler = new TextBlock();
            Grid.SetColumn(speler, aantalKolommen);
            Grid.SetRow(speler, 1);
            speler.HorizontalAlignment = HorizontalAlignment.Center;
            speler.FontSize = 30;
            MemoryGrid.Children.Add(speler);
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
        /// Voeg label toe aan laatste kolom op 1e rij
        /// </summary>
        private void voegTitelToe()
        {
            Label titel = new Label();
            titel.Content = "MemoryGame - veel succes!";
            Grid.SetColumn(titel, aantalKolommen);
            titel.HorizontalAlignment = HorizontalAlignment.Center;
            titel.FontSize = 30;
            MemoryGrid.Children.Add(titel);
        }
        /// <summary>
        /// Genereer MemoryKaartjes
        /// </summary>
        private void genereerMemoryKaartjes()
        {
            // genereer random lijst met afbeeldingen
            List <ImageSource> achterkantImg = genImgLijst();
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
                    MessageBox.Show("Lucky");
                    aantalKlik = 0;
                }
            }
            if (aantalKlik == 3)
            {
                // na een 3e klik beide kaartjes weer omdraaien en van speler wisselen
                aantalKlik = 0;
                openKaart1.Source = new BitmapImage(new Uri("Vraagteken.png", UriKind.Relative));
                openKaart2.Source = new BitmapImage(new Uri("Vraagteken.png", UriKind.Relative));
                // wissel van speler
                if (huidigeSpeler == 1)
                {
                    huidigeSpeler = 2;
                    // doe iets voor status
                } else
                {
                    huidigeSpeler = 1;
                    // doe iets voor status
                }
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
            }
            // voeg kolommen toe aan memorygrid
            for (int i = 0; i < aantalKolommen; i++)
            {
                MemoryGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            // voeg 1 extra kolum toe voor timer etc
            MemoryGrid.ColumnDefinitions.Add(new ColumnDefinition());

        }




        // deze wordt niet gebruikt, maar even laten staan...
        private void startspel(object sender, RoutedEventArgs e)
        {
            Spel huidigspel = new Spel();
            this.Content = huidigspel;
        }
    }
}
