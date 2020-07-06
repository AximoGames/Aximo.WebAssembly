
using Aximo.WebAssembly.Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data;

namespace WebAssembly
{
    internal class RuntimeImpl : IJsRuntime
    {

        internal static RuntimeImpl Current;

        public RuntimeImpl()
        {
            // TODO: Better way? maybe, remove Current, and add IJsRuntime GetCallback(int handle)
            Current = this;
        }

        public string InvokeJS(string str, out int exceptional_result)
        {
            var result = Runtime.InvokeJS(str, out exceptional_result);
            if (exceptional_result != 0)
            {
                Console.WriteLine($"Error: {result}");
                Console.WriteLine($"ErrorCode: {exceptional_result}");
                throw new Exception(result);
            }
            return result;
        }

        public JsToken GetObject(string code)
        {
            if (!code.StartsWith("return "))
                code = "return " + code;
            code = $"Interop.getObject('{code.Replace("'", "\\'")}')";
            Console.WriteLine("GetObject:" + code);
            var reference = InvokeJS(code, out _);
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
            var reference = InvokeJS($"Interop.getArrayElement('{arrayHandle}', {index})", out _);
            return JsToken.Parse(reference);
        }

        public void SetArrayElement(string arrayHandle, int index, JsToken value)
        {
            InvokeJS($"Interop.setArrayElementRef('{arrayHandle}', {index}, {value.ToNullableJs()})", out _);
        }

        public JsToken GetProperty(JsReference instance, string name)
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

        public JsToken SetProperty(JsReference instance, string name, JsToken value)
        {
            var result = InvokeJS($"Interop.setProperty({instance.ToNullableJs()}, '{name}', {value.ToNullableJs()})", out _);
            return JsToken.Parse(result);
        }

        public JsToken CallMethod(JsReference instance, string name, JsToken[] args)
        {
            var result = InvokeJS($"Interop.callMethod({instance.ToNullableJs()}, '{name}', {args.ToNullableJs()})", out _);
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
            var jsHandle = InvokeJS($"Interop.createDelegate({wasmHandle.ToNullableJs()})", out var _);
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
            Runtime.InvokeJS($"Interop.freeHandle('{handle}')", out _);
        }

        public void FreeHandles()
        {
            JsReference.FreeHandles();
        }

        public TRef TryCastObject<TRef>(JsToken obj) where TRef : JsToken
        {
            if (obj == null)
                return default;

            var tryCast = obj as TRef;
            if (tryCast != null)
                return tryCast;

            if (obj.IsReferenceType)
            {
                var handle = (obj as JsReference).Handle;
                tryCast = Activator.CreateInstance<TRef>();
                (tryCast as JsReference).Handle = handle;
            }

            return tryCast;
        }

        public TRef CastObject<TRef>(JsToken obj) where TRef : JsToken
        {
            if (obj == null)
                return default;

            var tryCast = TryCastObject<TRef>(obj);
            if (tryCast == null)
                throw new InvalidCastException($"Cannot cast {obj.ToNullableJs()} to {typeof(TRef)}");

            return tryCast;
        }

    }
}