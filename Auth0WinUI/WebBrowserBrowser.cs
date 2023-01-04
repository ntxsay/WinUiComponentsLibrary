using Auth0WinUI.Views;
using IdentityModel.OidcClient.Browser;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Auth0WinUI
{
    /// <summary>
    /// Implements the <see cref="IBrowser"/> interface using <see cref="WebBrowser"/>.
    /// </summary>
    public class WebBrowserBrowser : IBrowser
    {
        private readonly Func<Window> _windowFactory;
        private readonly Func<LoginContentDialog> _contentDialogFactory;
        private readonly bool _shouldCloseWindow;
        private readonly XamlRoot xamlRoot;

        /// <summary>
        /// Create a new instance of <see cref="WebBrowserBrowser"/> with a custom Window factory and optional flag to indicate if the window should be closed.
        /// </summary>
        /// <param name="windowFactory">A function that returns a <see cref="Window"/> to be used for hosting the browser.</param>
        /// <param name="shouldCloseWindow"> Whether the Window should be closed or not after completion.</param>
        /// <example> 
        /// This sample shows how to call the <see cref="WebBrowserBrowser(Func&lt;Window&gt;, bool)"/> constructor.
        /// <code>
        /// Window ReturnWindow()
        /// {
        ///     return window; // your WPF application window where you want the login to pop up
        /// }
        /// var browser = new WebBrowserBrowser(ReturnWindow, shouldCloseWindow: false); // specify false if you want the window to remain open
        /// </code>
        /// </example>
        public WebBrowserBrowser(Func<Window> windowFactory, bool shouldCloseWindow = true)
        {
            _windowFactory = windowFactory;
            _shouldCloseWindow = shouldCloseWindow;
        }

        public WebBrowserBrowser(Func<LoginContentDialog> contentDialogFactory, bool shouldCloseWindow = true)
        {
            _contentDialogFactory = contentDialogFactory;
            _shouldCloseWindow = shouldCloseWindow;
        }

        /// <summary>
        /// Create a new instance of <see cref="WebBrowserBrowser"/> that will create a customized <see cref="Window"/> as needed.
        /// </summary>
        /// <param name="title">An optional <see cref="string"/> specifying the title of the form. Defaults to "Authenticating...".</param>
        /// <param name="width">An optional <see cref="int"/> specifying the width of the form. Defaults to 1024.</param>
        /// <param name="height">An optional <see cref="int"/> specifying the height of the form. Defaults to 768.</param>
        public WebBrowserBrowser(XamlRoot _xamlRoot, string title = "Authenticating...")
            : this(() => new LoginContentDialog
            {
                Name = "WebAuthentication",
                XamlRoot = _xamlRoot,
                Title = title,
                Width = _xamlRoot.Size.Width,
                Height = _xamlRoot.Size.Height
            })
        {
            xamlRoot= _xamlRoot;
        }

        /// <inheritdoc />
        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            LoginContentDialog contentDialog = _contentDialogFactory.Invoke();
            //contentDialog.webView2.Width = xamlRoot.Size.Width;
            //contentDialog.webView2.Height = xamlRoot.Size.Height;

            SemaphoreSlim signal = new SemaphoreSlim(0, 1);

            BrowserResult result = new BrowserResult
            {
                ResultType = BrowserResultType.UserCancel
            };

            contentDialog.Loaded += async (s, e) =>
            {
                try
                {
                    //contentDialog.Content = browser;
                    //old :  browser.Navigate(options.StartUrl);

                    await contentDialog.webView2.EnsureCoreWebView2Async();
                    //A tester : webView.Source = new Uri(loginUrlAdress);
                    contentDialog.webView2.CoreWebView2.Navigate(options.StartUrl);

                    await signal.WaitAsync();
                }
                finally
                {
                    if (_shouldCloseWindow)
                        contentDialog.Hide();
                    else
                        contentDialog.Content = null;
                }
            };

            contentDialog.Closed += (s, e) =>
            {
                signal.Release();
            };

            contentDialog.webView2.NavigationCompleted += (s, e) =>
            {
                if (contentDialog.webView2.Source.ToString().StartsWith(options.EndUrl))
                {
                    result.ResultType = BrowserResultType.Success;
                    result.Response = contentDialog.webView2.Source.ToString();
                    signal.Release();
                }
            };

            await contentDialog.ShowAsync();

            //try
            //{
            //    contentDialog.Content = browser;
            //    await contentDialog.ShowAsync();

            //    //old :  browser.Navigate(options.StartUrl);

            //    await browser.EnsureCoreWebView2Async();
            //    //A tester : webView.Source = new Uri(loginUrlAdress);
            //    browser.CoreWebView2.Navigate(options.StartUrl);

            //    await signal.WaitAsync();
            //}
            //finally
            //{
            //    if (_shouldCloseWindow)
            //        contentDialog.Hide();
            //    else
            //        contentDialog.Content = null;
            //}

            return result;
        }
    }
}
