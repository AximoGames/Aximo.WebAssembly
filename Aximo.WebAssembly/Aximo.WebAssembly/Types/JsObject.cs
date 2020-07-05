using System.Collections.Generic;

namespace WebAssembly
{
    public class JsObject : JsReference
    {

        internal JsObject(string handle, JsType type) : base(handle, type)
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