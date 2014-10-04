using System;
using System.Collections.Generic;
using System.Text;
using System.Html;

namespace Numbers.Web.Controls
{
    public class Link : Control
    {
        public string Text
        {
            get { return linkElement.TextContent; }
            set { linkElement.TextContent = value; }
        }

        public string Href
        {
            get { return linkElement.GetAttribute("href"); }
            set { linkElement.SetAttribute("href", value); }
        }

        private Element linkElement;

        public Link(string className = String.Empty) :
            base("link", className)
        {
            linkElement = Document.CreateElement("a");
            linkElement.SetAttribute("target", "_blank");

            HtmlElement.AppendChild(linkElement);
        }
    }
}
