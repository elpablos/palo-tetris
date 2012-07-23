using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaloTetris
{
    /// <summary>
    /// Prvek reprezentujici utvar I.
    /// </summary>
    public class FieldI : BaseField
    {
        protected override int[][] _pieces
        {
            get
            {
                return
                    // typ I
                    new int[][]
                    {
                        // typ I - horizontalne
                        new int[]
                        {
                            0,0,0,0,
                            2,2,2,2,
                            0,0,0,0,
                            0,0,0,0
                        }
                        ,
                        // typ I - vertikalne
                        new int[]
                        {
                            0,0,2,0,
                            0,0,2,0,
                            0,0,2,0,
                            0,0,2,0
                        }
                    };
            }
        }
    }
}
