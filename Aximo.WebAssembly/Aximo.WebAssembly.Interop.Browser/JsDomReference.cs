
using System;

namespace Aximo.WebAssembly.Interop.Browser
{
    public class JsDomReference : JsObject
    {
        public JsDomReference(string handle) : base(handle, JsType.Element)
        {
        }

        public JsDomReference() : base(null, JsType.Element)
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