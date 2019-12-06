using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace ServerBrowser.Triggers
{
	/// <summary>
	/// Prevzato z http://stackoverflow.com/questions/4181310/how-can-i-bind-key-gestures-in-caliburn-micro a upraveno
	/// </summary>
	public class InputBindingTrigger : TriggerBase<FrameworkElement>, ICommand
	{
		private InputBindingCollection _windowBindings;

		public static readonly DependencyProperty InputBindingProperty = DependencyProperty.Register(
			"InputBinding",
			typeof(InputBinding),
			typeof(InputBindingTrigger),
			new UIPropertyMetadata(null));

		public InputBinding InputBinding
		{
			get { return (InputBinding)GetValue(InputBindingProperty); }
			set { SetValue(InputBindingProperty, value); }
		}

		public event EventHandler CanExecuteChanged = delegate { };

		public bool CanExecute(object parameter)
		{
			return true;
		}

		public void Execute(object parameter)
		{
			InvokeActions(parameter);
		}

		protected override void OnAttached()
		{
			if (InputBinding != null)
			{
				InputBinding.Command = this;
				AssociatedObject.Loaded += AssociatedObjectOnLoaded;
				AssociatedObject.Unloaded += AssociatedObjectOnUnloaded;
			}
			base.OnAttached();
		}

		private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			_windowBindings = GetWindow(AssociatedObject).InputBindings;
			if (!_windowBindings.Contains(InputBinding))
			{
				_windowBindings.Add(InputBinding);
			}
		}

		private void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
		{
			if (_windowBindings != null)
			{
				_windowBindings.Remove(InputBinding);
			}
		}

		private Window GetWindow(FrameworkElement frameworkElement)
		{
			if (frameworkElement is Window)
			{
				return frameworkElement as Window;
			}

			var parent = frameworkElement.Parent as FrameworkElement;

			return GetWindow(parent);
		}
	}
}