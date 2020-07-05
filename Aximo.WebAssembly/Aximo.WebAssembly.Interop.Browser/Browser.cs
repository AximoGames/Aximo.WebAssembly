using System;
using System.Collections.Generic;
using System.Text;

namespace Aximo.WebAssembly.Interop.Browser
{
    public static class Browser
    {
        public static JsDomReference GetElementById(string id)
        {
            return Runtime.GetObject<JsDomReference>($"document.getElementById('{id}')");
        }
    }

}
