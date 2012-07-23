using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleTetris.Fields
{
    /// <summary>
    /// Prvek reprezentujici utvar S.
    /// </summary>
    public class FieldS : BaseField
    {
        protected override int[][] _pieces
        {
            get 
            {
                return
                    // typ S
                    new int[][]
                    {
                        // typ S - horizontalne
                        new int[]
                        {
                            0,0,0,0,
                            0,0,3,3,
                            0,3,3,0,
                            0,0,0,0
                        },
                        // typ S - vertikalne
                        new int[]
                        {
                            0,0,3,0,
                            0,0,3,3,
                            0,0,0,3,
                            0,0,0,0
                        }
                    };
            }
        }
    }
}
