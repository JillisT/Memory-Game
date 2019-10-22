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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Memorygame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool savAanwezig;
        bool highscoresAanwezig;
        public MainWindow()
        {
            InitializeComponent();
            saven controleerMap = new saven();
            // controleer op de MemoryGame map bestaat, indien niet dan proberen map aan te maken. Indien het niet lukt, geef foutmelding
            if((controleerMap.controleerMap()))
            {
                savAanwezig = controleerMap.controleerSav();
                highscoresAanwezig = controleerMap.controleerHighscoresBestand();
            }
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
                SpelWindow spelwindow = new SpelWindow(true);
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
                MessageBoxResult m = MessageBox.Show("Wil je het opgeslagen spel verder spelen? Indien nee, dan wordt het voorgaande spel gewist", "Opgeslagen bestand gevonden.", MessageBoxButton.YesNo);
                if (m == MessageBoxResult.Yes)
                {
                    SpelWindow spelwindow = new SpelWindow(true);
                    spelwindow.Show();
                }
                else if (m == MessageBoxResult.No)
                {
                    SpelWindow spelwindow = new SpelWindow(false);
                    spelwindow.Show();
                }
            } else
            // geem sav aanwezig, niet spel starten
            {
                SpelWindow spelwindow = new SpelWindow(false);
                spelwindow.Show();
            }
            this.Hide();

        }
        private void Button_Settings(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Highscores(object sender, RoutedEventArgs e)
        {
            if (!(highscoresAanwezig))
                MessageBox.Show("Highscores zijn niet beschikbaar");
            Highscores highscores = new Highscores();
            highscores.Show();
        }
    }
}
