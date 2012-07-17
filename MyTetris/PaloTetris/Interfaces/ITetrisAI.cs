using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace PaloTetris
{
    [InheritedExport]
    public interface ITetrisAI
    {
        Guid UniqueID { get; }
        string DisplayName { get; }

        void Run(ITetrisGame Tetris);
    }
}
