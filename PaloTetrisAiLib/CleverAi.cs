using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaloTetris;

namespace PaloTetrisAiLib
{
    /// <summary>
    /// Umela inteligence pro Palo-tetris.
    /// Princip teto UI je pocitani zakrytych der, vysky pokladaneho prvku a pricitani bodu za radky, ktere padnou.
    /// </summary>
    public class CleverAi : BaseAi
    {
        #region Fields

        private readonly Guid UNIQUE_ID = new Guid("B4629553-D3C3-4555-9FF7-940654F462BD");

        private const string DISPLAYNAME = "Clever AI";

        #endregion // Fields

        #region Properties

        public override Guid UniqueID
        {
            get { return UNIQUE_ID; }
        }

        public override string DisplayName
        {
            get { return DISPLAYNAME; }
        }

        #endregion

        #region Overrided methods

        /// <summary>
        /// Ohodnoceni pozice dle vysky, prekrytych kosticek a kompletnich der.
        /// </summary>
        /// <param name="Tetris"></param>
        /// <returns></returns>
        protected override int Evaluate(PaloTetris.ITetrisGame game)
        {
            int ret = 0;

            ModifiedFloodFill holeCounter = new ModifiedFloodFill(game, px, py);
            holeCounter.ColorField(0, 0, 0, 0, 9);

            // vyska
            // int height = (game.Height - holeCounter.GetHigherPoint() * 10);
            int height = py * py;
            ret += height;
            // diry
            int holes = (holeCounter.GetCoveredHoles() * 25);
            ret -= holes;
            // radky
            int lines = (holeCounter.GetNumberOfFullLines() * 100);
            ret += lines;

            Console.WriteLine("[Height,Holes,Lines]:[{0},{1},{2}]:{3}", height, holes, lines, ret);

            return ret;
        }

        #endregion
    }
}
