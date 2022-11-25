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
        public void InitializeAppTitleBar();
        public void SetDragRegionForCustomTitleBar(AppWindow appWindow);
    }
}
