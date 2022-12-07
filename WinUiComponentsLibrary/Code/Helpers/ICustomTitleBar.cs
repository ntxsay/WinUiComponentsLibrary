using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiComponentsLibrary.Code.Helpers
{
    public interface ICustomTitleBar
    {
        /// <summary>
        /// Initialise la barre de titre personnalisée
        /// </summary>
        public void InitializeAppTitleBar();

        /// <summary>
        /// Définit la zone draggable de la barre de titre
        /// </summary>
        /// <param name="appWindow">Fenêtre actuelle</param>
        public void SetDragRegionForCustomTitleBar(AppWindow appWindow);
    }
}
