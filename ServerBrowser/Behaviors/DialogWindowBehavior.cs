using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using Microsoft.Xaml.Behaviors;

namespace ServerBrowser.Behaviors
{
    public class DialogWindowBehavior : Behavior<Window>
    {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached("DialogResult", typeof(bool?), typeof(DialogWindowBehavior), new PropertyMetadata(OnDialogResultChanged));

        public static bool? GetDialogResult(DependencyObject d)
        {
            return (bool?)d.GetValue(DialogResultProperty);
        }

        public static void SetDialogResult(DependencyObject d, bool? value)
        {
            d.SetValue(DialogResultProperty, value);
        }

        private static void OnDialogResultChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var window = d as Window;
            if (window != null)
            {
                window.DialogResult = args.NewValue as bool?;
            }
        }
    }
}