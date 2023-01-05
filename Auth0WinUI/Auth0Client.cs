using Auth0.OidcClient;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth0WinUI
{
    /// <summary>
    /// Classe primaire pour effectuer des opérations d'authentification et d'autorisation avec Auth0
    ///  en utilisant le <see cref="IdentityModel.OidcClient.OidcClient"/> sous-jacent .
    /// </summary>
    public class Auth0Client : Auth0ClientBase
    {
        /// <summary>
        /// Crée une nouvelle instance du client Auth0 OIDC.
        /// </summary>
        /// <param name="options">Le <see cref="Auth0ClientOptions"/> spécifiant la configuration du client Auth0 OIDC</param>
        public Auth0Client(XamlRoot xamlRoot, Auth0ClientOptions options)
            : base(options, "winui")
        {
            options.Browser ??= new WebBrowserContentDialog(xamlRoot);
        }
    }
}
