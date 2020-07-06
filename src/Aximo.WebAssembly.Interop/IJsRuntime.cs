using System;
using System.Collections.Generic;
using System.Text;

namespace Aximo.WebAssembly.Interop
{

    public interface IJsRuntime
    {
        string InvokeJS(string str);
        TRef TryCastObject<TRef>(JsToken obj) where TRef : JsToken;
        TRef CastObject<TRef>(JsToken obj) where TRef : JsToken;
        JsToken GetObject(string code);
        JsToken GetArrayElement(string arrayHandle, int index);
        void SetArrayElement(string arrayHandle, int index, JsToken value);
        JsToken GetProperty(JsReference instance, string name);
        JsToken SetProperty(JsReference instance, string name, JsToken value);
        JsToken CallMethod(JsReference instance, string name, JsToken[] args);
        JsDelegate CreateWasmCallback(Delegate wasmCallback);
        JsDelegate CreateWasmCallback(Func<JsToken[], JsToken> wasmCallback);
        void FreeHandle(string handle);
        void FreeHandles();
    }
}
