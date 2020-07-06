
using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Runtime.CompilerServices;

namespace Aximo.WebAssembly.Interop
{
    public static class Runtime
    {

        internal static IJsRuntime BaseRuntime { get; set; }
        public static void Initialize(IJsRuntime runtime)
        {
            BaseRuntime = runtime;
        }

        public static string InvokeJS(string code) => BaseRuntime.InvokeJS(code);
        public static JsToken GetObject(string code) => BaseRuntime.GetObject(code);
        public static TRef GetObject<TRef>(string code)
            where TRef : JsToken
        {
            return CastObject<TRef>(GetObject(code));
        }

        public static JsArrayReference GetArray(string code)
        {
            return (JsArrayReference)GetObject(code);
        }

        public static JsToken GetArrayElement(string arrayHandle, int index) => BaseRuntime.GetArrayElement(arrayHandle, index);
        public static void SetArrayElement(string arrayHandle, int index, JsToken value) => BaseRuntime.SetArrayElement(arrayHandle, index, value);
        public static JsToken GetProperty(JsReference instance, string name) => BaseRuntime.GetProperty(instance, name);
        public static JsToken SetProperty(JsReference instance, string name, JsToken value) => BaseRuntime.SetProperty(instance, name, value);
        public static JsToken CallMethod(JsReference instance, string name, JsToken[] args) => BaseRuntime.CallMethod(instance, name, args);
        public static JsDelegate CreateWasmCallback(Func<JsToken[], JsToken> wasmCallback) => BaseRuntime.CreateWasmCallback(wasmCallback);
        public static JsDelegate CreateWasmCallback(Delegate wasmCallback) => BaseRuntime.CreateWasmCallback(wasmCallback);
        public static void FreeHandles() => BaseRuntime.FreeHandles();
        public static TRef TryCastObject<TRef>(JsToken obj) where TRef : JsToken => BaseRuntime.TryCastObject<TRef>(obj);
        public static TRef CastObject<TRef>(JsToken obj) where TRef : JsToken => BaseRuntime.CastObject<TRef>(obj);
    }

}