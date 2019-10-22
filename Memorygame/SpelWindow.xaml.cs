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
        /// <summary>
        /// Spel starten
        /// </summary>
        /// <param name="_spelHervatten">Spel herstarten? Geef TRUE mee</param>
        public SpelWindow(bool _spelHervatten)
        {
            InitializeComponent();
            DataContext = this;
            if (_spelHervatten)
            {
                // er moet een spel worden hervat. Lees basisgegevens uit, start spel direct door met parameters 
                saven ophalen = new saven();
                string[] _basisGegevens = ophalen.basisGegevens();
                Spel spel = new Spel(Convert.ToInt32(_basisGegevens[0]), Convert.ToInt32(_basisGegevens[1]), _basisGegevens[2], _basisGegevens[3], Convert.ToInt32(_basisGegevens[4]), Convert.ToInt32(_basisGegevens[5]), true);
                this.Content = spel;
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
            // start een nieuw spel
            Spel spel = new Spel(0, 0, _Speler1, _Speler1, 1, 1, false);
            this.Content = spel;
        }

        private void schermSluiten (object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow mainwindow = new MainWindow();
            mainwindow.Show();
        }
    }
}
