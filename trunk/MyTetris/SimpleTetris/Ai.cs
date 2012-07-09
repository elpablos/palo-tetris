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

            // vubec nevim proc -13
            px = pr = -13;
            piece = Tetris.ActivePiece;
            //board = Tetris.Board;

            for (px = -4; px < board.GetLength(1) + 4 && !next; px++)
            {
                for (pr = 0; pr < Tetris.pieces[piece].Length && !next; pr++)
                {
                    if (!Tetris.IsSpaceFor(px, 0, Tetris.pieces[piece][pr]))
                    {
                        continue;
                    }
                    py = 0;

                    while (!Tetris.CanParkPiece(px, py, Tetris.pieces[piece][pr]))
                        py++;

                    int n = Eveluate();
                    if (n > maxScore)
                    {
                        maxScore = n;
                        maxpx = px;
                        maxpr = pr;
                    }
                }
            }

            // dalsi smycka
            for (int i = 0; i < 4 && Tetris.ActiveR != maxpr; i++)
            {
                Tetris.Rotate();
            }

            int displace = maxpx - Tetris.ActiveX;

            while (displace > 0)
            {
                if (!Tetris.Right())
                    Console.WriteLine("Can't move to {0} stack on {1}", maxpx, Tetris.ActiveX);
                displace--;
            }
            while (displace < 0)
            {
                if (!Tetris.Left())
                    Console.WriteLine("Can't move to {0} stack on {1}", maxpx, Tetris.ActiveX);
                displace++;
            }

            if (Tetris.ActiveX != maxpx)
            {
                Console.WriteLine("Uh oh");
            }
            else
            {
                Console.WriteLine("Drop");
                Tetris.Drop();
            }
        }

        #endregion // Public methods

        #region Private methods

        private int Eveluate()
        {
            int ret = 0;

            ret += IronMill();
            ret -= CountCoveredHoles() - 10000;

            Console.WriteLine("Evaluating.. {0}", ret);
            return ret;
        }

        private int CountCoveredHoles()
        {
            int holes = 0;
            for (int x = 0; x < board.GetLength(1); x++)
            {
                bool swap = false;
                for (int y = 0; y < board.GetLength(0); y++)
                {
                    if (Get(x, y)) swap = true;
                    else
                    {
                        if (swap) holes++;
                        swap = false;
                    }
                }
            }
            Console.WriteLine("Holes: " + holes);
            return holes;
        }

        private int IronMill()
        {
            int iron = 0;
            for (int y = 0; y < board.GetLength(0); y++)
            {
                for (int x = 0; x < board.GetLength(1); x++)
                {
                    if (Get(x, y)) iron += y * y * y;
                }
            }
            return iron;
        }

        private bool Get(int x, int y)
        {
            if (x < px || x >= px + 4 || y < py || y >= py + 4)
                return board[y,x] > 0;

            if (Tetris.pieces[piece][pr][(y - py) * 4 + (x - px)] != 0) return true;
            return board[y,x] > 0;
        }

        #endregion // Private methods
    }
}
