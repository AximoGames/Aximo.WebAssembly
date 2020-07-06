using System.Data.Common;

namespace Aximo.WebAssembly.Interop
{
    public class JsString : JsLiteral
    {
        public string Value;

        public JsString(string value) : base(JsType.String)
        {
            Value = value;
        }

        public static implicit operator string(JsString v)
        {
            return v.Value;
        }

        public static implicit operator JsString(string v)
        {
            return new JsString(v);
        }

        public override string ToJs()
        {
            return $"new JsToken({JsTypeNameExpression}, '{Value}')";
        }
    }

}