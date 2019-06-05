using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Schema;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Backend.DataAccess.Interfaces;
using Readinizer.Backend.Domain.Models;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;

namespace Readinizer.Frontend.ViewModels
{
    public class SysmonResultViewModel : ViewModelBase, ISysmonResultViewModel
    {
        private readonly IUnitOfWork unitOfWork;


        private ICommand backCommand;
        public ICommand BackCommand => backCommand ?? (backCommand = new RelayCommand(() => this.Back()));

        [Obsolete("Only for design data", true)]
        public SysmonResultViewModel()
        {
            if (!this.IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public SysmonResultViewModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork; 
        }

        private List<string> sysmonActiveList { get; set; }

        private List<string> sysmonNotActiveList { get; set; }

        public List<Computer> Computers { get; set; }

        public void loadComputers()
        {
            Computers = unitOfWork.ComputerRepository.GetAllEntities().Result;
            fillLists();
        }

        private void fillLists()
        {
            List<string> bad = new List<string>();
            List<string> good = new List<string>();
            foreach (var computer in Computers)
            {
                if (computer.isSysmonRunning.Equals(true))
                {
                    good.Add(computer.ComputerName + "." + computer.OrganisationalUnits.FirstOrDefault().ADDomain.Name);
                    
                }
                else if (computer.isSysmonRunning.Equals(false))
                {
                    bad.Add(computer.ComputerName + "." + computer.OrganisationalUnits.FirstOrDefault().ADDomain.Name);
                }
            }

            sysmonActiveList = good;
            sysmonNotActiveList = bad;
        }

        private List<KeyValuePair<string, int>> loadPieChartData()
        {
            int runningCounter = sysmonActiveList.Count;
            int notRunningCounter = sysmonNotActiveList.Count;

            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            valueList.Add(new KeyValuePair<string, int>("Sysmon is running", runningCounter));
            valueList.Add(new KeyValuePair<string, int>("Sysmon is not running", notRunningCounter));

            return valueList;
        }

        public List<KeyValuePair<string, int>> LoadPieChartData
        {
            get => loadPieChartData();

        }

        public List<string> SysmonNotActiveList
        {
            get => sysmonNotActiveList;
        }

        public List<string> SysmonActiveList
        {
            get => sysmonActiveList;
        }

        private void ShowTreeStructure()
        {
            Messenger.Default.Send(new ChangeView(typeof(TreeStructureResultViewModel)));
        }

        private void Back()
        {
            ShowTreeStructure();
        }
    }
}

