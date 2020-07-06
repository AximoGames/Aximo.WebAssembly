
using Aximo.WebAssembly.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;

namespace Aximo.Blazor
{
    internal class RuntimeImpl : IJsRuntime
    {

        internal static RuntimeImpl Current;

        public RuntimeImpl()
        {
            // TODO: Better way? maybe, remove Current, and add IJsRuntime GetCallback(int handle)
            Current = this;
        }

        public string InvokeJS(string str)
        {
            return Runtime.InvokeJS(str);
        }

        public JsToken GetObject(string code)
        {
            if (!code.StartsWith("return "))
                code = "return " + code;
            code = $"Interop.getObject('{code.Replace("'", "\\'")}')";
            Console.WriteLine("GetObject:" + code);
            var reference = InvokeJS(code);
            Console.WriteLine("GetObject.Result:" + reference);
            return JsToken.Parse(reference);
        }

        public TRef GetObject<TRef>(string code)
            where TRef : JsToken
        {
            return CastObject<TRef>(GetObject(code));
        }

        public JsArrayReference GetArray(string code)
        {
            return (JsArrayReference)GetObject(code);
        }

        public JsToken GetArrayElement(string arrayHandle, int index)
        {
            var reference = InvokeJS($"Interop.getArrayElement('{arrayHandle}', {index})");
            return JsToken.Parse(reference);
        }

        public void SetArrayElement(string arrayHandle, int index, JsToken value)
        {
            InvokeJS($"Interop.setArrayElementRef('{arrayHandle}', {index}, {value.ToNullableJs()})");
        }

        public JsToken GetProperty(JsReference instance, string name)
        {
            try
            {
                var result = InvokeJS($"Interop.getProperty({instance.ToNullableJs()}, '{name}')");
                return JsToken.Parse(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetProperty, Instance: {instance.ToNullableJs()}, Name: {name}");
                Console.WriteLine(ex);
                throw;
            }
        }

        public JsToken SetProperty(JsReference instance, string name, JsToken value)
        {
            var result = InvokeJS($"Interop.setProperty({instance.ToNullableJs()}, '{name}', {value.ToNullableJs()})");
            return JsToken.Parse(result);
        }

        public JsToken CallMethod(JsReference instance, string name, JsToken[] args)
        {
            var result = InvokeJS($"Interop.callMethod({instance.ToNullableJs()}, '{name}', {args.ToNullableJs()})");
            return JsToken.Parse(result);
        }

        internal Dictionary<int, object> WasmHandles = new Dictionary<int, object>();
        private JsToken CreateWasmHandle(object wasmObject)
        {
            var handle = wasmObject.GetHashCode(); // TODO: GetHashCode ok?
            if (!WasmHandles.ContainsKey(handle))
                WasmHandles.Add(handle, wasmObject);
            return new JsNumber(handle);
        }

        public JsDelegate CreateWasmCallback(Func<JsToken[], JsToken> wasmCallback)
        {
            var wasmHandle = CreateWasmHandle(wasmCallback);
            var jsHandle = InvokeJS($"Interop.createDelegate({wasmHandle.ToNullableJs()})");
            return JsToken.Parse<JsDelegate>(jsHandle);
        }

        public JsDelegate CreateWasmCallback(Delegate wasmCallback)
        {
            return CreateWasmCallback(WrapWasmCallback(wasmCallback));
        }

        internal Func<JsToken[], JsToken> WrapWasmCallback(Delegate wasmCallback)
        {
            return (args) =>
            {
                Console.WriteLine("'''''''''''''''''''''''''");
                return null;
            };
        }

        public void FreeHandle(string handle)
        {
            Runtime.InvokeJS($"Interop.freeHandle('{handle}')");
        }

        public void FreeHandles()
        {
            JsReference.FreeHandles();
        }

        public TRef TryCastObject<TRef>(object obj)
        {
            if (obj == null)
                return default;

            if (obj is TRef tryCast)
                return tryCast;

            tryCast = default;

            var token = obj as JsReference;

            if (token != null)
            {
                var handle = token.Handle;
                tryCast = Activator.CreateInstance<TRef>();
                if (tryCast is JsReference outRef)
                    outRef.Handle = handle;
            }

            return tryCast;
        }

        public TRef CastObject<TRef>(object obj)
        {
            if (obj == null)
                return default;

            var tryCast = TryCastObject<TRef>(obj);
            if (tryCast == null)
                throw new InvalidCastException($"Cannot cast {obj.GetType()} to {typeof(TRef)}");

            return tryCast;
        }

    }
}