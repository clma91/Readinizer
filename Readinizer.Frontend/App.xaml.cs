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

            container.RegisterType<IADDomainRepository, ADDomainRepository>();
            container.RegisterType<IADDomainService, ADDomainService>();

            container.RegisterType<IADOrganisationalUnitRepository, ADOrganisationalUnitRepository>();
            container.RegisterType<IADOrganisationalUnitService, ADOrganisationalUnitService>();

            container.RegisterType<IADOuMemberRepository, ADOuMemberRepository>();
            container.RegisterType<IADOuMemberService, ADOuMemberService>();

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ReadinizerDbContext>());

            var applicationView = container.Resolve<ApplicationView>();
            applicationView.Show();
        }
    }
}
