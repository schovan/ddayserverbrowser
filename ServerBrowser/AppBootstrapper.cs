using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using Caliburn.Micro;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.MicroKernel.Registration;
using ServerBrowser.ViewModels;

namespace ServerBrowser
{
	public class AppBootstrapper : BootstrapperBase
	{
		private WindsorContainer _container;

		public AppBootstrapper()
		{
			Initialize();
		}

		protected override void OnStartup(object sender, StartupEventArgs e)
		{
			InitializeCulture();
			DisplayRootViewFor<MainViewModel>();
		}

		protected override void Configure()
		{
			_container = new WindsorContainer();

			_container.AddFacility<TypedFactoryFacility>();

			_container.Register(Component.For<IEventAggregator>().ImplementedBy<EventAggregator>());
			_container.Register(Component.For<IWindowManager>().ImplementedBy<WindowManager>());

			_container.Install(FromAssembly.Containing<AppBootstrapper>());
        }

		protected override object GetInstance(Type service, string key)
		{
			return string.IsNullOrWhiteSpace(key) ? _container.Resolve(service) : _container.Resolve(key, service);
		}

		protected override IEnumerable<object> GetAllInstances(Type service)
		{
			return _container.ResolveAll(service).Cast<object>();
		}

		protected override void BuildUp(object instance)
		{
			instance.GetType().GetProperties()
			        .Where(property => property.CanWrite && property.PropertyType.IsPublic)
			        .Where(property => _container.Kernel.HasComponent(property.PropertyType)).ToList()
			        .ForEach(property => property.SetValue(instance, _container.Resolve(property.PropertyType), null));
		}

		private void InitializeCulture()
		{
			if (ConfigurationManager.AppSettings.AllKeys.Contains("Culture"))
			{
				var culture = ConfigurationManager.AppSettings["Culture"];
				Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
				CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
				CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);
				var lang = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);
				FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(lang));
				FrameworkContentElement.LanguageProperty.OverrideMetadata(typeof(TextElement), new FrameworkPropertyMetadata(lang));
			}
		}
	}
}
