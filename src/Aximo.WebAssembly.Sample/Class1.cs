using Aximo.WebAssembly.Interop;
using Aximo.WebAssembly.Interop.Browser;
using System;
using System.Threading;

namespace Aximo.WebAssembly.Sample
{
    public class Class1
    {
        public static void Run()
        {
            StartTime = DateTime.Now;
            Console.WriteLine("Hello from C#!");

            Runtime.InvokeJS("Interop.appendResult('dddd')", out _);

            DomTest();
            var tim = new Timer((e) => Test2());
            tim.Change(TimeSpan.FromSeconds(.1), TimeSpan.FromSeconds(1));
        }

        private static void DomTest()
        {
            var el = Browser.GetElementById("results");
            Console.WriteLine("TAG:" + el.TagName);
            el.SetClick((e) =>
            {
                Console.WriteLine("abc");
            });
        }

        private static DateTime StartTime;

        private static void Test2()
        {
            Console.WriteLine("HHH");
            var s = 5;
            var r = Runtime.GetArray("[55,66,77]");

            var el = (JsNumber)r[1];
            Console.WriteLine(el.Value);

            Console.WriteLine("Length: " + r.Length);

            //GC.Collect(GC.MaxGeneration);
            Runtime.FreeHandles();

            if ((DateTime.Now - StartTime).TotalSeconds > 10)
            {
                DomTest();
            }
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
