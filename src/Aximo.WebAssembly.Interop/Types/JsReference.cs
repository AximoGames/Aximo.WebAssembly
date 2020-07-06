using System.Collections.Generic;

namespace Aximo.WebAssembly.Interop
{
    public class JsReference : JsToken
    {
        public string Handle;

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

        public static void FreeHandles() // TODO: Make Internal
        {
            lock (FreeReferences)
            {
                foreach (var handle in FreeReferences)
                    Runtime.BaseRuntime.InvokeJS($"Interop.freeHandle('{handle}')");

                FreeReferences.Clear();
            }
        }

    }

}