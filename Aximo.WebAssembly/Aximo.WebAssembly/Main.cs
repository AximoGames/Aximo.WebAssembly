using System;
using System.Threading;
using WebAssembly;

namespace Wep.WebAdmin
{
    class Program
    {
        static void Main(string[] args)
        {
            Init();
        }

        private static void Init()
        {
            Console.WriteLine("Hello from C#!");

            Runtime.InvokeJS("Interop.appendResult('dddd')", out _);
            var el = Runtime.GetDomElement("results");
            Console.WriteLine("TAG:" + el.TagName);
            el.SetClick((e) =>
            {
                Console.WriteLine("abc");
            });

            var tim = new Timer((e) => Test2());
            tim.Change(TimeSpan.FromSeconds(.1), TimeSpan.FromSeconds(1));
        }

        private static void Test2()
        {
            Console.WriteLine("HHH");
            var s = 5;
            var r = Runtime.GetArray("[55,66,77]");

            var el = (JsNumber)r[1];
            Console.WriteLine(el.Value);

            Console.WriteLine("Length: " + r.Length);

            //GC.Collect(GC.MaxGeneration);
            JsReference.ExecFree();
        }

        private static void Test()
        {
            while (true)
            {
                Console.WriteLine("HHH");
                Thread.Sleep(1000);
            }
        }

    }
}
