using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleTetris.Fields
{
    /// <summary>
    /// Prvek reprezentujici utvar T.
    /// </summary>
    public class FieldT : BaseField
    {
        protected override int[][] _pieces
        {
            get 
            {
                return
                    // typ T
                    new int[][]
                    {
                        // typ T - horizontalne, packou dolu
                        new int[]
                        {
                            0,0,0,0,
                            0,7,7,7,
                            0,0,7,0,
                            0,0,0,0
                        },
                        // typ T - vertikalne, packou doprava
                        new int[]
                        {
                            0,0,7,0,
                            0,0,7,7,
                            0,0,7,0,
                            0,0,0,0
                        },
                        // typ T - horizontalne, packou nahoru
                        new int[]
                        {
                            0,0,7,0,
                            0,7,7,7,
                            0,0,0,0,
                            0,0,0,0
                        },
                        // typ T - vertikalne, packou doleva
                        new int[]
                        {
                            0,0,7,0,
                            0,7,7,0,
                            0,0,7,0,
                            0,0,0,0
                        }
                    };
            }
        }
    }
}
