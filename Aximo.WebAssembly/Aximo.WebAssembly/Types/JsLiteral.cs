namespace WebAssembly
{
    public abstract class JsLiteral : JsToken
    {
        protected JsLiteral(JsType type) : base(type)
        {
        }
    }

}