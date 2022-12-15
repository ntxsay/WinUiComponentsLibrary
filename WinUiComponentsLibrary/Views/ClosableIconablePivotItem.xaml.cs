// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using AppHelpersStd20.Extensions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiComponentsLibrary.Views
{
    public sealed partial class ClosableIconablePivotItem : PivotItem
    {
        public delegate void CloseItemEventHandler(ClosableIconablePivotItem sender, ExecuteRequestedEventArgs e);
        public event CloseItemEventHandler CloseItemRequested;

        public ClosableIconablePivotItem()
        {
            this.InitializeComponent();
        }

        public Guid Guid { get; private set; }

        public Guid HeaderGuid
        {
            get { return (Guid)GetValue(HeaderGuidProperty); }
            set { SetValue(HeaderGuidProperty, value); }
        }

        public static readonly DependencyProperty HeaderGuidProperty = DependencyProperty.Register(nameof(HeaderGuid), typeof(Guid),
                                                                typeof(ClosableIconablePivotItem), new PropertyMetadata(null, new PropertyChangedCallback(OnHeaderGuidChanged)));

        private static void OnHeaderGuidChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClosableIconablePivotItem parent && e.NewValue is Guid guid)
            {
                parent.Guid = guid;
            }
        }

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(String),
                                                                typeof(ClosableIconablePivotItem), new PropertyMetadata(null, new PropertyChangedCallback(OnTitleChanged)));

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClosableIconablePivotItem parent && e.NewValue is string title)
            {
                if (!title.IsStringNullOrEmptyOrWhiteSpace())
                {
                    parent.TbcTitle.Text = title.Trim();
                }
            }
        }

        public string Glyph
        {
            get { return (string)GetValue(GlyphProperty); }
            set { SetValue(GlyphProperty, value); }
        }

        public static readonly DependencyProperty GlyphProperty = DependencyProperty.Register(nameof(Glyph), typeof(String),
                                                                typeof(ClosableIconablePivotItem), new PropertyMetadata(null, new PropertyChangedCallback(OnGlyphChanged)));
        private static void OnGlyphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ClosableIconablePivotItem parent && e.NewValue is string glyph)
            {
                parent.MyFontIcon.Glyph = glyph;
            }
        }

        private void CloseItemXUiCmd_ExecuteRequested(XamlUICommand sender, ExecuteRequestedEventArgs args)
        {
            CloseItemRequested?.Invoke(this, args);
        }
    }
}
