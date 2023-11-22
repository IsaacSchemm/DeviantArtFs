using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace DeviantArtFs.WinForms {
    public class DeviantArtAuthorizationCodeForm : Form {
        public string Code { get; private set; }

        public DeviantArtAuthorizationCodeForm(int clientId, Uri callbackUrl, IEnumerable<string> scopes = null) {
            this.Width = 435;
            this.Height = 750;

            var webBrowser1 = new WebBrowser {
                Dock = DockStyle.Fill,
                ScriptErrorsSuppressed = true
            };
            this.Controls.Add(webBrowser1);

            StringBuilder sb = new StringBuilder();
            sb.Append($"response_type=code&");
            sb.Append($"client_id={clientId}&");
            sb.Append($"redirect_uri={callbackUrl}");
            if (scopes != null)
            {
                sb.Append($"&scope={WebUtility.UrlEncode(string.Join(" ", scopes))}");
            }
            string startUrl = "https://www.deviantart.com/oauth2/authorize?" + sb;

            this.Shown += (o, e) => {
                webBrowser1.Navigate(startUrl);
            };

            webBrowser1.Navigated += (o, e) => {
                if (e.Url.Authority == callbackUrl.Authority && e.Url.AbsolutePath == callbackUrl.AbsolutePath) {
                    int codeIndex = e.Url.Query.IndexOf("code=");
                    if (codeIndex > -1) {
                        string code = e.Url.Query.Substring(codeIndex + 5);
                        if (code.Contains("&")) code = code.Substring(0, code.IndexOf("&"));
                        Code = code;
                        DialogResult = DialogResult.OK;
                    }
                } else if (e.Url.AbsolutePath == "/") {
                    // oauth flow bug workaround
                    webBrowser1.Navigate(startUrl);
                }
            };

            webBrowser1.DocumentTitleChanged += (o, e) => {
                this.Text = webBrowser1.DocumentTitle;
            };

            webBrowser1.Navigating += (o, e) => {
                if (e.Url.OriginalString.StartsWith("javascript:void")) e.Cancel = true;
            };
        }
    }
}
