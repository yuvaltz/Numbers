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

        public ToolbarButton(string className, Action mouseDown = null, Action mouseUp = null) :
            base("toolbar-button", className)
        {
            this.mouseDown = mouseDown;
            this.mouseUp = mouseUp;

            IsEnabled = true;

            if (WindowExtensions.IsTouchAvailable())
            {
                Window.AddEventListener("touchstart", OnPointerDown, false);
                Window.AddEventListener("touchend", OnPointerUp, false);
            }
            else
            {
                Window.AddEventListener("mousedown", OnPointerDown, false);
                Window.AddEventListener("mouseup", OnPointerUp, false);
                Window.AddEventListener("mouseleave", OnPointerUp, false);
            }
        }

        private void OnPointerDown(Event e)
        {
            EventTarget target = e.Target ?? e.GetSrcElement();

            if (target != this.HtmlElement)
            {
                return;
            }

            IsPressed = true;

            if (IsEnabled && mouseDown != null)
            {
                mouseDown();
            }

            e.PreventDefault();
        }

        private void OnPointerUp(Event e)
        {
            EventTarget target = e.Target ?? e.GetSrcElement();

            if (target != this.HtmlElement)
            {
                return;
            }

            IsPressed = false;

            if (IsEnabled && mouseUp != null)
            {
                mouseUp();
            }

            e.PreventDefault();
        }
    }
}
