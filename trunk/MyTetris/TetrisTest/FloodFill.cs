using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TetrisTest
{
    public class FloodFill
    {
        public int[,] Field { get; private set; }

        public event EventHandler FieldColored = delegate { };

        public FloodFill(int[,] field)
        {
            Field = (int[,])field.Clone();

            //field.
        }

        public void ColorField(int startRow, int x, int y, int targetColor, int replacementColor)
        {
            // pokud je mimo rozsah
            if (x < 0 || x >= Field.GetLength(1) || y < 0 || y >= Field.GetLength(0))
            {
                // Console.WriteLine("FF-out-of-range:{0}/{1}", x, y);
                return;
            }
            // pokud pole neni cilem
            if (Field[y, x] != targetColor) return;
            // obarvujeme
            Field[y, x] = replacementColor;
            // FieldColored(this, EventArgs.Empty);
            // posilame obarvit i jine
            //floodFill(x - 1, y, targetColor, replacementColor);
            if (y == startRow)
                ColorField(startRow, x + 1, y, targetColor, replacementColor);  
            //floodFill(x, y - 1, targetColor, replacementColor);
            ColorField(startRow, x, y + 1, targetColor, replacementColor);
        }

        public int GetCoveredHoles()
        {
            int i = 0;
            int count = 0;
            while (i < Field.Length)
            {
                if (Field[i / Field.GetLength(1), i % Field.GetLength(1)] == 0) count++;
                i++;
            }

            return count;
        }
    }
}
