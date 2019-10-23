﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for Instellingen.xaml
    /// </summary>
    public partial class Instellingen : Window, INotifyPropertyChanged
    {
        int breedte = 4;
        int lengte = 4;
        string _breedteLengte;
        int _aantalSets;
        string _aantalSetsString;
        int _turnTimeSeconden;
        string _turnTimeString;
        int _turnTimeComboSeconden;
        string _turnTimeComboString;
        public Instellingen()
        {
            saven ophalen = new saven();
            InitializeComponent();
            DataContext = this;
            if (ophalen.controleerInstellingenBestand())
            {
                int[] _instellingen = ophalen.instelingenLezen();
                changeBreedteLengte(_instellingen[0], _instellingen[1]);
                aantalSets = _instellingen[2];
                turnTimeSeconden = _instellingen[3];
                turnTimeComboSeconden = _instellingen[4];
            } else
            {
                changeBreedteLengte(4, 4);
                aantalSets = 1;
                turnTimeSeconden = 10;
                turnTimeComboSeconden = 2;
            }
            
        }

        public string breedteLengte
        {
            get { return _breedteLengte; }
            set
            {
                _breedteLengte = value;
                PropertyGewijzigd();
            }
        }

        public string aantalSetsString
        {
            get { return _aantalSetsString; }
            set
            {
                _aantalSetsString = value;
                PropertyGewijzigd();
            }
        }

        public int aantalSets
        {
            get { return _aantalSets; }
            set
            {
                if (value < 1)
                    value = 1;
                if (value > (breedte * lengte) / 2)
                {
                    value = (breedte * lengte) / 2;
                    MessageBox.Show("Je hebt een grid van " + breedteLengte + ". Hierin zijn maximaal " + Convert.ToString((breedte * lengte) / 2) + " combinatie's mogelijk.");
                }
                _aantalSets = value;
                PropertyGewijzigd();
                aantalSetsString = Convert.ToString(value) + " paar";
            }
        }

        public string turnTimerString
        {
            get { return _turnTimeString; }
            set
            {
                _turnTimeString = value;
                PropertyGewijzigd();
            }
        }

        public int turnTimeSeconden
        {
            get { return _turnTimeSeconden; }
            set
            {
                if (value < 1)
                    value = 1;
                _turnTimeSeconden = value;
                PropertyGewijzigd();
                turnTimerString = Convert.ToString(_turnTimeSeconden) + " seconden";
            }
        }

        public string turnTimerComboString
        {
            get { return _turnTimeComboString; }
            set
            {
                _turnTimeComboString = value;
                PropertyGewijzigd();
            }
        }

        public int turnTimeComboSeconden
        {
            get { return _turnTimeComboSeconden; }
            set
            {
                if (value < 0)
                    value = 0;
                if (value > turnTimeSeconden - 1)
                    value = turnTimeSeconden - 1;
                _turnTimeComboSeconden = value;
                PropertyGewijzigd();
                turnTimerComboString = Convert.ToString(_turnTimeComboSeconden) + " seconden eraf per goede combo";
            }
        }

        private string[] instellingenOphalen()
        {
            string[] _instellingen = new string[5];
            return _instellingen;
        }

        private void Opslaan(object sender, RoutedEventArgs e)
        {
            saven opslaan = new saven();
            if (opslaan.controleerInstellingenAanweig()) {
                opslaan.instellingenWegschrijven(breedte, lengte, aantalSets, turnTimeSeconden, turnTimeComboSeconden);
                this.Close();
            }
            
            else
                MessageBox.Show("Instellingen kunnen niet worden opgeslagen door het ontbreken van het instellinen bestand");
        }

        private void changeBreedteLengte(int _breedte, int _lengte)
        {
            breedte = _breedte;
            lengte = _lengte;
            breedteLengte = Convert.ToString(breedte) + " * " + Convert.ToString(lengte);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Update property als content in variabele is veranderd
        /// </summary>
        /// <param name="propertyName">De property met een nieuwe waarde</param>
        protected void PropertyGewijzigd([CallerMemberName]string propertyName = null)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {

                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);

            }
        }

        private void setKlik4(object sender, RoutedEventArgs e)
        {
            changeBreedteLengte(4, 4);
        }
        private void setKlik5(object sender, RoutedEventArgs e)
        {
            changeBreedteLengte(5, 5);
        }

        private void setKlik6(object sender, RoutedEventArgs e)
        {
            changeBreedteLengte(6, 6);
        }

        private void pasSecondenAanKlik(object sender, RoutedEventArgs e)
        {
            turnTimerString = Convert.ToString(turnTimeSeconden) + " seconden";
        }
    }
}