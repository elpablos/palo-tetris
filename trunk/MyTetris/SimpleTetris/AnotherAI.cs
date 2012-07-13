using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleTetris.Interfaces;

namespace SimpleTetris
{
    public class AnotherAI : ITetrisAI
    {
        #region Fields

        protected int px;
        protected int py;
        protected int pr;

        #endregion // Fields

        #region Properties

        public ITetrisGame Tetris { get; private set; }

        #endregion // Properties

        #region Public methods

        public void Run(ITetrisGame game)
        {
            if (game == null || game.IsRunning == false)
                return;

            Tetris = game;

            int maxScore = Int32.MinValue;
            int maxpx = 0;
            int maxpr = 0;

            // pro vsechny x od -4 do maxX +4 
            for (px = -4; px < Tetris.Width + 4; px++)
            {
                int rot = Tetris.ActivePiece.Rotation;
                // pro vsechny rotace pro dany prvek
                for (pr = 0; pr < Tetris.ActivePiece.MaxRotation; pr++)
                {
                    Tetris.ActivePiece.Rotation = pr;
                    // pokud nelze prvek umistit, tak preskoc cykly
                    if (!Tetris.IsSpaceFor(px, 0, Tetris.ActivePiece.Pieces))
                    {
                        continue;
                    }
                    // vynulujeme posuv po Y
                    py = 0;

                    // inkrementuj Y dokud lze polozit prvek
                    while (!Tetris.CanParkPiece(px, py, Tetris.ActivePiece.Pieces))
                        py++;

                    // spocitej  ohodnoceni prvku na dane pozici
                    int n = Evaluate();
                    // pokud je ohodnoceni vetsi nez MAX skore, tak jej uloz
                    if (n > maxScore)
                    {
                        maxScore = n;
                        maxpx = px;
                        maxpr = pr;
                    }
                }
                Tetris.ActivePiece.Rotation = rot;
            }
            Console.WriteLine(maxScore);

            // orotuj, dokud se nedostanes na maximalne ohodnocenou rotaci
            for (int i = 0; i < 4 && Tetris.ActivePiece.Rotation != maxpr; i++)
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
                Tetris.Drop();
            }
        }

        #endregion // Public methods

        #region Private methods

        /// <summary>
        /// Ohodnoceni boardu pro danou konfiguraci.
        /// </summary>
        /// <returns></returns>
        private int Evaluate()
        {
            int ret = 0;

            ret += py * py * py;

            // pocitani der
            FloodFill ff = new FloodFill(Tetris, px, py);
            ff.ColorField(0, 0, 0, 0, 9);
            ret -= ff.GetCoveredHoles() * 100;
            return ret;
        }

        #endregion // Private methods
    }
}
