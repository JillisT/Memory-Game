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
        string padInstellingen = "instellingen.txt";

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
            if (!(File.Exists(map + padInstellingen)))
            {
                try { File.Create(map + padInstellingen); }
                catch (Exception) { MessageBox.Show("Kan instelingen bestand niet aanmaken. Zorg ervoor dat " + map + padInstellingen + " bestaat en toegankelijk is. Opslaan is niet beschikbaar"); return false; };
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
        /// Controleer of highscore bestand is ingevuld
        /// </summary>
        /// <returns>True of False</returns>
        public bool controleerHighscoresBestand()
        {
            if (new FileInfo(map + padHighscores).Length == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Controleer of highscore bestand bestaat
        /// </summary>
        /// <returns>True of False</returns>
        public bool controleerHighscoresBestandAanwezig()
        {
            return (File.Exists(map + padHighscores));
        }

        /// <summary>
        /// Controleer of instellingen bestand is ingevuld
        /// </summary>
        /// <returns>True of False</returns>
        public bool controleerInstellingenBestand()
        {
            if (new FileInfo(map + padInstellingen).Length == 0)
                return false;
            return true;
        }

        /// <summary>
        /// Controleer of instellingen bestand bestaat
        /// </summary>
        /// <returns>True of False</returns>
        public bool controleerInstellingenAanweig()
        {
            return (File.Exists(map + padInstellingen));
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
        /// Sla highscores op
        /// </summary>
        /// <param name="_scoreSpeler1">Score speler 1</param>
        /// <param name="_scoreSpeler2">Score speler 2</param>
        /// <param name="_naamSpeler1">Naam speler 1</param>
        /// <param name="_naamSpeler2">Naam speler 2</param>
        public void highscoreWegscrijven(int _scoreSpeler1, int _scoreSpeler2, string _naamSpeler1, string _naamSpeler2)
        {
            Dictionary<string, int> _highScores = highscoreUitlezen();
            if (_highScores.ContainsKey(_naamSpeler1))
                _highScores[_naamSpeler1] = _scoreSpeler1;
            else
                _highScores.Add(_naamSpeler1, _scoreSpeler1);
            if (_highScores.ContainsKey(_naamSpeler2))
                _highScores[_naamSpeler2] = _scoreSpeler2;
            else
                _highScores.Add(_naamSpeler2, _scoreSpeler2);
            int _aantalScores = _highScores.Count() > 10 ? 10 : _highScores.Count();
            var _highScoresGesorteerd = from entry in _highScores orderby entry.Value descending select entry;
            File.WriteAllText(map + padHighscores, string.Empty);
            foreach (KeyValuePair<string, Int32> entry in _highScoresGesorteerd)
            {
                File.AppendAllText(map + padHighscores, string.Format("{0}\n{1}\n", entry.Key, entry.Value));
                _aantalScores--;
                if (_aantalScores == 0)
                    break;
            }

        }

        /// <summary>
        /// Lees Highscores uit
        /// </summary>
        /// <returns>Dictionary met naam en score</returns>
        public Dictionary<string, int> highscoreUitlezen()
        {
            string _naam = "";
            Dictionary<string, int> _highScores = new Dictionary<string, int>();
            foreach (string line in File.ReadLines(map + padHighscores, Encoding.UTF8))
            {
                if (_naam == "")
                {
                    _naam = line;
                    continue;
                }
                else
                {
                    _highScores.Add(_naam, Convert.ToInt32(line));
                    _naam = "";
                }
            }
            return _highScores;
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
        public void instellingenWegschrijven(int _gridL, int _gridB, int _aantalCombo, int _turnTimer, int _turnTimerCombo)
        {
            File.WriteAllText(map + padInstellingen, string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n", _gridL, _gridB, _aantalCombo, _turnTimer, _turnTimerCombo));
        }

        /// <summary>
        /// Lees basisgegevens uit
        /// </summary>
        /// <returns>Array string met basisgegevens</returns>
        public int[] instelingenLezen()
        {
            int instellingen = 5;
            string[] lines = File.ReadAllLines(map + padInstellingen);
            int[] gegevens = new int[instellingen];
            for (int i = 0; i < instellingen; i++)
                gegevens[i] = Convert.ToInt32(lines[i]);
            return gegevens;
        }

    }
}
