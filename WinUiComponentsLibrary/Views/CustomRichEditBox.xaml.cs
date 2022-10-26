using Microsoft.UI.Text;
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
    public sealed partial class CustomRichEditBox : RichEditBox
    {
        public delegate void TextChangedEventHandler(CustomRichEditBox sender, TextGetOptions textGetOptions, string text);
        public event TextChangedEventHandler TextChangedRequested;
        private TextGetOptions TextGetOptions { get; set; } = TextGetOptions.None;
        private TextSetOptions TextSetOptions { get; set; } = TextSetOptions.None;
        public CustomRichEditBox()
        {
            this.InitializeComponent();

            this.Loaded += CustomRichEditBox_Loaded;
            this.Unloaded += CustomRichEditBox_Unloaded;
        }

        private void CustomRichEditBox_Unloaded(object sender, RoutedEventArgs e)
        {
            this.TextChanged -= CustomRichEditBox_TextChanged;
        }

        private void CustomRichEditBox_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsReadOnly)
            {
                this.IsReadOnly = false;
                this.Document.SetText(this.TextSetOptions, Text);
                this.IsReadOnly = true;
            }
            else
            {
                this.Document.SetText(this.TextSetOptions, Text);
            }
            this.TextChanged += CustomRichEditBox_TextChanged;
        }

        private void CustomRichEditBox_TextChanged(object sender, RoutedEventArgs e)
        {
            this.Document.GetText(this.TextGetOptions, out string text);
            SetValue(TextProperty, text);
            TextChangedRequested?.Invoke(this, this.TextGetOptions, text);
        }

        #region GetFormat
        internal TextGetOptions GetFormat
        {
            get => (TextGetOptions)GetValue(GetFormatProperty);
            set => SetValue(GetFormatProperty, value);
        }

        internal static readonly DependencyProperty GetFormatProperty = DependencyProperty.Register(nameof(GetFormat), typeof(TextGetOptions),
                                                                typeof(CustomRichEditBox), new PropertyMetadata(null, new PropertyChangedCallback(OnGetFormatChanged)));
        private static void OnGetFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomRichEditBox richEditBox && e.NewValue is TextGetOptions value)
                richEditBox.TextGetOptions = value;
        }
        #endregion

        #region SetFormat
        internal TextSetOptions SetFormat
        {
            get => (TextSetOptions)GetValue(SetFormatProperty);
            set => SetValue(SetFormatProperty, value);
        }

        internal static readonly DependencyProperty SetFormatProperty = DependencyProperty.Register(nameof(SetFormat), typeof(TextSetOptions),
                                                                typeof(CustomRichEditBox), new PropertyMetadata(null, new PropertyChangedCallback(OnSetFormatChanged)));
        private static void OnSetFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomRichEditBox richEditBox && e.NewValue is TextSetOptions value)
                richEditBox.TextSetOptions = value;
        }
        #endregion

        #region Text
        internal string Text
        {
            //get
            //{
            //    this.Document.GetText(this.TextGetOptions, out string text);
            //    return text;
            //}
            get => (string)GetValue(TextProperty);
            set
            {
                if (GetValue(TextProperty)?.ToString() != value)
                {
                    SetValue(TextProperty, value);
                }
            }
        }

        internal static readonly DependencyProperty TextProperty = DependencyProperty.Register(nameof(Text), typeof(string),
                                                                typeof(CustomRichEditBox), new PropertyMetadata(null, null));

        //private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    if (d is CustomRichEditBox richEditBox)
        //    {
        //        if (e.NewValue is string value)
        //        {
        //            if (richEditBox.IsReadOnly)
        //            {
        //                richEditBox.IsReadOnly = false;
        //                richEditBox.Document.SetText(richEditBox.TextSetOptions, value);
        //                richEditBox.IsReadOnly = true;
        //            }
        //            else
        //            {
        //                richEditBox.TextChanged -= richEditBox.CustomRichEditBox_TextChanged;
        //                richEditBox.Document.SetText(richEditBox.TextSetOptions, value);
        //                richEditBox.TextChanged += richEditBox.CustomRichEditBox_TextChanged;
        //            }
        //        }
        //    }
        //}
        #endregion
    }
}
