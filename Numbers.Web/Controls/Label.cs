using System;

namespace Numbers.Web.Controls
{
    public class Label : Control
    {
        public string Text
        {
            get { return HtmlElement.TextContent; }
            set { HtmlElement.TextContent = value; }
        }

        public Label(string className = String.Empty) :
            base("label", className)
        {
            //
        }
    }
}
