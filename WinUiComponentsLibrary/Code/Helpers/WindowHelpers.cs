using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinRT.Interop;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace WinUiComponentsLibrary.Code.Helpers
{
    public enum Monitor_DPI_Type : int
    {
        MDT_Effective_DPI = 0,
        MDT_Angular_DPI = 1,
        MDT_Raw_DPI = 2,
        MDT_Default = MDT_Effective_DPI
    }

    public class WindowHelpers
    {
        [DllImport("Shcore.dll", SetLastError = true)]
        public static extern int GetDpiForMonitor(IntPtr hmonitor, Monitor_DPI_Type dpiType, out uint dpiX, out uint dpiY);

        public static double GetScaleAdjustment(Window window)
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(window);
            WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            DisplayArea displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
            IntPtr hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

            // Get DPI.
            int result = GetDpiForMonitor(hMonitor, Monitor_DPI_Type.MDT_Default, out uint dpiX, out uint _);
            if (result != 0)
            {
                throw new Exception("Could not get DPI for monitor.");
            }

            uint scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
            return scaleFactorPercent / 100.0;
        }

        public static AppWindow GetAppWindowForCurrentWindow(Window window)
        {
            IntPtr hWnd = WindowNative.GetWindowHandle(window);
            WindowId wndId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(wndId);
        }

        static public Window CreateWindow()
        {
            Window newWindow = new Window();
            TrackWindow(newWindow);
            return newWindow;
        }

        static public void TrackWindow(Window window)
        {
            if (window == null)
                return;

            window.Closed += (sender, args) => {
                _activeWindows.Remove(window);
            };
            _activeWindows.Add(window);
        }

        static public Window GetWindowForElement(UIElement element)
        {
            if (element.XamlRoot != null)
            {
                foreach (Window window in _activeWindows)
                {
                    if (element.XamlRoot == window.Content.XamlRoot)
                    {
                        return window;
                    }
                }
            }
            return null;
        }

        static public UIElement FindElementByName(UIElement element, string name)
        {
            if (element.XamlRoot != null && element.XamlRoot.Content != null)
            {
                var ele = (element.XamlRoot.Content as FrameworkElement).FindName(name);
                if (ele != null)
                {
                    return ele as UIElement;
                }
            }
            return null;
        }

        static public List<Window> ActiveWindows => _activeWindows;

        static private List<Window> _activeWindows = new ();

        /// <summary>
        /// Obtient une valeur booléenne indiquant si un <see cref="ContentDialog"/> est ouvert dans la fenêtre actuelle
        /// </summary>
        /// <param name="window">fenêtre actuelle</param>
        /// <returns></returns>
        public static bool IsAnyContentDialogOpened(Window window)
        {
            if (window == null) return false;
            var openedpopups = VisualTreeHelper.GetOpenPopups(window);
            foreach (var popup in openedpopups)
            {
                if (popup.Child is ContentDialog)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Obtient une valeur booléenne indiquant si un <see cref="ContentDialog"/> est ouvert dans le <see cref="XamlRoot"/> spécifié
        /// </summary>
        /// <param name="xamlRoot">XamlRoot spécifié</param>
        /// <returns></returns>
        public static bool IsAnyContentDialogOpened(XamlRoot xamlRoot)
        {
            if (xamlRoot == null) return false;
            var openedpopups = VisualTreeHelper.GetOpenPopupsForXamlRoot(xamlRoot);
            foreach (var popup in openedpopups)
            {
                if (popup.Child is ContentDialog)
                {
                    return true;
                }
            }

            return false;
        }
    }

}
