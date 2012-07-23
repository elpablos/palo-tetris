using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleTetris.Fields
{
    /// <summary>
    /// Prvek reprezentujici utvar Z.
    /// </summary>
    public class FieldZ : BaseField
    {
        protected override int[][] _pieces
        {
            get 
            {
                return
                    // typ Z
                    new int[][]
                    {
                        // typ Z - horizontalne
                        new int[]
                        {
                            0,0,0,0,
                            0,4,4,0,
                            0,0,4,4,
                            0,0,0,0
                        },
                        // typ Z - vertikalne
                        new int[]
                        {
                            0,0,0,4,
                            0,0,4,4,
                            0,0,4,0,
                            0,0,0,0
                        }
                    };
            }
        }
    }
}
