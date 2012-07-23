using System;
namespace SimpleTetris.Interfaces
{
    public interface ITetrisGame
    {
        IField ActivePiece { get; }
        int ActiveX { get; }
        int ActiveY { get; }
        int[,] Board { get; }
        bool CanParkPiece(int x, int y, int[] piece);
        bool Drop();
        //IList<IField> Fields { get; }
        int Height { get; }
        bool IsSpaceFor(int x, int y, int[] piece);
        bool Left();
        event EventHandler Repaint;
        event EventHandler NextPieceGenerated;
        void Reset();
        bool Right();
        bool Rotate();
        void Start();
        void Stop();
        int Width { get; }

        bool IsRunning { get; }
    }
}
