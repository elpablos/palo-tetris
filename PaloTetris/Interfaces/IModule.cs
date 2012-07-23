using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace PaloTetris
{
    [InheritedExport]
    public interface IModule
    {
        void AfterLoaded(bool isFirstTime);
    }
}
