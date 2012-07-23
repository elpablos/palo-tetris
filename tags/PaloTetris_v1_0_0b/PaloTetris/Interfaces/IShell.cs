using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace PaloTetris
{
    [InheritedExport]
    public interface IShell
    {
        IEnumerable<ITetrisGame> TetrisGameCollection { get; }

        IEnumerable<ITetrisAI> TetrisAiCollection { get; }

        ITetrisGame TetrisGame { get; set; }

        ITetrisAI TetrisAi { get; set; }

        int MaxX { get; set; }

        int MaxY { get; set; }

        bool StartAI { get; set; }

        void Show();
    }
}
