using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAssembly;

namespace Wep.WebAdmin
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
