using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MaterialDesignThemes.Wpf;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.Domain.Models;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;
using SnackbarMessage = Readinizer.Frontend.Messages.SnackbarMessage;

namespace Readinizer.Frontend.ViewModels
{
    public class ApplicationViewModel : ViewModelBase, IApplicationViewModel
    {
        private ICommand closeCommand;
        public ICommand CloseCommand => closeCommand ?? (closeCommand = new RelayCommand(() => OnClose()));
        private ICommand githubCommand;
        public ICommand GithubCommand => githubCommand ?? (githubCommand = new RelayCommand(() => OnGithub()));
        private ICommand exportRSoPPotsCommand;
        public ICommand ExportRSoPPotsCommand => exportRSoPPotsCommand ?? (exportRSoPPotsCommand = new RelayCommand(() => Export(typeof(RsopPot))));
        private ICommand exportRSoPsCommand;
        public ICommand ExportRSoPsCommand => exportRSoPsCommand ?? (exportRSoPsCommand = new RelayCommand(() => Export(typeof(Rsop))));

        private ViewModelBase currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get => currentViewModel;
            set => Set(ref currentViewModel, value);
        }

        private bool canExport;
        public bool CanExport
        {
            get => canExport;
            set => Set(ref canExport, value);
        }

        public ISnackbarMessageQueue SnackbarMessageQueue { get; }

        private readonly StartUpViewModel startUpViewModel;
        private readonly TreeStructureResultViewModel treeStructureResultViewModel;
        private readonly SpinnerViewModel spinnerViewModel;
        private readonly DomainResultViewModel domainResultViewModel;
        private readonly RSoPResultViewModel rsopResultViewModel;
        private readonly OUResultViewModel ouResultViewModel;
        private readonly SysmonResultViewModel sysmonResultViewModel;
        private readonly IDialogService dialogService;
        private readonly IExportService exportService;

        [Obsolete("Only for desing data", true)]
        public ApplicationViewModel() : this(new StartUpViewModel(), null, null, null, null, null, null, null, null, null)
        {
            if (!IsInDesignMode)
            {
                throw new Exception("Use only for design data");
            }
        }

        public ApplicationViewModel(StartUpViewModel startUpViewModel, TreeStructureResultViewModel treeStructureResultViewModel, 
                                    ISnackbarMessageQueue snackbarMessageQueue, SpinnerViewModel spinnerViewModel, 
                                    DomainResultViewModel domainResultViewModel, RSoPResultViewModel rsopResultViewModel, 
                                    OUResultViewModel ouResultViewModel, SysmonResultViewModel sysmonResultViewModel,
                                    IDialogService dialogService, IExportService exportService)
        {
            this.startUpViewModel = startUpViewModel;
            this.treeStructureResultViewModel = treeStructureResultViewModel;
            this.spinnerViewModel = spinnerViewModel;
            this.domainResultViewModel = domainResultViewModel;
            this.rsopResultViewModel = rsopResultViewModel;
            this.ouResultViewModel = ouResultViewModel;
            this.sysmonResultViewModel = sysmonResultViewModel;
            this.dialogService = dialogService;
            this.exportService = exportService;

            this.SnackbarMessageQueue = snackbarMessageQueue;

            ShowStartUpView();
            Messenger.Default.Register<ChangeView>(this, ChangeView);
            Messenger.Default.Register<SnackbarMessage>(this, OnShowMessage);
            Messenger.Default.Register<EnableExport>(this, EnableExport);
        }

        private void ShowStartUpView()
        {
            CurrentViewModel = startUpViewModel;
        }

        private void ShowTreeStructureResultView(string visability)
        {
            CurrentViewModel = treeStructureResultViewModel;
            treeStructureResultViewModel.WithSysmon = visability;
            treeStructureResultViewModel.BuildTree();
            
        }

        private void ShowSpinnerView()
        {
            CurrentViewModel = spinnerViewModel;
        }

        private void ShowDomainResultView(int refId)
        {
            CurrentViewModel = domainResultViewModel;
            domainResultViewModel.RefId = refId;
            domainResultViewModel.loadRsopPots();
        }

        private void ShowRsopResultView(int refId)
        {
            CurrentViewModel = rsopResultViewModel;
            rsopResultViewModel.RefId = refId;
        }

        private void ShowOuResultView(int refId)
        {
            CurrentViewModel = ouResultViewModel;
            ouResultViewModel.RefId = refId;
        }

        private void ShowSysmonResultView()
        {
            CurrentViewModel = sysmonResultViewModel;
            sysmonResultViewModel.loadComputers();

        }

        private void ChangeView(ChangeView message)
        {
            if (message.ViewModelType == typeof(StartUpViewModel))
            {
                ShowStartUpView();
            }
            else if (message.ViewModelType == typeof(TreeStructureResultViewModel))
            {
                ShowTreeStructureResultView(message.Visability);
            }
            else if (message.ViewModelType == typeof(SpinnerViewModel))
            {
                ShowSpinnerView();
            }
            else if(message.ViewModelType == typeof(DomainResultViewModel))
            {
                ShowDomainResultView(message.RefId);
               
            }
            else if(message.ViewModelType == typeof(RSoPResultViewModel))
            {
                ShowRsopResultView(message.RefId);
            }
            else if (message.ViewModelType == typeof(OUResultViewModel))
            {
                ShowOuResultView(message.RefId);
            }
            else if (message.ViewModelType == typeof(SysmonResultViewModel))
            {
                ShowSysmonResultView();
            }
        }

        private void EnableExport(EnableExport message)
        {
            CanExport = message.ExportEnabled;
            RaisePropertyChanged(nameof(CanExport));
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

        private async void Export(Type type)
        {
            var settings = new SaveFileDialogSettings
            {
                Title = "Save all identical audit settings",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "JSON-file (*.json)|*.json|All Files (*.*)|*.*",
                CreatePrompt = false,
                CheckFileExists = false
            };

            bool? success = dialogService.ShowSaveFileDialog(this, settings);
            if (success == true)
            {
                var exportPath = settings.FileName;
                if (settings.CheckPathExists)
                {
                    var successfullyExported = await exportService.Export(type, exportPath);
                    if (!successfullyExported)
                    {
                        Messenger.Default.Send(new SnackbarMessage($"Something went wrong during saving the file"));
                    }
                }
                else
                {
                    Messenger.Default.Send(new SnackbarMessage($"The specified path '{exportPath}' does not exist"));
                }
            }
        }

    }
}
