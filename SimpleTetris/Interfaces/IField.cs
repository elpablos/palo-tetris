using System;
namespace SimpleTetris.Interfaces
{
    /// <summary>
    /// Interface pro prvky tetris hry.
    /// </summary>
    public interface IField
    {
        int GetPiece(int x, int y);
        int MaxRotation { get; }
        int[] Pieces { get; }
        int Rotation { get; set; }
    }
}
