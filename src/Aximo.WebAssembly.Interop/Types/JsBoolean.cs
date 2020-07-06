namespace Aximo.WebAssembly.Interop
{
    public class JsBoolean : JsLiteral
    {
        public bool Value;

        public JsBoolean(bool value) : base(JsType.Boolean)
        {
            Value = value;
        }
        public override string ToJs()
        {
            return $"new JsToken({JsTypeNameExpression}, {Value.ToString().ToLower()})";
        }
    }

}