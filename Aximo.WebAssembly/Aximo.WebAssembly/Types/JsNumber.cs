namespace WebAssembly
{
    public class JsNumber : JsLiteral
    {
        public double Value;

        public JsNumber(double value) : base(JsType.Number)
        {
            Value = value;
        }

        public JsNumber(int value) : base(JsType.Number)
        {
            Value = value;
        }

        public override string ToJs()
        {
            return $"new JsToken({JsTypeNameExpression}, {Value})";
        }
    }

}