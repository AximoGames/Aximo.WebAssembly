
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

        public static JsDomReference GetDomElement(string selector)
        {
            var reference = InvokeJS($"Interop.getElementById('{selector}')", out _);
            return JsToken.Parse<JsDomReference>(reference);
        }

        public static JsToken GetObject(string code)
        {
            if (!code.StartsWith("return "))
                code = "return " + code;
            var reference = InvokeJS($"Interop.getObject('{code}')", out _);
            return JsToken.Parse(reference);
        }

        public static TRef GetObject<TRef>(string code)
            where TRef : JsToken
        {
            return (TRef)GetObject(code);
        }

        public static JsArrayReference GetArray(string code)
        {
            return (JsArrayReference)GetObject(code);
        }

        internal static JsToken GetArrayElement(string arrayHandle, int index)
        {
            var reference = InvokeJS($"Interop.getArrayElement('{arrayHandle}', {index})", out _);
            return JsToken.Parse(reference);
        }

        internal static void SetArrayElement(string arrayHandle, int index, JsToken value)
        {
            InvokeJS($"Interop.setArrayElementRef('{arrayHandle}', {index}, {value.ToNullableJs()})", out _);
        }

        public static JsToken GetProperty(JsReference instance, string name)
        {
            try
            {
                var result = InvokeJS($"Interop.getProperty({instance.ToNullableJs()}, '{name}')", out _);
                return JsToken.Parse(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetProperty, Instance: {instance.ToNullableJs()}, Name: {name}");
                Console.WriteLine(ex);
                throw;
            }
        }

        public static JsToken SetProperty(JsReference instance, string name, JsToken value)
        {
            var result = InvokeJS($"Interop.setProperty({instance.ToNullableJs()}, '{name}', {value.ToNullableJs()})", out _);
            return JsToken.Parse(result);
        }

        public static JsToken CallMethod(JsReference instance, string name, JsToken[] args)
        {
            var result = InvokeJS($"Interop.callMethod({instance.ToNullableJs()}, '{name}', {args.ToNullableJs()})", out _);
            return JsToken.Parse(result);
        }

        private static Dictionary<int, object> WasmHandles = new Dictionary<int, object>();
        private static JsToken CreateWasmHandle(object wasmObject)
        {
            var handle = wasmObject.GetHashCode(); // TODO: GetHashCode ok?
            WasmHandles.Add(handle, wasmObject);
            return new JsNumber(handle);
        }

        internal static JsDelegate CreateWasmCallback(Func<JsToken[], JsToken> wasmCallback)
        {
            var wasmHandle = CreateWasmHandle(wasmCallback);
            var jsHandle = InvokeJS($"Interop.createDelegate({wasmHandle.ToNullableJs()})", out var _);
            return JsToken.Parse<JsDelegate>(jsHandle);
        }

        internal static JsToken CreateWasmCallback(Delegate wasmCallback)
        {
            return CreateWasmCallback(WrapWasmCallback(wasmCallback));
        }

        internal static Func<JsToken[], JsToken> WrapWasmCallback(Delegate wasmCallback)
        {
            return (args) =>
            {
                Console.WriteLine("'''''''''''''''''''''''''");
                return null;
            };
        }

        public static JsToken Callback(int cbHandle, int argsHandle, int length)
        {
            Console.WriteLine("IN CALLBACK");
            Console.WriteLine($"cb: {cbHandle}, args: {argsHandle}, length: {length}");
            try
            {
                var args = new JsArrayReference(argsHandle.ToString(), length);

                if (!WasmHandles.TryGetValue(cbHandle, out var funcObj))
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

    public class JsClickArgs
    {
    }

}