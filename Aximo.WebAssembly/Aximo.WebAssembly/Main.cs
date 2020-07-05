using System;
using System.Threading;
using WebAssembly;

namespace Wep.WebAdmin
{
    class Program
    {
        static void Main(string[] args)
        {
            Aximo.WebAssembly.Interop.Runtime.Initialize(new RuntimeImpl());
            Aximo.WebAssembly.Sample.Class1.Run();
        }
    }
}
