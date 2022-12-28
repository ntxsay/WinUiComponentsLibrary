﻿using AppHelpersStd20;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinUiComponentsLibrary.Code.Helpers
{
    public class VisualHelpers
    {
        public static T FindVisualChild<T>(DependencyObject elementCible) where T : DependencyObject
        {
            try
            {
                var count = VisualTreeHelper.GetChildrenCount(elementCible);
                if (count == 0) return null;

                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(elementCible, i);
                    if (child != null && child is T t)
                    {
                        return t;
                    }
                    else
                    {
                        T childOfChild = FindVisualChild<T>(child);
                        if (childOfChild != null)
                            return childOfChild;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Logs.Log(className: nameof(VisualHelpers), exception: ex);
                return null;
            }
        }

        public static T FindVisualChild<T>(DependencyObject elementCible, string childName) where T : DependencyObject
        {
            try
            {
                //int count = VisualTreeHelper.GetChildrenCount(elementCible);
                //if (count == 0) return null;

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(elementCible); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(elementCible, i);
                    if (child != null && child is T t)
                    {
                        // If the child's name is set for search
                        if (child is FrameworkElement frameworkElement && frameworkElement.Name == childName)
                        {
                            return t;
                        }
                        else
                        {
                            T childOfChild = FindVisualChild<T>(child, childName);
                            if (childOfChild != null)
                                return childOfChild;
                        }
                    }
                    else
                    {
                        T childOfChild = FindVisualChild<T>(child, childName);
                        if (childOfChild != null)
                            return childOfChild;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Logs.Log(className: nameof(VisualHelpers), exception: ex);
                return null;
            }
        }

        public static IEnumerable<T> FindVisualChilds<T>(DependencyObject elementCible) where T : DependencyObject
        {
            try
            {
                var count = VisualTreeHelper.GetChildrenCount(elementCible);
                if (count == 0) return Enumerable.Empty<T>();

                List<T> result = new List<T>();
                for (int i = 0; i < count; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(elementCible, i);
                    if (child != null && child is T t)
                    {
                        result.Add(t);
                    }
                    else
                    {
                        var elList = FindVisualChilds<T>(child);
                        if (elList != null && elList.Any())
                        {
                            result.AddRange(elList);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                Logs.Log(className: nameof(VisualHelpers), exception: ex);
                return Enumerable.Empty<T>();
            }
        }

        public static T FindVisualAncestor<T>(DependencyObject elementCible) where T : DependencyObject
        {
            try
            {
                DependencyObject parent = elementCible;
                while (parent != null)
                {
                    parent = VisualTreeHelper.GetParent(parent);
                    if (parent == null)
                    {
                        return null;
                    }
                    else
                    {
                        if (parent is T t)
                        {
                            return t;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Logs.Log(className: nameof(VisualHelpers), exception: ex);
                return null;
            }
        }
    }
}
