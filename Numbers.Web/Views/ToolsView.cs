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

        private IDialogContainer dialogContainer;
        private Control aboutDialog;
        private Control shareDialog;

        public ToolsView(IDialogContainer dialogService, string gameHash) :
            base("tools-panel")
        {
            this.dialogContainer = dialogService;

            Element permalinkElement = Document.CreateElement("a");
            permalinkElement.ClassName = "permalink";
            permalinkElement.SetAttribute("href", AppendHash(Window.Location.Href, gameHash));
            permalinkElement.TextContent = "permalink";

            Label shareLabel = new Label("share-label") { Text = "\u2764 share" };
            Label aboutLabel = new Label("about-label") { Text = "about" };

            shareLabel.HtmlElement.AddEventListener("mousedown", OnShareMouseDown, false);
            aboutLabel.HtmlElement.AddEventListener("mousedown", OnAboutMouseDown, false);

            this.HtmlElement.AppendChild(permalinkElement);
            this.AppendChild(shareLabel);
            this.AppendChild(aboutLabel);
        }

        private void OnShareMouseDown(Event e)
        {
            if (shareDialog == null)
            {
                shareDialog = CreateShareDialog();
            }

            dialogContainer.ShowDialog(shareDialog, 500, 300);
        }

        private void OnAboutMouseDown(Event e)
        {
            if (aboutDialog == null)
            {
                aboutDialog = CreateAboutDialog();
            }

            dialogContainer.ShowDialog(aboutDialog, 300, 150);
        }

        private Control CreateShareDialog()
        {
            Element iframe = Document.CreateElement("iframe");
            iframe.SetAttribute("src", "http://expando.github.io/add/?u=http%3A%2F%2Fyuvaltz.github.io%2Fnumbers&t=A%20numberful%20game");
            iframe.SetAttribute("frameborder", "0");
            iframe.SetAttribute("frametransparency", "1");
            iframe.SetAttribute("width", "500");
            iframe.SetAttribute("height", "300");

            Control dialog = new Control("dialog", "share");
            dialog.HtmlElement.AppendChild(iframe);

            return dialog;
        }

        private static Control CreateAboutDialog()
        {
            return new Control("dialog", "about")
            {
                new Label("about-dialog-header") { Text = "Numbers" },
                new Label("about-dialog-text") { Text = "Version 1.0" },
                new Link { Text = "Source on GitHub", Href = "http://www.github.com/yuvaltz/Numbers" },
            };
        }

        private static string AppendHash(string location, string hash)
        {
            int index = location.LastIndexOf("#");
            return String.Format("{0}#{1}", index == -1 ? location : location.Substring(0, index), hash);
        }
    }
}
