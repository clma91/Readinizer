using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;
using SnackbarMessage = Readinizer.Frontend.Messages.SnackbarMessage;

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

        public ISnackbarMessageQueue SnackbarMessageQueue { get; }

        private readonly StartUpViewModel startUpViewModel;
        private readonly TreeStructureResultViewModel treeStructureResultViewModel;

        [Obsolete("Only for desing data", true)]
        public ApplicationViewModel() : this(new StartUpViewModel(), null, null)
        {
            if (!IsInDesignMode)
            {
                throw new Exception("Use only for design data");
            }
        }

        public ApplicationViewModel(StartUpViewModel startUpViewModel, TreeStructureResultViewModel treeStructureResultViewModel, ISnackbarMessageQueue snackbarMessageQueue)
        {
            this.startUpViewModel = startUpViewModel;
            this.treeStructureResultViewModel = treeStructureResultViewModel;

            this.SnackbarMessageQueue = snackbarMessageQueue;

            ShowStartUpView();
            Messenger.Default.Register<ChangeView>(this, ChangeView);
            Messenger.Default.Register<SnackbarMessage>(this, OnShowMessage);
        }

        private void ShowStartUpView()
        {
            CurrentViewModel = startUpViewModel;
        }

        private void ShowTreeStructureResultView()
        {
            CurrentViewModel = treeStructureResultViewModel;
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

        private void OnShowMessage(SnackbarMessage message)
        {
            SnackbarMessageQueue.Enqueue(message.Message);
        }
    }
}
