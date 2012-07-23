using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaloTetris;

namespace PaloTetris
{
    public abstract class BaseAi : ITetrisAI
    {
        #region Fields

        protected int px;
        protected int py;
        protected int pr;

        #endregion // Fields

        #region Properties

        public abstract Guid UniqueID
        {
            get;
        }

        public abstract string DisplayName
        {
            get;
        }

        #endregion // Properties

        #region Public methods

        public void Run(ITetrisGame game)
        {
            if (game == null || game.IsRunning == false)
                return;

            int maxScore = Int32.MinValue;
            int maxpx = 0;
            int maxpr = 0;

            // pro vsechny x od -4 do maxX +4 
            for (px = -4; px < game.Width + 4; px++)
            {
                int rot = game.ActivePiece.Rotation;
                // pro vsechny rotace pro dany prvek
                for (pr = 0; pr < game.ActivePiece.MaxRotation; pr++)
                {
                    game.ActivePiece.Rotation = pr;

                    // pokud nelze prvek umistit, tak preskoc cykly
                    if (!game.IsSpaceFor(px, 0, game.ActivePiece.Pieces))
                    {
                        continue;
                    }
                    // vynulujeme posuv po Y
                    py = 0;

                    // inkrementuj Y dokud lze polozit prvek
                    while (!game.CanParkPiece(px, py, game.ActivePiece.Pieces))
                        py++;

                    // spocitej  ohodnoceni prvku na dane pozici
                    int n = Evaluate(game);
                    // pokud je ohodnoceni vetsi nez MAX skore, tak jej uloz
                    if (n > maxScore)
                    {
                        maxScore = n;
                        maxpx = px;
                        maxpr = pr;
                    }
                }
                game.ActivePiece.Rotation = rot;
            }

            // orotuj, dokud se nedostanes na maximalne ohodnocenou rotaci
            for (int i = 0; i < 4 && game.ActivePiece.Rotation != maxpr; i++)
            {
                game.Rotate();
            }

            // spocitej rozdil mezi maximalne ohodnocenym X a aktivnim X
            int displace = maxpx - game.ActiveX;

            // pokud je rozdil vetsi nez 0, tak posouvej doleva, dokud se to nesrovna
            while (displace > 0)
            {
                if (!game.Right())
                    Console.WriteLine("Can't move to {0} stack on {1}", maxpx, game.ActiveX);
                displace--;
            }

            // pokud je rozdil mensi nez 0, tak posouvej doprava, dokud se to nesrovna
            while (displace < 0)
            {
                if (!game.Left())
                    Console.WriteLine("Can't move to {0} stack on {1}", maxpx, game.ActiveX);
                displace++;
            }

            // pokud neni X stejne jako maximalni X, tak problem
            if (game.ActiveX != maxpx)
            {
                Console.WriteLine("Uh oh");
            }
            // jinak dropni prvek
            else
            {
                game.Drop();
            } 
        }

        #endregion // Public methods

        #region Abstract methods

        protected abstract int Evaluate(ITetrisGame game);

        #endregion // Abstract methods
    }
}
