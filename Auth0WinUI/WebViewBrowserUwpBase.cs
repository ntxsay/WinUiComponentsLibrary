using IdentityModel.OidcClient.Browser;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace Auth0WinUI
{
    /// <summary>
    /// Implements the <see cref="IBrowser"/> interface using the <see cref="WebView"/> control.
    /// </summary>
    public class WebViewBrowser : IBrowser
    {
        /// <inheritdoc />
        public Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            var tcs = new TaskCompletionSource<BrowserResult>();
            var currentAppView = ApplicationView.GetForCurrentView();

            RunOnNewView(async () => {
                var newAppView = CreateApplicationView();
                var webView = CreateWebView(Window.Current, options, tcs);
                //Old : webView.Navigate(new Uri(options.StartUrl));
                await webView.EnsureCoreWebView2Async();
                //A tester : webView.Source = new Uri(loginUrlAdress);
                webView.CoreWebView2.Navigate(options.StartUrl);
                await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newAppView.Id, ViewSizePreference.UseMinimum, currentAppView.Id, ViewSizePreference.UseMinimum);
            });

            return tcs.Task;
        }

        private async void RunOnNewView(DispatchedHandler function)
        {
            await CoreApplication.CreateNewView().Dispatcher.RunAsync(CoreDispatcherPriority.Normal, function);
        }

        private static ApplicationView CreateApplicationView()
        {
            var appView = ApplicationView.GetForCurrentView();
            appView.Title = "Authentication...";
            return appView;
        }

        private static WebView2 CreateWebView(Window window, BrowserOptions options, TaskCompletionSource<BrowserResult> tcs)
        {
            var webView = new WebView2();

            webView.NavigationStarting += (sender, e) =>
            {
                //Old : e.Uri.AbsoluteUri.StartsWith(options.EndUrl)
                if (e.Uri.StartsWith(options.EndUrl))
                {
                    tcs.SetResult(new BrowserResult { ResultType = BrowserResultType.Success, Response = e.Uri.ToString() });
                    window.Close();
                }
            };

            // There is no closed event so the best we can do is detect visibility. This means we close when they minimize too.
            window.VisibilityChanged += (sender, e) =>
            {
                if (!window.Visible && !tcs.Task.IsCompleted)
                {
                    tcs.SetResult(new BrowserResult { ResultType = BrowserResultType.UserCancel });
                    window.Close();
                }
            };

            window.Content = webView;
            window.Activate();

            return webView;
        }
    }
}
