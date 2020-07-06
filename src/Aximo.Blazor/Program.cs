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

        private static IJSInProcessRuntime Runtime;

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
            Runtime = runtime;

            var tim = new Timer((e) => test2());
            tim.Change(TimeSpan.FromSeconds(.1), TimeSpan.FromSeconds(1));
        }

        private static void test2()
        {
            var s = "blubbX";
            Runtime.InvokeVoid("window.console.log", s);
        }

    }
}
