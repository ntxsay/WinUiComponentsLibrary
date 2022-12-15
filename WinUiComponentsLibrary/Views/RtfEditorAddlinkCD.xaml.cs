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
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinUiComponentsLibrary.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RtfEditorAddlinkCD : ContentDialog, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        private string _Link = "";
        public string Link
        {
            get => _Link;
            private set
            {
                if (_Link != value)
                {
                    _Link = value;
                    OnPropertyChanged();
                }
            }
        }

        public RtfEditorAddlinkCD()
        {
            this.InitializeComponent();
        }

        public RtfEditorAddlinkCD(string linkName, string linkAdress)
        {
            this.InitializeComponent();
            if (!linkName.IsStringNullOrEmptyOrWhiteSpace())
            {
                richEditName.Document.SetText(Microsoft.UI.Text.TextSetOptions.FormatRtf, linkName);
            }
            if (!linkAdress.IsStringNullOrEmptyOrWhiteSpace())
            {
                Link = linkAdress;
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
