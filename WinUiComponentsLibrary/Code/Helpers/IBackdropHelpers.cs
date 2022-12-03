using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiComponentsLibrary.Code.Helpers
{
    public interface IBackdropHelpers
    {
        bool TrySetMicaBackdrop();
        bool TrySetAcrylicBackdrop();
        void Window_Activated(object sender, WindowActivatedEventArgs args);
        void Window_Closed(object sender, WindowEventArgs args);
        void Window_ThemeChanged(FrameworkElement sender, object args);
        void SetConfigurationSourceTheme();
        void SetBackdrop(BackdropType type);
        void InitializeBackground();
    }
}
