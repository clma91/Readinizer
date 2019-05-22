using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.Business.Services;
using Readinizer.Backend.DataAccess;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.DataAccess.Repositories;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.ViewModels;
using Readinizer.Frontend.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using MvvmDialogs;
using Readinizer.Backend.Business.Factory;
using Readinizer.Backend.DataAccess.UnityOfWork;
using Unity;

namespace Readinizer.Frontend
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            IUnityContainer container = new UnityContainer();

            container.RegisterType<IApplicationViewModel, ApplicationViewModel>();
            container.RegisterType<ApplicationView>();
            container.RegisterType<IStartUpViewModel, StartUpViewModel>();
            container.RegisterType<ITreeStructureResultViewModel, TreeStructureResultViewModel>();
            container.RegisterType<ISpinnerViewModel, SpinnerViewModel>();
            container.RegisterType<IDomainResultViewModel, DomainResultViewModel>();
            container.RegisterType<IRSoPResultViewModel, RSoPResultViewModel>();
            container.RegisterType<IOUResultViewModel, OUResultViewModel>();

            container.RegisterType<IADDomainService, ADDomainService>();
            container.RegisterType<ISiteService, SiteService>();
            container.RegisterType<IOrganisationalUnitService, OrganisationalUnitService>();
            container.RegisterType<IComputerService, ComputerService>();
            container.RegisterType<IRSoPService, RSoPService>();
            container.RegisterType<ISysmonService, SysmonService>();
            container.RegisterType<IPingService, PingService>();
            container.RegisterType<IAnalysisService, AnalysisService>();
            container.RegisterType<IRSoPPotService, RSoPPotService>();
            container.RegisterType<ISysmonResultViewModel, SysmonResultViewModel>();
            container.RegisterType<IExportService, ExportService>();

            container.RegisterType<ITreeNodesFactory, TreeNodesFactory>();

            container.RegisterSingleton<IReadinizerDbContext, ReadinizerDbContext>();
            container.RegisterSingleton<IUnitOfWork, UnitOfWork>();
            container.RegisterSingleton<IDialogService, DialogService>();

            container.RegisterSingleton<ISnackbarMessageQueue, SnackbarMessageQueue>();

            var ctx = new DbContext(ConfigurationManager.ConnectionStrings["ReadinizerDbContext"].ConnectionString);
            ctx.Database.CreateIfNotExists();

            var applicationView = container.Resolve<ApplicationView>();
            applicationView.Show();
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string friendlyMsg = string.Format("Sorry something went wrong.  The error was: [{0}]", e.Exception.Message);
            string caption = "Error";
            MessageBox.Show(friendlyMsg, caption, MessageBoxButton.OK, MessageBoxImage.Error);

            // Signal that we handled things--prevents Application from exiting
            e.Handled = true;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);


        }
    }
}
