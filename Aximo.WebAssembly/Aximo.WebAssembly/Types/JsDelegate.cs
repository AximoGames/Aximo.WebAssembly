namespace WebAssembly
{
    public class JsDelegate : JsReference
    {
        internal JsDelegate(string handle) : base(handle, JsType.Function)
        {
        }
    }

}