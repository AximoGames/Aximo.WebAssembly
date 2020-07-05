namespace WebAssembly
{
    public class JsArrayReference : JsReference
    {

        public int Length;

        internal JsArrayReference(string handle, int length) : base(handle, JsType.Array)
        {
            Length = length;
        }

        public JsToken this[int index]
        {
            get
            {
                return Runtime.GetArrayElement(Handle, index);
            }
            set
            {
                Runtime.SetArrayElement(Handle, index, value);
            }
        }

        public JsToken[] ToArray()
        {
            var ar = new JsToken[Length];
            for (var i = 0; i < Length; i++)
            {
                ar[i] = this[i];
            }
            return ar;
        }

    }

}