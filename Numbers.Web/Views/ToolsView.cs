using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Numbers.Web.Controls;
using System.Html;
using System.Web;

namespace Numbers.Web.Views
{
    public class ToolsView : Control
    {
        private class ShareService
        {
            public int ImageIndex { get; private set; }
            public string Header { get; private set; }
            public string UrlFormat { get; private set; }

            public ShareService(int imageIndex, string header, string urlFormat)
            {
                this.ImageIndex = imageIndex;
                this.Header = header;
                this.UrlFormat = urlFormat;
            }
        }

        private static readonly ShareService[] ShareServices = new[]
        {
            new ShareService(0, "Facebook", "http://www.facebook.com/share.php?u={0}"),
            new ShareService(1, "Twitter", "http://twitter.com/share?text={2}&url={0}&hashtags=Numbers"),
            new ShareService(2, "Google+", "http://plus.google.com/share?url={0}"),
            new ShareService(3, "LinkedIn", "http://www.linkedin.com/shareArticle?mini=true&url={0}&title={1}&summary={2}"),
            new ShareService(4, "Pinterest", "http://pinterest.com/pin/create/button/?url={0}&media={3}&description={1}%20-%20{2}"),
            new ShareService(5, "Tumblr", "http://www.tumblr.com/share?v=3&u={0}&t={1}&s={2}"),
            new ShareService(6, "StumbleUpon", "http://www.stumbleupon.com/submit?url={0}&title={1}"),
            new ShareService(7, "Reddit", "http://reddit.com/submit?url={0}&title={1}"),
            new ShareService(8, "Delicious", "http://delicious.com/post?url={0}&title={1}%20-%20{2}"),
            new ShareService(9, "Digg", "http://digg.com/submit?phase=2&url={0}&title={1}&bodytext={2}"),
            new ShareService(10, "Blogger", "http://www.blogger.com/blog_this.pyra?t={1}&u={0}"),
            new ShareService(11, "Email", "mailto:?subject={1}&body={2}%20-%20{0}"),
        };

        public const int Height = 32;

        private string gameHash;
        public string GameHash
        {
            get { return gameHash; }
            set
            {
                gameHash = value;
                permalinkElement.SetAttribute("href", AppendHash(Window.Location.Href, gameHash));
            }
        }

        private bool isVisible;
        public bool IsVisible
        {
            get { return isVisible; }
            set
            {
                isVisible = value;
                HtmlElement.Style.Visibility = isVisible ? "visible" : "collapse";
                likeButton.HtmlElement.Style.Display = isVisible ? "initial" : "none";
            }
        }

        private IDialogContainer dialogContainer;
        private Statistics statistics;

        private Element permalinkElement;
        private Control likeButton;
        private Control aboutDialog;
        private Control shareDialog;

        private bool shareTooltipAdded;
        private bool shareTooltipRemoved;

        public ToolsView(IDialogContainer dialogContainer, Statistics statistics) :
            base("tools-panel")
        {
            this.dialogContainer = dialogContainer;
            this.statistics = statistics;

            isVisible = true;

            permalinkElement = Document.CreateElement("a");
            permalinkElement.ClassName = "permalink";
            permalinkElement.TextContent = "permalink";

            likeButton = new Control("fb-like");
            likeButton.HtmlElement.SetAttribute("data-href", DocumentExtensions.GetMetaPropertyValue("og:url"));
            likeButton.HtmlElement.SetAttribute("data-layout", "standard");
            likeButton.HtmlElement.SetAttribute("data-action", "like");
            likeButton.HtmlElement.SetAttribute("data-show-faces", "true");
            likeButton.HtmlElement.SetAttribute("data-share", "false");

            Label shareLabel = new Label("share-label") { Text = "\u2764 share" };
            Label aboutLabel = new Label("about-label") { Text = "about" };

            shareLabel.HtmlElement.AddEventListener("mousedown", OnShareMouseDown, false);
            aboutLabel.HtmlElement.AddEventListener("mousedown", OnAboutMouseDown, false);

            this.AppendChild(likeButton);
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

            if (shareTooltipRemoved)
            {
                dialogContainer.ShowDialog(shareDialog, 500, 300);
            }
            else if (!shareTooltipAdded)
            {
                Tooltip tooltip = new Tooltip("Thank you!", Direction.Bottom, 20) { Top = -48 };
                tooltip.HtmlElement.Style.Right = "39px";

                AppendChild(tooltip);
                shareTooltipAdded = true;

                tooltip.StartAppearAnimation();

                Window.SetTimeout(() =>
                {
                    tooltip.StartDisappearAnimation();
                    dialogContainer.ShowDialog(shareDialog, 500, 300);
                }, 2000);

                Window.SetTimeout(() =>
                {
                    RemoveChild(tooltip);
                    shareTooltipRemoved = true;
                }, 2000 + Tooltip.DisappearDuration);
            }
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
            string title = HttpUtility.UrlPathEncode(DocumentExtensions.GetMetaPropertyValue("og:title"));
            string description = HttpUtility.UrlPathEncode(DocumentExtensions.GetMetaPropertyValue("og:description"));
            string image = HttpUtility.UrlEncode(DocumentExtensions.GetMetaPropertyValue("og:image"));
            string url = HttpUtility.UrlEncode("http://git.io/numbers");

            Control dialog = new Control("dialog", "share");

            int top = 0;

            foreach (ShareService shareService in ShareServices)
            {
                Control buttonImage = new Control("share-button-image");
                buttonImage.HtmlElement.Style.BackgroundPosition = String.Format("{0}px 0px", -32 * shareService.ImageIndex);

                Control button = new Control("share-button")
                {
                    buttonImage,
                    new Label("share-button-label") { Text = shareService.Header }
                };

                string shareServiceHeader = shareService.Header;
                string shareServiceUrl = String.Format(shareService.UrlFormat, url, title, description, image);

                button.HtmlElement.AddEventListener("click", () =>
                {
                    statistics.ReportShare(shareServiceHeader);
                    Window.Open(shareServiceUrl);
                });

                button.Top = top;
                top += 40;

                dialog.AppendChild(button);
            };

            return dialog;
        }

        private static Control CreateAboutDialog()
        {
            return new Control("dialog", "about")
            {
                new Label("about-dialog-header") { Text = "Numbers" },
                new Label("about-dialog-text") { Text = String.Format("Version {0}", AssemblyInfo.Version) },
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
