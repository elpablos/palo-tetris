using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Composition.Hosting;
using BaseMef;

namespace MefApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var catalog = new DirectoryCatalog(@"D:\home-net\MyTetris\MefApp\bin\Debug");
            var container = new CompositionContainer(catalog);
            IEnumerable<Lazy<IModule>> module = container.GetExports<IModule>();
            Console.WriteLine(module.Count());

            foreach (Lazy<IModule> item in module)
            {
                Console.WriteLine(item.Value.DisplayName);
            }
        }
    }
}
