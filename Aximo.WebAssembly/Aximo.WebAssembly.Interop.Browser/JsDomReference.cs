
using System;

namespace Aximo.WebAssembly.Interop.Browser
{
    public class JsDomReference : JsObject
    {
        internal JsDomReference(string handle) : base(handle, JsType.Element)
        {
        }

        public string TagName => this["tagName"];

        public void SetClick(Action<JsClickArgs> cb)
        {
            var handle = Runtime.CreateWasmCallback(cb);
            Runtime.CallMethod(this, "addEventListener", new JsToken[] { new JsString("click"), handle });
        }

    }

}