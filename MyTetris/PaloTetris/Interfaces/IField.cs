using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace PaloTetris
{
    [InheritedExport]
    public interface IField
    {
        int MaxRotation { get; }
        int[] Pieces { get; }
        int Rotation { get; set; }

        int GetPiece(int x, int y);
    }
}
