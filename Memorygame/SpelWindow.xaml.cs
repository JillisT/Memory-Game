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
        public SpelWindow()
        {
            InitializeComponent();
            DataContext = this;
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
            Spel spel = new Spel(Speler1, Speler2);
            this.Content = spel;
        }
    }
}
