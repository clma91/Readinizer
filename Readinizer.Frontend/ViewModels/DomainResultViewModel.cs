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
    public class DomainResultViewModel : ViewModelBase, IDomainResultViewModel
    {
        private readonly IUnitOfWork unitOfWork;


        private ICommand backCommand;
        public ICommand BackCommand => backCommand ?? (backCommand = new RelayCommand(() => this.Back()));

        [Obsolete("Only for design data", true)]
        public DomainResultViewModel()
        {
            if (!this.IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public DomainResultViewModel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork; 
        }

        public int RefId { get; set; }

        private ADDomain Domain { get; set; }

        public string Domainname { get => Domain.Name; }

        private List<RsopPot> goodList { get; set; }

        private List<RsopPot> badList { get; set; }

        public List<RsopPot> RsopPots { get; set; }

        public void loadRsopPots()
        {
            Domain = unitOfWork.ADDomainRepository.GetByID(RefId);
            RsopPots = Domain.RsopPots;
            fillLists();
        }

        private void fillLists()
        {
            List<RsopPot> bad = new List<RsopPot>();
            List<RsopPot> good = new List<RsopPot>();
            foreach (var pot in RsopPots)
            {
                if (pot.Rsops.FirstOrDefault().RsopPercentage > 99)
                {
                    good.Add(pot);
                    
                }
                else
                {
                    bad.Add(pot);
                }
            }

            goodList = good;
            badList = bad;

        }

        private List<KeyValuePair<string, int>> loadPieChartData()
        {
            int goodPots = GoodList.Count;
            int badPots = BadList.Count;

            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            valueList.Add(new KeyValuePair<string, int>("Correct", goodPots));
            valueList.Add(new KeyValuePair<string, int>("Not Correct", badPots));

            return valueList;
        }

        public List<KeyValuePair<string, int>> LoadPieChartData
        {
            get => loadPieChartData();

        }

        public List<RsopPot> GoodList
        {
            get => goodList;
        }

        public List<RsopPot> BadList
        {
            get => badList;
        }

        private string potName;
        public string PotName
        {
            get { return potName; }
            set
            {
                potName = value;
                int rsopPotID = RsopPots.Find(x => x.Name.Equals(potName)).RsopPotId;
                ShowPotView(rsopPotID);
                potName = null;
            }
        }

        private void ShowPotView(int potRefId)
        {
           Messenger.Default.Send(new ChangeView(typeof(RSoPResultViewModel), potRefId));

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

