using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics;

namespace WinUiComponentsLibrary.Code.Helpers
{
    //https://github.com/ghost1372/SettingsUI/blob/main/src/SettingsUI/Tools/Helpers/WindowHelper/WindowHelper.ReSizeWindow.cs
    public static class WindowMinMaxSize
    {
        private static NativeMethods.WinProc newWndProc = null;
        private static IntPtr oldWndProc = IntPtr.Zero;

        /// <summary>
        /// Default is 900
        /// </summary>
        public static int MinWindowWidth { get; set; } = 900;
        /// <summary>
        /// Default is 1800
        /// </summary>
        public static int MaxWindowWidth { get; set; } = 1800;
        /// <summary>
        /// Default is 600
        /// </summary>
        public static int MinWindowHeight { get; set; } = 600;
        /// <summary>
        /// Default is 1600
        /// </summary>
        public static int MaxWindowHeight { get; set; } = 1600;


        public static void RegisterWindowMinMax(this Window window)
        {
            //Get the Window's HWND
            var hwnd = WindowHelpers.GetWindowHandleForCurrentWindow(window);

            newWndProc = new NativeMethods.WinProc(WndProc);
            oldWndProc = NativeMethods.SetWindowLongPtr(hwnd, NativeMethods.WindowLongIndexFlags.GWL_WNDPROC, newWndProc);
        }

        public static void RegisterWindowMinMax(this Window window, SizeInt32 newMinSizeConstraint, SizeInt32 newMaxSizeConstraint)
        {
            MinWindowWidth = newMinSizeConstraint.Width;
            MinWindowHeight = newMinSizeConstraint.Height;
            MaxWindowWidth = newMaxSizeConstraint.Width;
            MaxWindowHeight = newMaxSizeConstraint.Height;
            //Get the Window's HWND
            var hwnd = WindowHelpers.GetWindowHandleForCurrentWindow(window);

            newWndProc = new NativeMethods.WinProc(WndProc);
            oldWndProc = NativeMethods.SetWindowLongPtr(hwnd, NativeMethods.WindowLongIndexFlags.GWL_WNDPROC, newWndProc);
        }

        private static IntPtr WndProc(IntPtr hWnd, NativeMethods.WindowMessage Msg, IntPtr wParam, IntPtr lParam)
        {
            switch (Msg)
            {
                case NativeMethods.WindowMessage.WM_GETMINMAXINFO:
                    var dpi = NativeMethods.GetDpiForWindow(hWnd);
                    var scalingFactor = (float)dpi / 96;

                    var minMaxInfo = Marshal.PtrToStructure<NativeMethods.MINMAXINFO>(lParam);
                    minMaxInfo.ptMinTrackSize.x = (int)(MinWindowWidth * scalingFactor);
                    //minMaxInfo.ptMaxTrackSize.x = (int)(MaxWindowWidth * scalingFactor);
                    minMaxInfo.ptMinTrackSize.y = (int)(MinWindowHeight * scalingFactor);
                    //minMaxInfo.ptMaxTrackSize.y = (int)(MaxWindowHeight * scalingFactor);

                    Marshal.StructureToPtr(minMaxInfo, lParam, true);
                    break;

            }
            return NativeMethods.CallWindowProc(oldWndProc, hWnd, Msg, wParam, lParam);
        }

        /// <summary>
        /// Set Window Width and Height
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void SetWindowSize(this Window window, int width, int height)
        {
            var hwnd = WindowHelpers.GetWindowHandleForCurrentWindow(window);
            // Win32 uses pixels and WinUI 3 uses effective pixels, so you should apply the DPI scale factor
            var dpi = NativeMethods.GetDpiForWindow(hwnd);
            var scalingFactor = (float)dpi / 96;
            width = (int)(width * scalingFactor);
            height = (int)(height * scalingFactor);

            NativeMethods.SetWindowPos(hwnd, NativeMethods.HWND_TOP, 0, 0, width, height, NativeMethods.SetWindowPosFlags.SWP_NOMOVE);
        }
    }
}
