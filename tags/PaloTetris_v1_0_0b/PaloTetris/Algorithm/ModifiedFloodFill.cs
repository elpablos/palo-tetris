using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaloTetris;

namespace PaloTetris
{
    /// <summary>
    /// Trida, ktera se umoznuje pouziti zjenodnoduseneho algoritmu FloodFill
    /// pro zjisteni poctu "zakrytych" policek.
    /// </summary>
    public class ModifiedFloodFill
    {
        #region Properties

        public int[,] Field { get; private set; }

        public int ReplacementColor { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Konstruktor.
        /// </summary>
        /// <param name="game"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public ModifiedFloodFill(ITetrisGame game, int x, int y)
        {
            Field = (int[,])game.Board.Clone();

            for (int i = 0; i < 16; i++)
            {
                // pokud policko je neobarvene a neni mimo rozsah, tak obarvime
                if (game.ActivePiece.Pieces[i] != 0 && (y + i / 4) < game.Height && (x + i % 4) < game.Width)
                {
                    Field[y + i / 4, x + i % 4] = game.ActivePiece.Pieces[i];
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Metoda, ktera obarvi policka.
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="targetColor"></param>
        /// <param name="replacementColor"></param>
        public void ColorField(int startRow, int x, int y, int targetColor, int replacementColor)
        {
            ReplacementColor = replacementColor;
            // pokud je mimo rozsah
            if (x < 0 || x >= Field.GetLength(1) || y < 0 || y >= Field.GetLength(0))
            {
                // Console.WriteLine("FF-out-of-range:{0}/{1}", x, y);
                return;
            }
            // pokud pole neni cilem
            if (Field[y, x] != targetColor) return;
            // obarvujeme
            Field[y, x] = replacementColor;
            // posilame obarvit i jine
            //floodFill(x - 1, y, targetColor, replacementColor);
            if (y == startRow)
                ColorField(startRow, x + 1, y, targetColor, replacementColor);
            //floodFill(x, y - 1, targetColor, replacementColor);
            ColorField(startRow, x, y + 1, targetColor, replacementColor);
        }

        /// <summary>
        /// Metoda vracejici pocet dir.
        /// </summary>
        /// <returns></returns>
        public int GetCoveredHoles()
        {
            int i = 0;
            int count = 0;
            while (i < Field.Length)
            {
                if (Field[i / Field.GetLength(1), i % Field.GetLength(1)] == 0) count++;
                i++;
            }

            return count;
        }

        /// <summary>
        /// Metoda vracejici pocet moznych kompletnich radku.
        /// </summary>
        /// <returns></returns>
        public int GetNumberOfFullLines()
        {
            int ret = 0;
            for (int j = 0; j < Field.GetLength(0); j++)
            {
                ret += IsCompleteLine(j) ? 1 : 0;
            }
            return ret;
        }

        /// <summary>
        /// Metoda vraci nejvetsi vysku bodu v hernim poli.
        /// Pozn. Vyska je cislovana od nuly do max height!
        /// </summary>
        /// <returns>cislo, reprezentujici vysku nejvyssiho bodu</returns>
        public int GetHigherPoint()
        {
            for (int j = 0; j < Field.GetLength(0); j++)
            {
                for (int i = 0; i < Field.GetLength(1); i++)
                {
                    if (Field[j, i] != 0 && (Field[j, i] != ReplacementColor)) return j;
                }
            }
            return 0;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Pomocna metoda, ktera vraci true, pokud je radka "kompletni".
        /// Tedy lze ji shodit.
        /// </summary>
        /// <param name="line">cislo radky</param>
        /// <returns>true, pokud je radka kompletni</returns>
        private bool IsCompleteLine(int line)
        {
            for (int i = 0; i < Field.GetLength(1); i++)
            {
                if (Field[line, i] == 0 || (Field[line, i] == ReplacementColor)) return false;
            }
            return false;
        }

        #endregion
    }
}
