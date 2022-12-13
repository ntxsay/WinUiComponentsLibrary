// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Windows.Graphics;
using WinRT;
using WinRT.Interop;
using WinUiComponentsLibrary.Code;
using WinUiComponentsLibrary.Code.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiComponentsLibrary.Views.Fenetres
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class FluentWindow : Window, IBackdropHelpers
    {
        public readonly AppWindow m_AppWindow;
        private readonly Microsoft.UI.Windowing.OverlappedPresenter overlappedPresenter;

        public FluentWindow()
        {
            this.InitializeComponent();
            m_AppWindow = WindowHelpers.GetAppWindowForCurrentWindow(this);
            overlappedPresenter = Microsoft.UI.Windowing.OverlappedPresenter.Create();
            m_AppWindow.SetPresenter(overlappedPresenter);

            this.Activated += Window_Activated;
            this.Closed += Window_Closed;

            InitializeBackground();
        }



        #region Background
        public WindowActivationState WindowActivationState { get; private set; }
        BackdropType m_currentBackdrop;
        WindowsSystemDispatcherQueueHelper m_wsdqHelper;
        Microsoft.UI.Composition.SystemBackdrops.MicaController m_micaController;
        Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController m_acrylicController;
        Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration m_configurationSource;
        
        public void InitializeBackground()
        {
            m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();
        }
        
        public bool TrySetMicaBackdrop()
        {
            if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
            {
                // Hooking up the policy object
                m_configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
                ((FrameworkElement)this.Content).ActualThemeChanged += Window_ThemeChanged;

                // Initial configuration state.
                m_configurationSource.IsInputActive = true;
                SetConfigurationSourceTheme();

                m_micaController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();

                // Enable the system backdrop.
                // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
                m_micaController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
                m_micaController.SetSystemBackdropConfiguration(m_configurationSource);
                return true; // succeeded
            }

            return false; // Mica is not supported on this system
        }

        public bool TrySetAcrylicBackdrop()
        {
            if (Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController.IsSupported())
            {
                // Hooking up the policy object
                m_configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
                ((FrameworkElement)this.Content).ActualThemeChanged += Window_ThemeChanged;

                // Initial configuration state.
                m_configurationSource.IsInputActive = true;
                SetConfigurationSourceTheme();

                m_acrylicController = new Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController();

                // Enable the system backdrop.
                // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
                m_acrylicController.AddSystemBackdropTarget(this.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
                m_acrylicController.SetSystemBackdropConfiguration(m_configurationSource);
                return true; // succeeded
            }

            return false; // Acrylic is not supported on this system
        }

        public virtual void Window_Activated(object sender, WindowActivatedEventArgs args)
        {
            WindowActivationState = args.WindowActivationState;
            if (m_configurationSource != null)
                m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
        }

        public virtual void Window_Closed(object sender, WindowEventArgs args)
        {
            // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
            // use this closed window.
            if (m_micaController != null)
            {
                m_micaController.Dispose();
                m_micaController = null;
            }
            if (m_acrylicController != null)
            {
                m_acrylicController.Dispose();
                m_acrylicController = null;
            }
            this.Activated -= Window_Activated;
            m_configurationSource = null;
        }

        public void Window_ThemeChanged(FrameworkElement sender, object args)
        {
            if (m_configurationSource != null)
            {
                SetConfigurationSourceTheme();
            }
        }

        public void SetConfigurationSourceTheme()
        {
            switch (((FrameworkElement)this.Content).ActualTheme)
            {
                case ElementTheme.Dark: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
                case ElementTheme.Light: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
                case ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
            }
        }

        public void SetBackdrop(BackdropType type)
        {
            // Reset to default color. If the requested type is supported, we'll update to that.
            // Note: This sample completely removes any previous controller to reset to the default
            //       state. This is done so this sample can show what is expected to be the most
            //       common pattern of an app simply choosing one controller type which it sets at
            //       startup. If an app wants to toggle between Mica and Acrylic it could simply
            //       call RemoveSystemBackdropTarget() on the old controller and then setup the new
            //       controller, reusing any existing m_configurationSource and Activated/Closed
            //       event handlers.
            m_currentBackdrop = BackdropType.DefaultColor;

            if (m_micaController != null)
            {
                m_micaController.Dispose();
                m_micaController = null;
            }
            if (m_acrylicController != null)
            {
                m_acrylicController.Dispose();
                m_acrylicController = null;
            }
            //this.Activated -= Window_Activated;
            //this.Closed -= Window_Closed;
            ((FrameworkElement)this.Content).ActualThemeChanged -= Window_ThemeChanged;
            m_configurationSource = null;

            if (type == BackdropType.Mica)
            {
                if (TrySetMicaBackdrop())
                {
                    Debug.WriteLine("Mica");
                    m_currentBackdrop = type;
                }
                else
                {
                    // Mica isn't supported. Try Acrylic.
                    type = BackdropType.DesktopAcrylic;
                    Debug.WriteLine("  Mica isn't supported. Trying Acrylic.");
                }
            }

            if (type == BackdropType.DesktopAcrylic)
            {
                if (TrySetAcrylicBackdrop())
                {
                    Debug.WriteLine("Acrylic");
                    m_currentBackdrop = type;
                }
                else
                {
                    // Acrylic isn't supported, so take the next option, which is DefaultColor, which is already set.
                    Debug.WriteLine("  Acrylic isn't supported. Switching to default color.");
                }
            }
        }
        #endregion

        #region Window Size
        /// <summary>
        /// Gets or sets a value indicating whether the minimize button is visible
        /// </summary>
        public bool IsMinimizable
        {
            get => overlappedPresenter.IsMinimizable;
            set => overlappedPresenter.IsMinimizable = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the maximimze button is visible
        /// </summary>
        public bool IsMaximizable
        {
            get => overlappedPresenter.IsMaximizable;
            set => overlappedPresenter.IsMaximizable = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the window can be resized.
        /// </summary>
        public bool IsResizable
        {
            get => overlappedPresenter.IsResizable;
            set => overlappedPresenter.IsResizable = value;
        }

        /*
         * These are currently throwing
        /// <summary>
        /// Gets or sets a value indicating whether the window has a border or not.
        /// </summary>
        public bool HasBorder
        {
            get => overlappedPresenter.HasBorder;
            set => overlappedPresenter.SetBorderAndTitleBar(value, overlappedPresenter.HasTitleBar);
        }
        /// <summary>
        /// Gets or sets a value indicating whether the window is modal or not.
        /// </summary>
        public bool IsModal
        {
            get => overlappedPresenter.IsModal;
            set => overlappedPresenter.IsModal = value;
        }*/

        /// <summary>
        /// Gets or sets a value indicating whether this window is shown in task switchers.
        /// </summary>
        public bool IsShownInSwitchers
        {
            get => m_AppWindow.IsShownInSwitchers;
            set => m_AppWindow.IsShownInSwitchers = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this window is shown in task switchers.
        /// </summary>
        public bool IsAlwaysOnTop
        {
            get => overlappedPresenter.IsAlwaysOnTop;
            set => overlappedPresenter.IsAlwaysOnTop = value;
        }

        /// <summary>
        /// Gets or sets the presenter for the current window
        /// </summary>
        /// <seealso cref="PresenterKind"/>
        /// <seealso cref="PresenterChanged"/>
        public Microsoft.UI.Windowing.AppWindowPresenter Presenter
        {
            get => m_AppWindow.Presenter;
        }

        /// <summary>
        /// Gets or sets the presenter kind for the current window
        /// </summary>
        /// <seealso cref="Presenter"/>
        /// <seealso cref="PresenterChanged"/>
        public Microsoft.UI.Windowing.AppWindowPresenterKind PresenterKind
        {
            get => m_AppWindow.Presenter.Kind;
            set
            {
                if (value is Microsoft.UI.Windowing.AppWindowPresenterKind.Overlapped)
                    m_AppWindow.SetPresenter(overlappedPresenter);
                else
                    m_AppWindow.SetPresenter(value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the window
        /// </summary>
        public double Width
        {
            get { return m_AppWindow.Size.Width; }
            set
            {
                this.SetWindowSize(value, m_AppWindow.Size.Height);
            }
        }

        /// <summary>
        /// Gets or sets the height of the window
        /// </summary>
        public double Height
        {
            get { return m_AppWindow.Size.Height; }
            set
            {
                this.SetWindowSize(m_AppWindow.Size.Width, value);
            }
        }
        #endregion
    }
}
