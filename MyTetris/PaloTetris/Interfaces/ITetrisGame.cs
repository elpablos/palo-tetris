using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace PaloTetris
{
    [InheritedExport]
    public interface ITetrisGame
    {
        Guid UniqueID { get; }
        string DisplayName { get; }
        IField ActivePiece { get; }
        int ActiveX { get; }
        int ActiveY { get; }
        int[,] Board { get; }
        int Width { get; set; }
        int Height { get; set; }
        bool IsRunning { get; }

        event EventHandler GameEnd;
        event EventHandler Repaint;
        event EventHandler NextPieceGenerated;

        void Start();
        void Stop();
        void Reset();

        bool CanParkPiece(int x, int y, int[] piece);
        bool IsSpaceFor(int x, int y, int[] piece);

        bool Left();
        bool Right();
        bool Rotate();
        bool Drop();
    }
}
