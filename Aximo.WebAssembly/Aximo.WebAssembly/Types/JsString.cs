namespace WebAssembly
{
    public class JsString : JsLiteral
    {
        public string Value;

        public JsString(string value) : base(JsType.String)
        {
            Value = value;
        }

        public override string ToJs()
        {
            return $"new JsToken({JsTypeNameExpression}, '{Value}')";
        }
    }

}