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

        private ADDomain domain { get => unitOfWork.ADDomainRepository.GetByID(RefId); }

        public string Domainname { get => domain.Name; }

        private List<string> goodList { get; set; }

        private List<string> badList { get; set; }

        public List<RsopPot> RsopPots
        {
            get => loadRsopPots();
        }

        private List<RsopPot> loadRsopPots()
        {
            
            List<RsopPot> rsopPots = domain.RsopPots;
            return rsopPots;
        }

        private void fillLists()
        {
            List<string> bad = new List<string>();
            List<string> good = new List<string>();
            foreach (var pot in RsopPots)
            {
                if (pot.Rsops.FirstOrDefault().RsopPercentage > 99)
                {
                    good.Add(pot.Name);
                    
                }
                else
                {
                    bad.Add(pot.Name);
                }
            }

            goodList = good;
            badList = bad;

        }

        private List<KeyValuePair<string, int>> _LoadPieChartData()
        {
            fillLists();
            int goodPots = GoodList.Count;
            int badPots = BadList.Count;

            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            valueList.Add(new KeyValuePair<string, int>("Correct", goodPots));
            valueList.Add(new KeyValuePair<string, int>("Not Correct", badPots));

            return valueList;
        }

        public List<KeyValuePair<string, int>> LoadPieChartData
        {
            get => _LoadPieChartData();

        }

        public List<string> GoodList
        {
            get => goodList;
        }

        public List<string> BadList
        {
            get => badList;
        }

        private string pot;
        public string Pot
        {
            get { return pot; }
            set
            {
                pot = value;
                int rsopPotID = RsopPots.Find(x => x.Name.Equals(pot)).RsopPotId;
                ShowPotView(rsopPotID);
                pot = null;
            }
        }

        private void ShowPotView(int potRefId)
        {
           Messenger.Default.Send(new ChangeView(typeof(RSoPResultViewModel), potRefId));

        }
    }
}
