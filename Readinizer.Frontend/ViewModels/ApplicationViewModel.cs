using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;

namespace Readinizer.Frontend.ViewModels
{
    public class ApplicationViewModel : ViewModelBase, IApplicationViewModel
    {
        private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { Set(ref currentViewModel, value); }
        }

        private readonly StartUpViewModel startUpViewModel;
        private readonly TreeStructureResultViewModel treeStructureResultViewModel;

        [Obsolete("Only for desing data", true)]
        public ApplicationViewModel()
        {
            if (!IsInDesignMode)
            {
                throw new Exception("Use only for design data");
            }
        }

        public ApplicationViewModel(StartUpViewModel startUpViewModel, TreeStructureResultViewModel treeStructureResultViewModel)
        {
            this.startUpViewModel = startUpViewModel;
            this.treeStructureResultViewModel = treeStructureResultViewModel;

            ShowTreeStructureResultView();
            Messenger.Default.Register<ChangeView>(this, ChangeView);
        }

        private void ShowStartUpView()
        {
            currentViewModel = startUpViewModel;
        }

        private void ShowTreeStructureResultView()
        {
            currentViewModel = treeStructureResultViewModel;
        }

        private void ChangeView(ChangeView message)
        {
            if (message.ViewModelType == typeof(StartUpViewModel))
            {
                ShowStartUpView();
            }
            else if (message.ViewModelType == typeof(TreeStructureResultViewModel))
            {
                ShowTreeStructureResultView();
            }
        }
    }
}
