using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace WinUiComponentsLibrary.ViewModels
{
    public struct ColorName
    {
        public string Name { get; set; }
        public Color Color { get; set; }
        public Brush Brush { get; set; }

        public ColorName(string name, Color color)
        {
            Name = name;
            Color = color;
            Brush = new SolidColorBrush(Color);
        }
    }
}
