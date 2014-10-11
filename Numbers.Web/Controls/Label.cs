using System;
using System.Linq;

namespace Numbers.Web.Controls
{
    public class Label : Control
    {
        public string Text
        {
            get { return HtmlElement.TextContent; }
            set { HtmlElement.TextContent = value; }
        }

        public Label(params string[] classesName) :
            base(classesName.Concat(new [] { "label" }).ToArray())
        {
            //
        }
    }
}
