using System;
using System.Collections.Generic;
using System.Text;
using Numbers.Web.Controls;
using System.Html;
using Numbers.Web.Transitions;

namespace Numbers.Web.Controls
{
    public interface IDialogContainer
    {
        void ShowDialog(Control dialog, int dialogWidth, int dialogHeight);
    }

    public class DialogContainer : Control, IDialogContainer
    {
        private Control currentDialog;
        private int dialogWidth;
        private int dialogHeight;

        private ITransition appearTransition;
        private ITransition disappearTransition;

        public DialogContainer() :
            base("dialog-container")
        {
            this.HtmlElement.Style.Opacity = "0";
            this.HtmlElement.Style.Visibility = "hidden";

            appearTransition = new SequentialTransition(
                new Keyframe(this.HtmlElement, "visibility", "visible"),
                new Transition(this.HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(200, TimingCurve.EaseIn)));

            disappearTransition = new SequentialTransition(
                new Transition(this.HtmlElement, "opacity", new DoubleValueBounds(1, 0), new TransitionTiming(200, TimingCurve.EaseIn)),
                new Keyframe(this.HtmlElement, "visibility", "hidden"));

            Window.AddEventListener("touchstart", OnPointerDown, false);
            Window.AddEventListener("mousedown", OnPointerDown, false);

            Window.AddEventListener("resize", UpdateLayout);
        }

        public override void Dispose()
        {
            base.Dispose();

            Window.RemoveEventListener("touchstart", OnPointerDown, false);
            Window.RemoveEventListener("mousedown", OnPointerDown, false);

            Window.RemoveEventListener("resize", UpdateLayout);
        }

        public void ShowDialog(Control dialog, int dialogWidth, int dialogHeight)
        {
            if (currentDialog != null)
            {
                RemoveChild(currentDialog);
            }

            this.currentDialog = dialog;
            this.dialogWidth = dialogWidth;
            this.dialogHeight = dialogHeight;

            currentDialog.HtmlElement.Style.Opacity = "0";

            AppendChild(currentDialog);

            UpdateLayout();

            ITransition dialogAppearTransition = new ParallelTransition(
                new Transition(currentDialog.HtmlElement, "opacity", new DoubleValueBounds(0, 1), new TransitionTiming(200), 200),
                new Transition(currentDialog.HtmlElement, "margin", new ValueBounds("-20px 0px 0px 0px", "0px 0px 0px 0px"), new TransitionTiming(400, TimingCurve.EaseOut), 200));

            disappearTransition.Stop();
            appearTransition.Start();
            dialogAppearTransition.Start();
        }

        private void UpdateLayout()
        {
            if (currentDialog == null)
            {
                return;
            }

            currentDialog.Left = (Window.InnerWidth - dialogWidth) / 2;
            currentDialog.Top = (Window.InnerHeight - dialogHeight) / 2;
        }

        private void OnPointerDown(Event e)
        {
            EventTarget target = e.Target ?? e.GetSrcElement();

            if (target != this.HtmlElement || currentDialog == null)
            {
                return;
            }

            appearTransition.Stop();
            disappearTransition.Start();

            e.PreventDefault();
        }
    }
}
