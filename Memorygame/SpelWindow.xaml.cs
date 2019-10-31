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
using System.Windows.Shapes;

namespace Memorygame
{
    /// <summary>
    /// Interaction logic for SpelWindow.xaml
    /// </summary>
    public partial class SpelWindow : Window
    {
        bool mapAanwezig = false;
        string locatieSavBestand;
        string locatieInstellingenBestand;
        string locatieHighScoreBestand;
        /// <summary>
        /// Spel starten
        /// </summary>
        /// <param name="_spelHervatten">Spel herstarten? Geef TRUE mee</param>
        public SpelWindow(bool _spelhervatten, string _locatieInstellingenBestand, string _locatieHighScoreBestand, string _locatieSavBestand)
        {
            InitializeComponent();
            DataContext = this;
                Spel spel = new Spel(true, _locatieInstellingenBestand, _locatieHighScoreBestand, _locatieSavBestand);
                this.Content = spel;
        }

        /// <summary>
        /// SpelWindow zonder bestandenmap
        /// </summary>
        public SpelWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        /// <summary>
        /// SpelWindow met bestandenmap
        /// </summary>
        public SpelWindow(string _locatieInstellingenBestand, string _locatieHighScoreBestand, string _locatieSavBestand)
        {
            InitializeComponent();
            DataContext = this;
            locatieInstellingenBestand = _locatieInstellingenBestand;
            locatieHighScoreBestand = _locatieHighScoreBestand;
            locatieSavBestand = _locatieSavBestand;
            mapAanwezig = true;
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
            if (mapAanwezig)
            {
                // start een nieuw spel
                Spel spel = new Spel(Speler1, Speler2, locatieInstellingenBestand, locatieHighScoreBestand, locatieSavBestand);
                this.Content = spel;
            } else
            {
                Spel spel = new Spel(Speler1, Speler2);
                this.Content = spel;
            }
            
        }

        private void schermSluiten (object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
        }
    }
}
