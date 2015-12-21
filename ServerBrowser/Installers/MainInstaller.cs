using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ServerBrowser.Services;
using ServerBrowser.Services.Configuration;
using ServerBrowser.Services.Dialog;
using ServerBrowser.Services.Server;
using ServerBrowser.ViewModels;
using ServerBrowser.Views;

namespace ServerBrowser.Installers
{
    public class MainInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<MainViewModel>().LifestyleSingleton());
            container.Register(Component.For<MainView>().LifestyleSingleton());
            container.Register(Component.For<OptionsViewModel>().LifestyleTransient());
            container.Register(Component.For<OptionsView>().LifestyleTransient());
            container.Register(Component.For<IConfigurationService>().ImplementedBy<ConfigurationService>().LifestyleSingleton());
            container.Register(Component.For<IDialogService>().ImplementedBy<DialogService>().LifestyleSingleton());
            container.Register(Component.For<IServerService>().ImplementedBy<ServerService>().LifestyleSingleton());
        }
    }
}
