using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleTetris
{
    /// <summary>
    /// Umela inteligence pro Tetris.
    /// </summary>
    public class Ai
    {
        #region Fields

        bool next;
        int[,] board;
        int px, py, pr, piece;
        
        #endregion // Fields
        
        #region Properties

        /// <summary>
        /// Reference na hru.
        /// </summary>
        public Tetris Tetris { get; private set; }

        #endregion // Properties

        #region Constructor

        /// <summary>
        /// Konstruktor, predavajici reference na hru tetris.
        /// </summary>
        /// <param name="tetris"></param>
        public Ai(Tetris tetris)
        {
            Tetris = tetris;
            board = Tetris.Board;
        }

        #endregion // Constructor

        #region Public methods

        public void Run()
        {
            int maxScore = Int32.MinValue;
            int maxpx = 0;
            int maxpr = 0;

            piece = Tetris.ActivePiece;

            // pro vsechny x od -4 do maxX +4 
            for (px = -4; px < board.GetLength(1) + 4 && !next; px++)
            {
                // pro vsechny rotace pro dany prvek
                for (pr = 0; pr < Tetris.pieces[piece].Length && !next; pr++)
                {
                    // pokud nelze prvek umistit, tak preskoc cykly
                    if (!Tetris.IsSpaceFor(px, 0, Tetris.pieces[piece][pr]))
                    {
                        continue;
                    }
                    // vynulujeme posuv po Y
                    py = 0;

                    // inkrementuj Y dokud lze polozit prvek
                    while (!Tetris.CanParkPiece(px, py, Tetris.pieces[piece][pr]))
                        py++;

                    // spocitej  ohodnoceni prvku na dane pozici
                    int n = Eveluate();
                    // pokud je ohodnoceni vetsi nez MAX skore, tak jej uloz
                    if (n > maxScore)
                    {
                        maxScore = n;
                        maxpx = px;
                        maxpr = pr;
                    }
                }
            }

            // orotuj, dokud se nedostanes na maximalne ohodnocenou rotaci
            for (int i = 0; i < 4 && Tetris.ActiveR != maxpr; i++)
            {
                Tetris.Rotate();
            }

            // spocitej rozdil mezi maximalne ohodnocenym X a aktivnim X
            int displace = maxpx - Tetris.ActiveX;

            // pokud je rozdil vetsi nez 0, tak posouvej doleva, dokud se to nesrovna
            while (displace > 0)
            {
                if (!Tetris.Right())
                    Console.WriteLine("Can't move to {0} stack on {1}", maxpx, Tetris.ActiveX);
                displace--;
            }

            // pokud je rozdil mensi nez 0, tak posouvej doprava, dokud se to nesrovna
            while (displace < 0)
            {
                if (!Tetris.Left())
                    Console.WriteLine("Can't move to {0} stack on {1}", maxpx, Tetris.ActiveX);
                displace++;
            }

            // pokud neni X stejne jako maximalni X, tak problem
            if (Tetris.ActiveX != maxpx)
            {
                Console.WriteLine("Uh oh");
            }
            // jinak dropni prvek
            else
            {
                Console.WriteLine("Drop");
                Tetris.Drop();
            }
        }

        #endregion // Public methods

        #region Private methods

        /// <summary>
        /// Ohodnoceni boardu pro danou konfiguraci.
        /// </summary>
        /// <returns></returns>
        private int Eveluate()
        {
            int ret = 0;

            ret += IronMill();
            ret -= CountCoveredHoles() * 10000;

            Console.WriteLine("Evaluating.. {0}", ret);
            return ret;
        }

        /// <summary>
        /// Pocitani schovanych der.
        /// </summary>
        /// <returns></returns>
        private int CountCoveredHoles()
        {
            int holes = 0;
            // pro vsechny body boardu
            for (int x = 0; x < board.GetLength(1); x++)
            {
                bool swap = false;
                for (int y = 0; y < board.GetLength(0); y++)
                {
                    // pokud je to nenulovy bod, tak nastav swap na true
                    if (Get(x, y)) swap = true;
                    else
                    {
                        // pokud je dira a swap je na true, tak inkrementuj diry
                        if (swap) holes++;
                        // nastav opet swap na false
                        swap = false;
                    }
                }
            }
            // pocet dir
            Console.WriteLine("Holes: " + holes);
            return holes;
        }

        /// <summary>
        /// Pocitani y^3 pro vsechny body.
        /// </summary>
        /// <returns>bodova hodnota pole</returns>
        private int IronMill()
        {
            int iron = 0;
            // pro vsechny body boardu
            for (int y = 0; y < board.GetLength(0); y++)
            {
                for (int x = 0; x < board.GetLength(1); x++)
                {
                    // pokud je bod nenulovy, tak pricti y^3
                    if (Get(x, y)) iron += y * y * y;
                }
            }
            return iron;
        }

        /// <summary>
        /// Pomocna metoda, ktera vraci true, pokud
        /// je bod nenulovy.
        /// </summary>
        /// <param name="x">pozice x</param>
        /// <param name="y">pozice y</param>
        /// <returns>true, pokud je to bod a ne "voda"</returns>
        private bool Get(int x, int y)
        {
            // pokud je bod mimo aktivni prvek, tak vraci true, pokud neni pole prazdne
            if (x < px || x >= px + 4 || y < py || y >= py + 4)
                return board[y,x] > 0;

            // pokud je bod aktnivniho prvku nenulovy, tak vraci true
            if (Tetris.pieces[piece][pr][(y - py) * 4 + (x - px)] != 0) return true;

            // jinak pokud je bod nenulovy, tak true
            return board[y,x] > 0;
        }

        #endregion // Private methods
    }
}
