using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Auth0WinUI.Views
{
    public sealed partial class LoginContentDialog : ContentDialog
    {
        public LoginContentDialog()
        {
            this.InitializeComponent();
        }

        private async void WebView2_NavigationCompleted(WebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            string isFullyLoaded = await sender.ExecuteScriptAsync("document.readyState === 'complete';");
            if (isFullyLoaded != null && isFullyLoaded == "true")
            {
                string html = await sender.ExecuteScriptAsync("document.documentElement.outerHTML;");
                if (!string.IsNullOrEmpty(html) && !string.IsNullOrWhiteSpace(html))
                {

                }
            }
        }
    }
}
