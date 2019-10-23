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
            // haal scores op en sla op in dictionary _scores
            saven scores = new saven();
            Dictionary<String, Int32> _scores = scores.highscoreUitlezen();
            int marginboven = 0;
            int positie = 0;
            int laatsteScrore = -1;
            // loop dic _scores af
            foreach (KeyValuePair<String, Int32> _item in _scores)
            {
                // als score voorgaande speler niet gelijk is, dan 1 positie verhogen.
                if (_item.Value != laatsteScrore)
                    positie++;
                laatsteScrore = _item.Value;
                Label _label = new Label();
                Label _label2 = new Label();
                _label.Content = _item.Key;
                _label2.Content = _item.Value;
                _label.Margin = new Thickness(40, marginboven, 0,0);
                _label2.Margin = new Thickness(550, marginboven, 0, 0);
                _label.Width = positie < 4 ? 400 : 500;
                _label2.Width = 100;
                // pas kleur toe aan de hand van positie
                _label.Foreground = positie == 1 ? new SolidColorBrush(Colors.Gold) : positie == 2 ? new SolidColorBrush(Colors.Silver) : positie == 3 ? new SolidColorBrush(Colors.SaddleBrown) : new SolidColorBrush(Colors.White);
                _label2.Foreground = positie == 1 ? new SolidColorBrush(Colors.Gold) : positie == 2 ? new SolidColorBrush(Colors.Silver) : positie == 3 ? new SolidColorBrush(Colors.SaddleBrown) : new SolidColorBrush(Colors.White);
                _label.FontSize = 30;
                _label2.FontSize = 30;
                _label.HorizontalAlignment = HorizontalAlignment.Left;
                _label2.HorizontalContentAlignment = HorizontalAlignment.Right;
                if (positie < 4)
                { 
                    // voor positie 1,2 en 3 pas medaille toe
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri("/medaille/" + positie + ".png", UriKind.Relative));
                    image.Height = 40;
                    image.Margin = new Thickness(0, marginboven, 0, 0);
                    image.HorizontalAlignment = HorizontalAlignment.Left;
                    image.VerticalAlignment = VerticalAlignment.Top;
                    lijst.Children.Add(image);
                }
                lijst.Children.Add(_label);
                lijst.Children.Add(_label2);
                marginboven += 40;
            }
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
