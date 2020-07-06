
using Aximo.WebAssembly.Interop;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Runtime.CompilerServices;

namespace Aximo.Blazor
{

    internal sealed class Runtime
    {

        public static IJSInProcessRuntime JsRuntime;

        public static string InvokeJS(string str)
        {
            return JsRuntime.Invoke<string>("Interop.InvokeJs", str);
        }

        [JSInvokable("Callback")]
        public JsToken Callback(int cbHandle, int argsHandle, int length)
        {
            Console.WriteLine("IN CALLBACK");
            Console.WriteLine($"cb: {cbHandle}, args: {argsHandle}, length: {length}");
            try
            {
                var args = new JsArrayReference(argsHandle.ToString(), length);

                if (!RuntimeImpl.Current.WasmHandles.TryGetValue(cbHandle, out var funcObj))
                    return null;

                var wasmCallback = (Func<JsToken[], JsToken>)funcObj;
                var result = wasmCallback(args.ToArray());
                return result;
            }
            catch
            {
                //Console.WriteLine($"cb: {cb.ToNullableJs()}, args: {args.ToNullableJs()}");
                throw;
            }
        }
    }

}