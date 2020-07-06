
using Aximo.WebAssembly.Interop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Runtime.CompilerServices;

namespace WebAssembly
{

    internal sealed class Runtime
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern string InvokeJS(string str, out int exceptional_result);

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