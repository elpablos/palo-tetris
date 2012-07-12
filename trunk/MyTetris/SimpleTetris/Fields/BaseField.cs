using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.ComponentModel.Composition;
using SimpleTetris.Interfaces;

namespace SimpleTetris.Fields
{
    /// <summary>
    /// Abstraktni prvek hry.
    /// </summary>
    public abstract class BaseField : IField
    {
        #region Fields

        private int _rotation;

        #endregion // Fields

        #region Properties

        protected abstract int[][] _pieces { get; }

        public int Rotation
        {
            get { return _rotation; }
            set
            {
                if (_rotation != value)
                    _rotation = value;
                _rotation = value % MaxRotation;
            }
        }

        public int MaxRotation
        {
            get { return _pieces.Count(); }
        }

        public int[] Pieces
        {
            get { return _pieces[_rotation]; }
        }

        #endregion // Properties

        #region Public methods

        public int GetPiece(int x, int y)
        {
            return Pieces[y * 4 + x];
        }

        /// <summary>
        /// Vraci barvu dle typu policka.
        /// </summary>
        /// <param name="colorNumber">cislo barvy</param>
        /// <returns>barva</returns>
        public static Brush PieceColor(int colorNumber)
        {
            Brush ret = Brushes.White;
            switch (colorNumber)
            {
                case 1:
                    ret = Brushes.Yellow;
                    break;
                case 2:
                    ret = Brushes.Blue;
                    break;
                case 3:
                    ret = Brushes.Red;
                    break;
                case 4:
                    ret = Brushes.Cyan;
                    break;
                case 5:
                    ret = Brushes.Green;
                    break;
                case 6:
                    ret = Brushes.Magenta;
                    break;
                case 7:
                    ret = Brushes.Black;
                    break;
                default:
                    break;
            }
            return ret;
        }

        #endregion // Public methods
    }
}
