using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Auth0WinUI
{
    internal class WebHelpers
    {
        internal static string GetDecodedHtmlStringCodeSource(string htmlString)
        {
            if (string.IsNullOrEmpty(htmlString) || string.IsNullOrWhiteSpace(htmlString))
            {
                Console.WriteLine("Le code source HTML doit être renseignée");
                return null;
            }

            string sHtmlDecoded = System.Text.RegularExpressions.Regex.Unescape(htmlString);
            string sUrlDecoded = HttpUtility.HtmlDecode(sHtmlDecoded);

            if (sUrlDecoded.StartsWith("\""))
            {
                sUrlDecoded = sUrlDecoded.Remove(0, 1);
            }

            if (sUrlDecoded.EndsWith("\""))
            {
                sUrlDecoded = sUrlDecoded.Remove(sUrlDecoded.Length - 1, 1);
            }

            return sUrlDecoded;
        }

    }
}
