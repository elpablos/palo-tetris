using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleTetris.Fields
{
    /// <summary>
    /// Prvek reprezentujici utvar J.
    /// </summary>
    public class FieldJ : BaseField
    {
        protected override int[][] _pieces
        {
            get 
            {
                return
                    // typ J
                    new int[][]
                    {
                        // typ J - horizontalne, packou dolu
                        new int[]
                        {
                            0,0,0,0,
                            0,6,6,6,
                            0,0,0,6,
                            0,0,0,0
                        },
                        // typ J - vertikalne, packou doprava
                        new int[]
                        {
                            0,0,6,6,
                            0,0,6,0,
                            0,0,6,0,
                            0,0,0,0
                        },
                        // typ J - horizontalne, packou nahoru
                        new int[]
                        {
                            0,6,0,0,
                            0,6,6,6,
                            0,0,0,0,
                            0,0,0,0
                        },
                        // typ J - vertikalne, packou doleva
                        new int[]
                        {
                            0,0,6,0,
                            0,0,6,0,
                            0,6,6,0,
                            0,0,0,0
                        }
                    };
            }
        }
    }
}
