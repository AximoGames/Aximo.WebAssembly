using System.Linq;

namespace Aximo.WebAssembly.Interop
{
    public static class JsTokenExtensions
    {
        public static string ToNullableJs(this JsToken token)
        {
            if (token == null)
                return "null";
            return token.ToJs();
        }

        public static string ToNullableJs(this JsToken[] tokens)
        {
            if (tokens == null)
                return "[]";
            return "[" + string.Join(", ", tokens.Select(t => t.ToNullableJs())) + "]";
        }
    }

}