using System.Collections.Generic;

namespace WebAssembly
{
    public class JsReference : JsToken
    {
        internal string Handle;

        internal static List<string> FreeReferences = new List<string>();

        internal JsReference(string handle, JsType type) : base(type)
        {
            Handle = handle;
        }

        ~JsReference()
        {
            Free();
        }

        public override string ToJs()
        {
            return $"new JsToken({JsTypeNameExpression}, '{Handle}')";
        }

        public void Free()
        {
            lock (FreeReferences)
                FreeReferences.Add(Handle);
        }

        internal static void ExecFree()
        {
            lock (FreeReferences)
            {
                foreach (var handle in FreeReferences)
                    Runtime.InvokeJS($"Interop.freeHandle('{handle}')", out _);

                FreeReferences.Clear();
            }
        }

    }

}