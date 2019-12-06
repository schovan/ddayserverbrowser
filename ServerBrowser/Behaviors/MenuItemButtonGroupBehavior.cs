using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Xaml.Behaviors;

namespace ServerBrowser.Behaviors
{
    public class MenuItemButtonGroupBehavior : Behavior<MenuItem>
    {
        public static readonly DependencyProperty CheckedHeaderProperty = DependencyProperty.Register("CheckedHeader", typeof(string), typeof(MenuItemButtonGroupBehavior), new FrameworkPropertyMetadata(OnCheckedHeaderChanged)
        {
            BindsTwoWayByDefault = true,
            DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        });

        public string CheckedHeader
        {
            get { return (string)GetValue(CheckedHeaderProperty); }
            set { SetValue(CheckedHeaderProperty, value); }
        }

        private static void OnCheckedHeaderChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var menuItemButtonGroupBehavior = (MenuItemButtonGroupBehavior)d;
            menuItemButtonGroupBehavior.CheckHeader((string)args.NewValue);
        }

        private void CheckHeader(string checkedHeader)
        {
            var menuItem = GetCheckableSubMenuItems(AssociatedObject).SingleOrDefault(item =>
                item.Header is string &&
                (string) item.Header == checkedHeader
                && item.IsChecked == false);
            if (menuItem != null)
            {
                menuItem.IsChecked = true;
                GetCheckableSubMenuItems(AssociatedObject).Where(item => item != menuItem).ToList().ForEach(item => item.IsChecked = false);
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            GetCheckableSubMenuItems(AssociatedObject)
                .ToList()
                .ForEach(item => item.Click += OnClick);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            GetCheckableSubMenuItems(AssociatedObject)
                .ToList()
                .ForEach(item => item.Click -= OnClick);
        }

        private static IEnumerable<MenuItem> GetCheckableSubMenuItems(ItemsControl menuItem)
        {
            var itemCollection = menuItem.Items;
            return itemCollection.OfType<MenuItem>().Where(menuItemCandidate => menuItemCandidate.IsCheckable);
        }

        private void OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var menuItem = (MenuItem)sender;

            CheckedHeader =  menuItem.Header.ToString();

            if (!menuItem.IsChecked)
            {
                menuItem.IsChecked = true;
                return;
            }

            GetCheckableSubMenuItems(AssociatedObject)
                .Where(item => item != menuItem)
                .ToList()
                .ForEach(item => item.IsChecked = false);
        }
    }
}
