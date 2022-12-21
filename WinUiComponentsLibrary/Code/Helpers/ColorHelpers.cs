using System.Globalization;
using Windows.UI;

namespace WinUiComponentsLibrary.Code.Helpers
{
    public class ColorHelpers
    {
        public static Color HexToColor(in string hexColor,bool useAlpha = false)
        {
            string _hexColor = hexColor;
            //Remove # if present
            if (_hexColor.IndexOf('#') != -1)
            {
                _hexColor = _hexColor.Replace("#", "");
            }

            if (_hexColor.Length == 6)
            {
                _hexColor = "FF" + _hexColor;
            }

            //100 % — FF  //50 % — 80
            //95 % — F2  //45 % — 73
            //90 % — E6  //40 % — 66
            //85 % — D9  //30 % — 4D
            //80 % — CC     //25 % — 40
            //75 % — BF  //20 % — 33
            //70 % — B3  //15 % — 26
            //65 % — A6  //10 % — 1A
            //60 % — 99  //5 % — 0D
            //55 % — 8C  //0 % — 00

            byte alpha = 0;
            byte red = 0;
            byte green = 0;
            byte blue = 0;

            if (_hexColor.Length == 8)
            {
                //#AARRGGBB
                alpha = byte.Parse(_hexColor.Substring(0, 2), NumberStyles.AllowHexSpecifier);
                red = byte.Parse(_hexColor.Substring(2, 2), NumberStyles.AllowHexSpecifier);
                green = byte.Parse(_hexColor.Substring(4, 2), NumberStyles.AllowHexSpecifier);
                blue = byte.Parse(_hexColor.Substring(6, 2), NumberStyles.AllowHexSpecifier);
            }
            return Color.FromArgb(useAlpha ? alpha : (byte)255, red, green, blue);
        }
    }
}
