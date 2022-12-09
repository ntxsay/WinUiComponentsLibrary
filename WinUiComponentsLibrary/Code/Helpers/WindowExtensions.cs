using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinUiComponentsLibrary.Views.Fenetres;

namespace WinUiComponentsLibrary.Code.Helpers
{
    public static class WindowExtensions
    {
        private static bool ShowWindow(Window window, Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD nCmdShow)
        {
            Windows.Win32.Foundation.HWND hWnd = new Windows.Win32.Foundation.HWND(window.GetWindowHandle());
            return Windows.Win32.PInvoke.ShowWindow(hWnd, nCmdShow);
        }

        public static bool MinimizeWindow(this Window window) => ShowWindow(window, Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD.SW_MINIMIZE); //6
        public static bool MaximizeWindow(this Window window) => ShowWindow(window, Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD.SW_MAXIMIZE); //3
        public static bool HideWindow(this Window window) => ShowWindow(window, Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD.SW_HIDE); //0
        public static bool ShowWindow(this Window window) => ShowWindow(window, Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD.SW_NORMAL); //5
        public static bool RestoreWindow(this Window window) => ShowWindow(window, Windows.Win32.UI.WindowsAndMessaging.SHOW_WINDOW_CMD.SW_RESTORE); //9

        /// <summary>
        /// Sets the width and height of the window in device-independent pixels.
        /// </summary>
        /// <param name="window">Window to set the size for.</param>
        /// <param name="width">Width of the window in device-independent units.</param>
        /// <param name="height">Height of the window in device-independent units.</param>
        public static void SetWindowSize(this Window window, double width, double height)
        {
            var scale = WindowHelpers.GetScaleAdjustment(window);
            window.GetAppWindow().Resize(new Windows.Graphics.SizeInt32((int)(width * scale), (int)(height * scale)));
        }

        /// <summary>
        /// Gets the AppWindow from the handle
        /// </summary>
        /// <param name="window"></param>
        /// <returns></returns>
        public static AppWindow GetAppWindow(this Microsoft.UI.Xaml.Window window) => GetAppWindowFromWindowHandle(window.GetWindowHandle());

        /// <summary>
        /// Gets the native HWND pointer handle for the window
        /// </summary>
        /// <param name="window">The window to return the handle for</param>
        /// <returns>HWND handle</returns>
        public static IntPtr GetWindowHandle(this Microsoft.UI.Xaml.Window window)
            => window is null ? throw new ArgumentNullException(nameof(window)) : WinRT.Interop.WindowNative.GetWindowHandle(window);

        /// <summary>
        /// Gets the AppWindow from an HWND
        /// </summary>
        /// <param name="hwnd"></param>
        /// <returns>AppWindow</returns>
        public static AppWindow GetAppWindowFromWindowHandle(IntPtr hwnd)
        {
            if (hwnd == IntPtr.Zero)
                throw new ArgumentNullException(nameof(hwnd));
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            return AppWindow.GetFromWindowId(windowId);
        }

        /// <summary>
        /// Activates the window and displays it in its current size and position.
        /// </summary>
        /// <param name="window">Window</param>
        /// <returns><c>true</c> if the window was previously visible, or <c>false</c> if the window was previously hidden.</returns>
        //public static bool Show(this Microsoft.UI.Xaml.Window window) => HwndExtensions.ShowWindow(GetWindowHandle(window));

        /// <summary>
        /// Hides the window and activates another window.
        /// </summary>
        /// <param name="window">Window</param>
        /// <returns><c>true</c> if the window was previously visible, or <c>false</c> if the window was previously hidden.</returns>
        //public static bool Hide(this Microsoft.UI.Xaml.Window window) => HwndExtensions.HideWindow(GetWindowHandle(window));
    }
}
