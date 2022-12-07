using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiComponentsLibrary.Code.Helpers
{
    public interface IBackdropHelpers
    {
        /// <summary>
        /// Tente d'appliquer le thème Mica
        /// </summary>
        /// <returns>True si appliqué</returns>
        bool TrySetMicaBackdrop();

        /// <summary>
        /// Tente d'appliquer le thème Acrylique
        /// </summary>
        /// <returns>True si appliqué</returns>
        bool TrySetAcrylicBackdrop();
        void Window_Activated(object sender, WindowActivatedEventArgs args);
        void Window_Closed(object sender, WindowEventArgs args);
        void Window_ThemeChanged(FrameworkElement sender, object args);
        void SetConfigurationSourceTheme();

        /// <summary>
        /// Définit le type de thème à appliquer pour la fenêtre
        /// </summary>
        /// <param name="type">Type de thème</param>
        void SetBackdrop(BackdropType type);
        
        /// <summary>
        /// Initialise la personnalisation du thème au démarrage de la fenêtre
        /// </summary>
        void InitializeBackground();
    }
}
