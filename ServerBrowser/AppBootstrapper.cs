using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using Caliburn.Micro;
using Castle.Core.Internal;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor.Installer;
using ServerBrowser.Services;
using ServerBrowser.Services.Configuration;
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
            LoadConfiguration();
            DisplayRootViewFor<MainViewModel>();
        }

        private void LoadConfiguration()
        {
            var configurationService = _container.Resolve<IConfigurationService>();
            configurationService.Load();
            var culture = configurationService.Settings.Culture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(culture);
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));
        }

        protected override void Configure()
        {
            _container = new WindsorContainer();
            _container.Register(Component.For<IEventAggregator>().ImplementedBy<EventAggregator>());
            _container.Register(Component.For<IWindowManager>().ImplementedBy<WindowManager>());

            _container.Install(FromAssembly.This());
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
                    .Where(property => _container.Kernel.HasComponent(property.PropertyType))
                    .ForEach(property => property.SetValue(instance, _container.Resolve(property.PropertyType), null));
        }
    }
}
