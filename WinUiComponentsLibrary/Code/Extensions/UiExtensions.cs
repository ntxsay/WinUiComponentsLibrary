using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;

namespace WinUiComponentsLibrary.Code.Extensions
{
    public static class UiExtensions
    {
        public static bool ScrollToElement(this ScrollViewer scrollViewer, FrameworkElement element, bool isVerticalScrolling = true, bool smoothScrolling = true, float? zoomFactor = null)
        {
            var transform = element.TransformToVisual((UIElement)scrollViewer.Content);
            var position = transform.TransformPoint(new Point(0, 0));

            if (isVerticalScrolling)
            {
                return scrollViewer.ChangeView(null, position.Y, zoomFactor, !smoothScrolling);
            }
            else
            {
                return scrollViewer.ChangeView(position.X, null, zoomFactor, !smoothScrolling);
            }
        }
    }
}
