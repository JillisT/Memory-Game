﻿using System;
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
            naaminvoer.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            spelstart.Visibility = Visibility.Hidden;
            naaminvoer.Visibility = Visibility.Visible;
            
            //this.Content = huidigspel;
        }

        private void terug_click(object sender, RoutedEventArgs e)
        {
            spelstart.Visibility = Visibility.Visible;
            naaminvoer.Visibility = Visibility.Hidden;
        }

        private void startspel_click(object sender, RoutedEventArgs e)
        {
            Spel huidigspel = new Spel();
            this.Content = huidigspel;
        }
    }
}
