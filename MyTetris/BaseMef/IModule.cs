using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition;

namespace BaseMef
{
    [InheritedExport]
    public interface IModule
    {
        string DisplayName { get; }
    }
}
