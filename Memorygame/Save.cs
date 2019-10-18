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
    class Save
    {
        string pad = @"c:\MemoryGame\opgeslagen_spel.SAV";
        string map = @"c:\MemoryGame";

        public bool controleerBestand()
        {
            return (File.Exists(pad));
        }
        public void Wegschrijven(int _scoreSpeler1, int _scoreSpeler2, string speler1, string speler2, int combo, List<String> statusKaartjes, List<String> omgedraaideKaartjes)
        {
            if (!(Directory.Exists(map)))
            {
               Directory.CreateDirectory(map);
            }
            if ((Directory.Exists(map)))
            {
                File.WriteAllText(pad, string.Format("{0}\n{1}\n{2}\n{3}\n{4}", _scoreSpeler1, _scoreSpeler2, speler1, speler2, combo));
                File.AppendAllText(pad, "\nKAARTJES_POSITIE\n");
                foreach (String kaartje in statusKaartjes)
                {
                    File.AppendAllText(pad, kaartje + "\n");
                }
                File.AppendAllText(pad, "EIND KAARTJES_POSITIE\n");
                File.AppendAllText(pad, "BEGIN OMGEDRAAIDE KAARTJES\n");
                foreach (String kaartje in omgedraaideKaartjes)
                {
                    File.AppendAllText(pad, kaartje + "\n");
                }
                File.AppendAllText(pad, "EIND OMGEDRAAIDE KAARTJES\n");
                MessageBox.Show("Spel opgeslagen!");
            } else
            {
                MessageBox.Show("Spel kan niet worden opgeslagen. Zorg ervoor dat de map C:\\MemoryGame bestaat");
            }
        }
        public List<String> gegevens_positieKaartjes()
        {
            List<String> positieKaartjes = new List<String>();
            int status = 0;
            foreach (string line in File.ReadLines(pad, Encoding.UTF8))
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
                    positieKaartjes.Add(line);
                }
            }
            return positieKaartjes;
        }

        public List<String> gegevens_omgedraaideKaartjes()
        {
            List<String> omgedraaideKaartjes = new List<String>();
            int status = 0;
            foreach (string line in File.ReadLines(pad, Encoding.UTF8))
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

        public string[] gegevens_basis()
        {
            int aantalBasisRegels = 5;
            string[] lines = File.ReadAllLines(pad);
            string[] gegevens = new string[aantalBasisRegels];
            for (int i = 0; i < aantalBasisRegels; i++)
            {
                gegevens[i] = lines[i];
            }
            return gegevens;
        }
    }
}
