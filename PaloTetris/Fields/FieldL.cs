using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaloTetris
{
    /// <summary>
    /// Prvek reprezentujici utvar L.
    /// </summary>
    public class FieldL : BaseField
    {
        protected override int[][] _pieces
        {
            get
            {
                return
                    // typ L
                    new int[][]
                    {
                        // typ L - horizontalne, packou dolu
                        new int[]
                        {
                            0,0,0,0,
                            0,5,5,5,
                            0,5,0,0,
                            0,0,0,0
                        },
                        // typ L - vertikalne, packou doprava
                        new int[]
                        {
                            0,0,5,0,
                            0,0,5,0,
                            0,0,5,5,
                            0,0,0,0
                        },
                        // typ L - horizontalne, packou nahoru
                        new int[]
                        {
                            0,0,0,5,
                            0,5,5,5,
                            0,0,0,0,
                            0,0,0,0
                        },
                        // typ L - vertikalne, packou doleva
                        new int[]
                        { 
                            0,5,5,0,
                            0,0,5,0,
                            0,0,5,0,
                            0,0,0,0
                        }
                    };
            }
        }
    }
}
