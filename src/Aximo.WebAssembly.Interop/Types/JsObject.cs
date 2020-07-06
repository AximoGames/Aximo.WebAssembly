using System.Collections.Generic;

namespace Aximo.WebAssembly.Interop
{
    public class JsObject : JsReference
    {

        internal protected JsObject(string handle, JsType type) : base(handle, type)
        {
        }

        public JsToken this[string name]
        {
            get
            {
                return Runtime.GetProperty(this, name);
            }
            set
            {
                Runtime.SetProperty(this, name, value);
            }
        }

    }

}