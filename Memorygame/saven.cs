using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Memorygame
{
    class saven
    {
        string map = @"C:\MemoryGame\";
        string padSavBestand = "save.sav";
        string padHighscores = "memory.txt";

        /// <summary>
        /// Controleer of map bestaat en bestanden bestaan. Zo niet, probeer eerst aan te maken
        /// </summary>
        /// <returns>Bool of map bestaat</returns>
        public bool controleerMap()
        {
            if (!(Directory.Exists(map)))
            {
                try { Directory.CreateDirectory(map); }
                catch (Exception) { MessageBox.Show("Kan map niet aanmaken. Zorg ervoor dat de map " + map + " bestaat en toegankelijk is. Opslaan en highscores zijn niet beschikbaar"); return false; };
            }
            if (!(File.Exists(map + padSavBestand)))
            {
                try { File.Create(map + padSavBestand); }
                catch (Exception) { MessageBox.Show("Kan SAV bestand niet aanmaken. Zorg ervoor dat " + map + padSavBestand + " bestaat en toegankelijk is. Opslaan is niet beschikbaar"); return false; };
            }
            if (!(File.Exists(map + padHighscores)))
            {
                try { File.Create(map + padHighscores); }
                catch (Exception) { MessageBox.Show("Kan highscore bestand niet aanmaken. Zorg ervoor dat " + map + padHighscores + " bestaat en toegankelijk is. Opslaan is niet beschikbaar"); return false; };
            }
            return true;
        }

        /// <summary>
        /// Controleer of Sav bestand bestaat
        /// </summary>
        /// <returns>True of False</returns>
        public bool controleerSavBestandAanwezig()
        {
            return (File.Exists(map + padSavBestand));
        }

        /// <summary>
        /// Controleer of Sav bestand bestaat
        /// </summary>
        /// <returns>True of False</returns>
        public bool controleerHighscoresBestandAanwezig()
        {
            if (new FileInfo(map + padHighscores).Length == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Controleer of SAV bestand leeg is of is ingevuld
        /// </summary>
        /// <returns>True als ingevuld, false als leeg</returns>
        public bool controleerSav()
        {
            if (new FileInfo(map + padSavBestand).Length == 0)
                return false;
            return true;
        }

        public void resetSav()
        {
            if (controleerSav())
                File.WriteAllText(map + padSavBestand, string.Empty);
        }

        /// <summary>
        /// Sla spel op in SAV bestand
        /// </summary>
        /// <param name="_scoreSpeler1">Score speler 1</param>
        /// <param name="_scoreSpeler2">Score speler 2</param>
        /// <param name="_naamSpeler1">Naam speler 1</param>
        /// <param name="_naamSpeler2">Naam speler 2</param>
        /// <param name="_huidigeCombo">Huidige combo</param>
        /// <param name="_huidigeSpeler">Huidige speler (ID)</param>
        /// <param name="_statusKaartjes">Status kaartjes (positie)</param>
        /// <param name="_omgedraaideKaartjes">Omgedraaide kaartjes</param>
        public void savebestandWegscrijven(int _scoreSpeler1, int _scoreSpeler2, string _naamSpeler1, string _naamSpeler2, int _huidigeCombo, int _huidigeSpeler, List<String> _statusKaartjes, List<String> _omgedraaideKaartjes)
        {
                File.WriteAllText(map + padSavBestand, string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n", _scoreSpeler1, _scoreSpeler2, _naamSpeler1, _naamSpeler2, _huidigeCombo, _huidigeSpeler));
                File.AppendAllText(map + padSavBestand, "KAARTJES_POSITIE\n");
                foreach (String _kaartje in _statusKaartjes)
                {
                    File.AppendAllText(map + padSavBestand, _kaartje + "\n");
                }
                File.AppendAllText(map + padSavBestand, "EIND KAARTJES_POSITIE\nBEGIN OMGEDRAAIDE KAARTJES\n");
                foreach (String _kaartje in _omgedraaideKaartjes)
                {
                    File.AppendAllText(map + padSavBestand, _kaartje + "\n");
                }
                File.AppendAllText(map + padSavBestand, "EIND OMGEDRAAIDE KAARTJES");
            }

        /// <summary>
        /// Lees kaartjes lijst uit (positie) vanuit SAV bestan
        /// </summary>
        /// <returns>String lijst met kaartjes op volgorde</returns>
        public List<String> kaartjesLijst()
        {
            List<String> _kaartjesLijst = new List<String>();
            int status = 0;
            foreach (string line in File.ReadLines(map + padSavBestand, Encoding.UTF8))
            {
                if (line == "KAARTJES_POSITIE")
                {
                    status = 1;
                    continue;
                }
                if (line == "EIND KAARTJES_POSITIE")
                {
                    break;
                }
                if (status == 1)
                {
                    _kaartjesLijst.Add(line);
                }
            }
            return _kaartjesLijst;
        }

        /// <summary>
        /// Lees omgedraaide kaartjes uit vanuit SAV bestan
        /// </summary>
        /// <returns>Lijst met tags omgedraaide kaartjes</returns>
        public List<String> omgedraaideKaartjes()
        {
            List<String> omgedraaideKaartjes = new List<String>();
            int status = 0;
            foreach (string line in File.ReadLines(map + padSavBestand, Encoding.UTF8))
            {
                if (line == "BEGIN OMGEDRAAIDE KAARTJES")
                {
                    status = 1;
                    continue;
                }
                if (line == "EIND OMGEDRAAIDE KAARTJES")
                {
                    break;
                }
                if (status == 1)
                {
                    omgedraaideKaartjes.Add(line);
                }
            }
            return omgedraaideKaartjes;
        }

        /// <summary>
        /// Lees basisgegevens uit
        /// </summary>
        /// <returns>Array string met basisgegevens</returns>
        public string[] basisGegevens()
        {
            int aantalBasisRegels = 6;
            string[] lines = File.ReadAllLines(map + padSavBestand);
            string[] gegevens = new string[aantalBasisRegels];
            for (int i = 0; i < aantalBasisRegels; i++)
                gegevens[i] = lines[i];
            return gegevens;
        }
    }
}
