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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Memorygame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool bestandenAanwezig;
        bool highscoreAanwezig;
        bool savAanwezig;
        string map = @"C:\MemoryGame\";
        string padSavBestand = "save.sav";
        string padHighscores = "memory.txt";
        string padInstellingen = "instellingen.txt";
        string padSavBestandVolledig;
        string padInstellingenVolledig;
        string padHighScoreVolledig;
        public MainWindow()
        {
            InitializeComponent();
            bestandenAanwezig = controleerMapenBestanden();
            if (bestandenAanwezig == true)
            {
                highscoreAanwezig = controleerHighscoresBestand();
                savAanwezig = controleerSav();
                padSavBestandVolledig = map + padSavBestand;
                padInstellingenVolledig = map + padInstellingen;
                padHighScoreVolledig = map + padHighscores;
            }
            
        }



        /// <summary>
        /// Controleer of map bestaat en bestanden bestaan. Zo niet, probeer eerst aan te maken
        /// </summary>
        /// <returns>Bool of map en bestanden bestaan</returns>
        public bool controleerMapenBestanden()
        {
            if (!(Directory.Exists(map)))
            {
                try { Directory.CreateDirectory(map); }
                catch (Exception) { MessageBox.Show("Kan map niet aanmaken. Zorg ervoor dat de map " + map + " bestaat en toegankelijk is. Opslaan en highscores zijn niet beschikbaar"); return false; };
            }
            if (!(File.Exists(map + padSavBestand)))
            {
                try { File.Create(map + padSavBestand); }
                catch (Exception) { MessageBox.Show("Kan SAV bestand niet aanmaken. Zorg ervoor dat " + map + padSavBestand + " bestaat en toegankelijk is. Opslaan is niet beschikbaar"); return false; };
            }
            if (!(File.Exists(map + padHighscores)))
            {
                try { File.Create(map + padHighscores); }
                catch (Exception) { MessageBox.Show("Kan highscore bestand niet aanmaken. Zorg ervoor dat " + map + padHighscores + " bestaat en toegankelijk is. Opslaan is niet beschikbaar"); return false; };
            }
            if (!(File.Exists(map + padInstellingen)))
            {
                try { File.Create(map + padInstellingen); }
                catch (Exception) { MessageBox.Show("Kan instelingen bestand niet aanmaken. Zorg ervoor dat " + map + padInstellingen + " bestaat en toegankelijk is. Opslaan is niet beschikbaar"); return false; };
            }
            return true;
        }

        /// <summary>
        /// Controleer of highscore bestand is ingevuld
        /// </summary>
        /// <returns>True als scores aanwezig zijn, false als er nog geen scores zijn</returns>
        public bool controleerHighscoresBestand()
        {
            if (new FileInfo(map + padHighscores).Length == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Controleer of SAV bestand leeg is of is ingevuld
        /// </summary>
        /// <returns>True als ingevuld, false als leeg</returns>
        public bool controleerSav()
        {
            if (new FileInfo(map + padSavBestand).Length == 0)
                return false;
            return true;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Continue(object sender, RoutedEventArgs e)
        {
            if (savAanwezig)
            {
                SpelWindow spelwindow = new SpelWindow(true, padInstellingenVolledig, padHighScoreVolledig, padSavBestandVolledig);
                spelwindow.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Geen opgeslagen spel gevonden!");
            }


        }

        private void Button_New_Game(object sender, RoutedEventArgs e)
        {
            if (savAanwezig)
                // sav aanwezig, vragen om spel te herstarten of hervatten
            {
                MessageBoxResult m = MessageBox.Show("Wil je een nieuw spel starten? Het opgeslagen spel wordt gewist", "Opgeslagen bestand gevonden.", MessageBoxButton.YesNo);
                if (m == MessageBoxResult.Yes)
                {
                    File.WriteAllText(map + padSavBestand, string.Empty);
                    SpelWindow spelwindow = new SpelWindow(padInstellingenVolledig, padHighScoreVolledig, padSavBestandVolledig);
                    spelwindow.Show();
                }
                else if (m == MessageBoxResult.No)
                {
                    SpelWindow spelwindow = new SpelWindow(true, padInstellingenVolledig, padHighScoreVolledig, padSavBestandVolledig);
                    spelwindow.Show();
                }
            } else
            // geen sav aanwezig, nieuw spel starten
            {
                if (bestandenAanwezig)
                {
                    SpelWindow spelwindow = new SpelWindow(padInstellingenVolledig, padHighScoreVolledig, padSavBestandVolledig);
                    spelwindow.Show();
                } else
                {
                    SpelWindow spelwindow = new SpelWindow();
                    spelwindow.Show();
                }
                
            }
            this.Hide();

        }
        private void Button_Settings(object sender, RoutedEventArgs e)
        {
            if (bestandenAanwezig)
            {
                try { File.ReadAllLines(map + padInstellingen); }
                catch (Exception) { MessageBox.Show("Het instellingenbestand is niet beschikbaar. Probeer het over enkele seconden opnieuw of probeer het spel opnieuw te starten."); return; };
                Instellingen instellingen = new Instellingen(map + padInstellingen);
                instellingen.Show();
            } else
            {
                MessageBox.Show("Instellingen zijn niet beschikbaar doordat de Memory map met het instellingenbestand mist");
            }
            
        }

        private void Button_Highscores(object sender, RoutedEventArgs e)
        {
            if (highscoreAanwezig)
            {
                
                Highscores highscores = new Highscores(map + padHighscores);
                highscores.Show();
            } else
            {
                MessageBox.Show("Highscores zijn niet aanwezig");
            }
            
        }

        private void Help_Click(object sender, MouseButtonEventArgs e)
        {
            Help help = new Help();
            help.Show();
        }
    }
}
