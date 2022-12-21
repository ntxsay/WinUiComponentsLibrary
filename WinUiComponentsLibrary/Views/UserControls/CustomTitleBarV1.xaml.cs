// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using WinUiComponentsLibrary.Code.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiComponentsLibrary.Views.UserControls
{
    public sealed partial class CustomTitleBarV1 : Grid
    {
        readonly Window window;
        readonly AppWindow appWindow;
        private bool _IsMiddleContentDraggable = true;
        public bool IsMiddleContentDraggable
        {
            get => _IsMiddleContentDraggable;
            set
            {
                if (_IsMiddleContentDraggable != value)
                {
                    _IsMiddleContentDraggable = value;
                    SetDragRegionForCustomTitleBar();
                }
            }
        }

        public readonly AppWindowTitleBar appWindowTitleBar;
        public CustomTitleBarV1()
        {
            this.InitializeComponent();
        }


        /// <summary>
        /// Efface le contenu de gauche
        /// </summary>
        public void ClearLeftContent() => this.LeftContentContainer.Children.Clear();

        /// <summary>
        /// Efface le contenu du milieu
        /// </summary>
        public void ClearMiddleContent() => this.MiddleContentContainer.Children.Clear();

        /// <summary>
        /// Ajoute un élément d'interface dans le contenu de gauche
        /// </summary>
        /// <param name="uIElement">Elément d'interface</param>
        public void AddElementToLeftContent(UIElement uIElement)
        {
            if (uIElement == null) return;
            this.LeftContentContainer.Children.Add(uIElement);
        }

        /// <summary>
        /// Ajoute un élément d'interface dans le contenu du milieu
        /// </summary>
        /// <param name="uIElement">Elément d'interface</param>
        public void AddElementToMiddleContent(UIElement uIElement)
        {
            if (uIElement == null) return;
            this.MiddleContentContainer.Children.Add(uIElement);
        }

        /// <summary>
        /// Ajoute un élément d'interface dans le contenu de droite
        /// </summary>
        /// <param name="uIElement">Elément d'interface</param>
        public void AddElementToRightContent(UIElement uIElement)
        {
            if (uIElement == null) return;
            this.RightContentContainer.Children.Add(uIElement);
        }

        public CustomTitleBarV1(Window _window, AppWindow _appWindow)
        {
            this.InitializeComponent();
            window = _window;
            appWindow = _appWindow;
            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                appWindowTitleBar = appWindow.TitleBar;
                appWindowTitleBar.ExtendsContentIntoTitleBar = true;
                appWindowTitleBar.ButtonBackgroundColor = Colors.Transparent;
                appWindowTitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                //appWindowTitleBar.ButtonForegroundColor = (app.Resources["PageSelectedBackground"] as SolidColorBrush).Color;  //Colors.Black;
                appWindowTitleBar.ButtonInactiveForegroundColor = Colors.Black;
            }
            else
            {
                // Title bar customization using these APIs is currently
                // supported only on Windows 11. In other cases, hide
                // the custom title bar element.
                this.Visibility = Visibility.Collapsed;

                // Show alternative UI for any functionality in
                // the title bar, such as search.
            }

            Loaded += UserControl_Loaded;
            SizeChanged += UserControl_SizeChanged;
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // Check to see if customization is supported.
            // Currently only supported on Windows 11.
            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                SetDragRegionForCustomTitleBar();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Check to see if customization is supported.
            // Currently only supported on Windows 11.
            if (AppWindowTitleBar.IsCustomizationSupported())
            {
                SetDragRegionForCustomTitleBar();
            }
        }

        private void SetDragRegionForCustomTitleBar()
        {
            // Check to see if customization is supported.
            // Currently only supported on Windows 11.
            if (AppWindowTitleBar.IsCustomizationSupported() && appWindow.TitleBar.ExtendsContentIntoTitleBar)
            {
                double marginLeftMenu = this.Margin.Left;
                double scaleAdjustment = WindowHelpers.GetScaleAdjustment(window);
                double rightPadding = appWindow.TitleBar.RightInset / scaleAdjustment;
                double leftPadding = appWindow.TitleBar.LeftInset / scaleAdjustment;
                int titleBarHeight = (int)(this.ActualHeight * scaleAdjustment);

                this.RightDragColumn.Width = new GridLength(rightPadding + 4);

                List<Windows.Graphics.RectInt32> dragRectsList = new();

                //Première zone draggable après l'icone du menu de navigtion à avant la barre de recherche
                Windows.Graphics.RectInt32 dragRectL;
                dragRectL.X = (int)(this.Margin.Left * scaleAdjustment);
                dragRectL.Y = 0;
                dragRectL.Height = titleBarHeight;
                /*Largeur de la zone draggable : après l'icone du menu de navigtion à avant la barre de recherche
                 addition de largeur du titre et de l'icone + la colonne draggable*/

                if (!_IsMiddleContentDraggable)
                    dragRectL.Width = (int)((this.LeftContentColumn.ActualWidth + this.LeftDragColumn.ActualWidth) * scaleAdjustment);
                else
                    dragRectL.Width = (int)((this.LeftContentColumn.ActualWidth + this.LeftDragColumn.ActualWidth + this.MiddleContentColumn.ActualWidth) * scaleAdjustment);
                dragRectsList.Add(dragRectL);

                //Deuxième zone draggable Après la barre de recherche aux button - 0 x
                Windows.Graphics.RectInt32 dragRectR;
                dragRectR.X = (int)((marginLeftMenu + this.LeftContentColumn.ActualWidth
                                    + this.LeftDragColumn.ActualWidth
                                    + this.MiddleContentColumn.ActualWidth) * scaleAdjustment);
                dragRectR.Y = 0;
                dragRectR.Height = titleBarHeight;
                dragRectR.Width = (int)(this.MiddleDragColumn.ActualWidth * scaleAdjustment);
                dragRectsList.Add(dragRectR);

                Windows.Graphics.RectInt32[] dragRects = dragRectsList.ToArray();

                appWindow.TitleBar.SetDragRectangles(dragRects);

            }

        }

    }
}
