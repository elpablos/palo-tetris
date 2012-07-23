using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleTetris.Fields
{
    /// <summary>
    /// Prvek reprezentujici utvar O.
    /// </summary>
    public class FieldO : BaseField
    {
        protected override int[][] _pieces
        {
            get
            {
                return 
                    // typ O
                    new int[][] 
                    {
                        // prvni a jedina rotace
                        new int[] 
                        { 
                            0,0,0,0,
                            0,1,1,0,
                            0,1,1,0,
                            0,0,0,0
                        }
                    };
            }
        }
    }
}
