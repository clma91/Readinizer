using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;
using SnackbarMessage = Readinizer.Frontend.Messages.SnackbarMessage;

namespace Readinizer.Frontend.ViewModels
{
    public class ApplicationViewModel : ViewModelBase, IApplicationViewModel
    {
        private ICommand closeCommand;
        public ICommand CloseCommand => closeCommand ?? (closeCommand = new RelayCommand(() => this.OnClose(), () => true));
        private ICommand githubCommand;
        public ICommand GithubCommand => githubCommand ?? (githubCommand = new RelayCommand(() => this.OnGithub(), () => true));

        private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return currentViewModel; }
            set { Set(ref currentViewModel, value); }
        }

        public ISnackbarMessageQueue SnackbarMessageQueue { get; }

        private readonly StartUpViewModel startUpViewModel;
        private readonly TreeStructureResultViewModel treeStructureResultViewModel;
        private readonly SpinnerViewModel spinnerViewModel;

        [Obsolete("Only for desing data", true)]
        public ApplicationViewModel() : this(new StartUpViewModel(), null, null, null)
        {
            if (!IsInDesignMode)
            {
                throw new Exception("Use only for design data");
            }
        }

        public ApplicationViewModel(StartUpViewModel startUpViewModel, TreeStructureResultViewModel treeStructureResultViewModel, ISnackbarMessageQueue snackbarMessageQueue,
                                    SpinnerViewModel spinnerViewModel)

        {
            this.startUpViewModel = startUpViewModel;
            this.treeStructureResultViewModel = treeStructureResultViewModel;
            this.spinnerViewModel = spinnerViewModel;

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
            treeStructureResultViewModel.BuildTree();
        }

        private void ShowSpinnerView()
        {
            CurrentViewModel = spinnerViewModel;
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
            } else if (message.ViewModelType == typeof(SpinnerViewModel))
            {
                ShowSpinnerView();
            }
        }

        private void OnShowMessage(SnackbarMessage message)
        {
            SnackbarMessageQueue.Enqueue(message.Message);
        }

        private void OnClose()
        {
            Application.Current.Shutdown();
        }

        private void OnGithub()
        {
            Process.Start("https://github.com/clma91/Readinizer/wiki");
        }
    }
}
