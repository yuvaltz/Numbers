using System;
using System.Html;
using System.Runtime.CompilerServices;

namespace Numbers.Web.Controls
{
    public class ToolbarButton : Control
    {
        public bool IsEnabled { get; set; }
        public bool IsPressed { get; private set; }

        private Action mouseDown;
        private Action mouseUp;

        public ToolbarButton(string className, string imageSource, Action mouseDown, Action mouseUp = null) :
            base("toolbar-button", className)
        {
            this.mouseDown = mouseDown;
            this.mouseUp = mouseUp;

            Element imageElement = Document.CreateElement("img");
            imageElement.SetAttribute("src", imageSource);

            HtmlElement.AppendChild(imageElement);
            AppendChild(new Control("toolbar-button-overlay"));

            IsEnabled = true;

            if (HtmlElement.IsTouchAvailable())
            {
                HtmlElement.AddEventListener("touchstart", OnMouseDown, false);
                HtmlElement.AddEventListener("touchend", OnMouseUp, false);
                HtmlElement.AddEventListener("touchmove", OnMouseLeave, false);
            }
            else
            {
                HtmlElement.AddEventListener("mousedown", OnMouseDown, false);
                HtmlElement.AddEventListener("mouseup", OnMouseUp, false);
                HtmlElement.AddEventListener("mouseleave", OnMouseLeave, false);
            }
        }

        private void OnMouseDown()
        {
            if (!IsPressed)
            {
                IsPressed = true;

                if (IsEnabled && mouseDown != null)
                {
                    mouseDown();
                }
            }
        }

        private void OnMouseUp()
        {
            if (IsPressed)
            {
                IsPressed = false;

                if (IsEnabled && mouseUp != null)
                {
                    mouseUp();
                }
            }
        }

        private void OnMouseLeave()
        {
            OnMouseUp();
        }
    }
}
