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
    /// Interaction logic for Highscores.xaml
    /// </summary>
    public partial class Highscores : Window
    {
        public Highscores()
        {
            InitializeComponent();
            saven scores = new saven();
            Dictionary<String, Int32> _scores = scores.highscoreUitlezen();
            int margin = 0;
            int fontsize = 30;
            foreach (KeyValuePair<String, Int32> _item in _scores)
            {
                Label _label = new Label();
                _label.Content = _item.Key + " - " + _item.Value;
                _label.Margin = new Thickness(0,margin,0,0);
                _label.Foreground = new SolidColorBrush(Colors.White);
                _label.FontSize = fontsize;
                _label.HorizontalContentAlignment = HorizontalAlignment.Center;
                margin += 40;
                fontsize -= 2;
                lijst.Children.Add(_label);
            }
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
