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
    /// Interaction logic for Spel.xaml
    /// </summary>
    public partial class Spel : Page
    {
        public Spel()
        {
            InitializeComponent();
        }

        private void startspel(object sender, RoutedEventArgs e)
        {
            Spel huidigspel = new Spel();
            this.Content = huidigspel;
        }
    }
}
