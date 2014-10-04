using System;
using System.Collections.Generic;
using System.Text;
using Numbers.Web.Controls;
using System.Html;

namespace Numbers.Web.Views
{
    public class ToolsView : Control
    {
        public const int Width = 296;
        public const int Height = 32;

        public ToolsView(string gameHash) :
            base("tools-panel")
        {
            Element permalinkElement = Document.CreateElement("a");
            permalinkElement.ClassName = "permalink";
            permalinkElement.SetAttribute("href", AppendHash(Window.Location.Href, gameHash));
            permalinkElement.TextContent = "permalink";

            Label shareLabel = new Label("share-label") { Text = "\u2764 share" };
            Label aboutLabel = new Label("about-label") { Text = "about" };

            this.HtmlElement.AppendChild(permalinkElement);
            this.AppendChild(shareLabel);
            this.AppendChild(aboutLabel);
        }

        private static string AppendHash(string location, string hash)
        {
            int index = location.LastIndexOf("#");
            return String.Format("{0}#{1}", index == -1 ? location : location.Substring(0, index), hash);
        }
    }
}
