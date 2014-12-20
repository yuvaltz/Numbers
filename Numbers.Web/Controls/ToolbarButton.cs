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

            Window.AddEventListener("touchstart", OnPointerDown, false);
            Window.AddEventListener("touchend", OnPointerUp, false);
            Window.AddEventListener("touchmove", OnPointerMove, false);
            Window.AddEventListener("touchcancel", OnPointerUp, false);
            Window.AddEventListener("mousedown", OnPointerDown, false);
            Window.AddEventListener("mouseup", OnPointerUp, false);
            HtmlElement.AddEventListener("mouseleave", e => { if (IsPressed) { OnPointerUp(e); } }, false);
        }

        public override void Dispose()
        {
            base.Dispose();

            Window.RemoveEventListener("touchstart", OnPointerDown, false);
            Window.RemoveEventListener("touchend", OnPointerUp, false);
            Window.RemoveEventListener("touchmove", OnPointerMove, false);
            Window.RemoveEventListener("touchcancel", OnPointerUp, false);
            Window.RemoveEventListener("mousedown", OnPointerDown, false);
            Window.RemoveEventListener("mouseup", OnPointerUp, false);
        }

        private void OnPointerDown(Event e)
        {
            EventTarget target = e.Target ?? e.GetSrcElement();

            if (target != this.HtmlElement || IsPressed)
            {
                return;
            }

            IsPressed = true;

            if (IsEnabled && mouseDown != null)
            {
                Window.SetTimeout(mouseDown);
            }

            e.PreventDefault();
        }

        private void OnPointerMove(Event e)
        {
            EventTarget target = e.Target ?? e.GetSrcElement();

            if (target != this.HtmlElement)
            {
                return;
            }

            e.PreventDefault();
        }

        private void OnPointerUp(Event e)
        {
            EventTarget target = e.Target ?? e.GetSrcElement();

            if (target != this.HtmlElement || !IsPressed)
            {
                return;
            }

            IsPressed = false;

            if (IsEnabled && mouseUp != null)
            {
                Window.SetTimeout(mouseUp);
            }

            e.PreventDefault();
        }
    }
}
