using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.Threading;

namespace Aximo.Blazor
{
    public class Program
    {

        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<IJSInProcessRuntime>(services => (IJSInProcessRuntime)services.GetRequiredService<IJSRuntime>());

            await builder.Build().RunAsync();
        }

        public static void Test(IJSInProcessRuntime runtime)
        {
            Runtime.JsRuntime = runtime;

            //var tim = new Timer((e) => test2());
            //tim.Change(TimeSpan.FromSeconds(.1), TimeSpan.FromSeconds(1));

            Aximo.WebAssembly.Interop.Runtime.Initialize(new RuntimeImpl());
            Aximo.WebAssembly.Sample.Class1.Run();
        }

        private static void test2()
        {
            var s = "blubbX";
            Runtime.JsRuntime.InvokeVoid("window.console.log", s);
        }

    }
}
