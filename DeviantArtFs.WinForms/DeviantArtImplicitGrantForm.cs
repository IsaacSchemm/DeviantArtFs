﻿using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace DeviantArtFs.WinForms {
    public class DeviantArtImplicitGrantForm : Form {
        private readonly string _state;

        public string AccessToken { get; private set; }
        public DateTimeOffset? ExpiresAt { get; private set; }

        public DeviantArtImplicitGrantForm(int clientId, Uri callbackUrl, IEnumerable<string> scopes = null) {
            _state = Guid.NewGuid().ToString();

            this.Width = 435;
            this.Height = 750;

            var webBrowser1 = new WebBrowser {
                Dock = DockStyle.Fill,
                ScriptErrorsSuppressed = true
            };
            this.Controls.Add(webBrowser1);

            StringBuilder sb = new StringBuilder();
            sb.Append($"response_type=token&");
            sb.Append($"state={WebUtility.UrlEncode(_state)}&");
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
                    if (e.Url.Fragment.Length == 0) return;
                    var psd = QueryHelpers.ParseQuery(e.Url.Fragment.Substring(1));
                    if (!psd.TryGetValue("access_token", out StringValues access_token)) return;
                    if (!psd.TryGetValue("token_type", out StringValues token_type)) return;
                    if (!psd.TryGetValue("state", out StringValues state)) return;
                    if (state == _state && token_type == "bearer") {
                        AccessToken = access_token;
                        if (psd.TryGetValue("expires_in", out StringValues expires_in)) {
                            if (double.TryParse(expires_in, out double expsec)) {
                                ExpiresAt = DateTimeOffset.Now.AddSeconds(expsec);
                            }
                        }
                        webBrowser1.Navigate("about:blank");
                    }
                } else if (e.Url.AbsoluteUri == "about:blank") {
                    DialogResult = DialogResult.OK;
                } else if (e.Url.AbsolutePath == "/") {
                    // oauth flow bug workaround
                    webBrowser1.Navigate(startUrl);
                }
            };

            webBrowser1.NewWindow += (o, e) => {

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
