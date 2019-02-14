using System;
using System.Windows;
using System.Windows.Controls;

namespace FontLinkSettings
{
    internal class FallbackTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(Object item, DependencyObject container)
        {
            if (container is FrameworkElement root &&
                item is FontLinkFallback fallback)
            {
                if (fallback.GDISize == 0 && fallback.DirectXSize == 0)
                {
                    return root.FindResource("Fallback") as DataTemplate;
                }

                return root.FindResource("FallbackWithSizes") as DataTemplate;
            }

            return null;
        }
    }
}
