using System;
using System.Html;

namespace Numbers.Web.Controls
{
    public class ToolbarButton : Control
    {
        public bool IsEnabled { get; set; }

        public ToolbarButton(string className, string imageSource, Action clicked) :
            base("toolbar-button", className)
        {
            Element imageElement = Document.CreateElement("img");
            imageElement.SetAttribute("src", imageSource);

            HtmlElement.AppendChild(imageElement);
            AppendChild(new Control("toolbar-button-overlay"));

            IsEnabled = true;

            HtmlElement.AddEventListener("mousedown", e => { if (IsEnabled) { clicked(); } }, false);
        }
    }
}
