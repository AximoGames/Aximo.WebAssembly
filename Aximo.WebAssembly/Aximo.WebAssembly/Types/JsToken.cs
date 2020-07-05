
using System;

namespace WebAssembly
{
    public abstract class JsToken
    {
        public JsType Type;

        public JsToken(JsType type)
        {
            Type = type;
        }

        internal static JsToken Parse(string handleStr)
        {
            if (string.IsNullOrEmpty(handleStr))
                return null;

            Console.WriteLine("TOKEN:" + handleStr);

            var ar = handleStr.Split('|');
            Enum.TryParse<JsType>(ar[0], true, out var type);
            var handle = ar[1];
            switch (type)
            {
                case JsType.Array:
                    return new JsArrayReference(handle, int.Parse(ar[2]));
                case JsType.Element:
                    return new JsDomReference(handle);
                case JsType.Function:
                    return new JsDelegate(handle);
                case JsType.Number:
                    return new JsNumber(double.Parse(handle));
                case JsType.Boolean:
                    return new JsBoolean(bool.Parse(handle));
                case JsType.String:
                    return new JsString(handle); // TODO: | char
                default:
                    return new JsObject(handle, type);
            }
        }

        internal static TRef Parse<TRef>(string handleStr)
            where TRef : JsToken
        {
            try
            {
                var jsRef = Parse(handleStr);
                return (TRef)jsRef;
            }
            catch
            {
                Console.WriteLine($"Expected: {typeof(TRef)}. handleStr: {handleStr}");
                throw;
            }
        }

        public abstract string ToJs();

        private protected string JsTypeNameExpression => "'" + Type.ToString().ToLower() + "'";

        public static implicit operator string(JsToken value) => ((JsString)value).Value;
        public static implicit operator double(JsToken value) => ((JsNumber)value).Value;
        public static implicit operator bool(JsToken value) => ((JsBoolean)value).Value;
        public static implicit operator int(JsToken value) => (int)Math.Round(((JsNumber)value).Value);

    }

}