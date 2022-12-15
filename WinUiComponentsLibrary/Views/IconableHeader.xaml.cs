// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using AppHelpersStd20.Extensions;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiComponentsLibrary.Views
{
    public sealed partial class IconableHeader : StackPanel
    {
        public Guid Guid { get; private set; }
        public IconableHeader()
        {
            this.InitializeComponent();
        }

        public Guid HeaderGuid
        {
            get { return (Guid)GetValue(HeaderGuidProperty); }
            set { SetValue(HeaderGuidProperty, value); }
        }

        public static readonly DependencyProperty HeaderGuidProperty = DependencyProperty.Register(nameof(HeaderGuid), typeof(Guid),
                                                                typeof(IconableHeader), new PropertyMetadata(null, new PropertyChangedCallback(OnHeaderGuidChanged)));

        private static void OnHeaderGuidChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IconableHeader parent && e.NewValue is Guid guid)
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
                                                                typeof(IconableHeader), new PropertyMetadata(null, new PropertyChangedCallback(OnTitleChanged)));

        private static void OnTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IconableHeader parent && e.NewValue is string title)
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
                                                                typeof(IconableHeader), new PropertyMetadata(null, new PropertyChangedCallback(OnGlyphChanged)));
        private static void OnGlyphChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IconableHeader parent && e.NewValue is string glyph)
            {
                parent.MyFontIcon.Glyph = glyph;
            }
        }
    }
}
