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
        public MainWindow()
        {
            InitializeComponent();
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
            
        }

        private void Button_New_Game(object sender, RoutedEventArgs e)
        {
            // maak huidigspel aan met Spel class
            Spel huidigspel = new Spel();
            // laat huidigspel zien in huidig scherm
            this.Content = huidigspel;
        }

        private void Button_Settings(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Highscores(object sender, RoutedEventArgs e)
        {

        }
    }
}
