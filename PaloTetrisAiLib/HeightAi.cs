using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PaloTetris;

namespace PaloTetrisAiLib
{
    /// <summary>
    /// Umela inteligence do Palo-Tetrisu.
    /// Velice jednoduchy algoritmus ohodnocujici pouze vysku policka.
    /// </summary>
    public class HeightAi : BaseAi
    {
        #region Fields

        private readonly Guid UNIQUE_ID = new Guid("692BAF56-6A5D-4289-86E0-92248D7C1686");

        private const string DISPLAYNAME = "Height AI";

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
        /// Ohodnoceni pozice dle vysky.
        /// </summary>
        /// <param name="Tetris"></param>
        /// <returns></returns>
        protected override int Evaluate(PaloTetris.ITetrisGame game)
        {
            int ret = 0;

            ret += py * py * py;

            return ret;
        }

        #endregion
    }
}
