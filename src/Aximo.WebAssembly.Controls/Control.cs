using Aximo.WebAssembly.Interop;
using Aximo.WebAssembly.Interop.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aximo.WebAssembly.Controls
{
    public class HtmlControl
    {

        private JsDomReference Dom;

        public HtmlControl(string tagName)
        {
            TagName = tagName;
            Dom = Runtime.GetObject<JsDomReference>($"document.createElement('{tagName}')");
        }

        public HtmlControl(JsDomReference dom)
        {
            TagName = dom.TagName;
        }

        public string TagName { get; }

        public void SetClick(Action act)
        {
        }

    }
}
