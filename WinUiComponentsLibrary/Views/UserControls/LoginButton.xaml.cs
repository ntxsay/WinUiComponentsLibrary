// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinUiComponentsLibrary.Code.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiComponentsLibrary.Views.UserControls
{
    public sealed partial class LoginButton : Button
    {
        public LoginButton()
        {
            this.InitializeComponent();
        }

        private async void Image_Loaded(object sender, RoutedEventArgs e)
        {
            if (sender is Image image)
            {
                if (image.Tag?.ToString() == "g-drive-login")
                {
                    ImageSource imageSource = await InputOutputHelpers.ImageFromFileAsync("ms-appx:///Assets/Medias/Icons/Google_Drive_icon_(2020).svg");
                    image.Source = imageSource;
                }
            }
        }

        private void Button_Loaded(object sender, RoutedEventArgs e)
        {
            ResizePictureBoxInParent();
        }

        private void Button_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ResizePictureBoxInParent();
        }

        private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            ResizePictureBoxInParent();
        }

        public void ResizePictureBoxInParent()
        {
            if (this.ActualWidth == 0 || this.ActualHeight == 0)
                return;
            this.Picture.Height = this.ActualHeight - (4 + this.Padding.Top + this.Padding.Bottom);
            this.Picture.Width = this.ActualWidth - (4 + this.Padding.Left + this.Padding.Right);
        }

       
    }
}
