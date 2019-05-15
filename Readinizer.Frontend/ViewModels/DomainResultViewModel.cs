using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public bool CanRSoPPotView { get; private set; }

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

        public string Domainname { get; set; }

        public int RefId { get; set; }

        private ADDomain domain { get; set; }

        private List<RsopPot> loadRsopPots()
        {
            var domain = unitOfWork.ADDomainRepository.GetByID(RefId);
            List<RsopPot> rsopPots = domain.RsopPots;

            return rsopPots;
        }

        public List<RsopPot> RsopPots
        {
            get => loadRsopPots();
        }

        private string _rsopPot = null;
        public string RsopPot
        {
            get { return _rsopPot; }
            set
            {
                _rsopPot = value;
                RSoPPotView(_rsopPot, 2);
            }
        }


        private List<KeyValuePair<string, int>> _LoadPieChartData()
        {

            List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
            valueList.Add(new KeyValuePair<string, int>("Good", 60));
            valueList.Add(new KeyValuePair<string, int>("Bad", 30));
            valueList.Add(new KeyValuePair<string, int>("Not Configured", 10));

            return valueList;
        }

        public List<KeyValuePair<string, int>> LoadPieChartData
        {
            get => _LoadPieChartData();

        }

        private List<string> _LoadGoodList()
        {
            List<string> goodList = new List<string>();
            foreach (var pot in RsopPots)
            {
                
            }
            return goodList;
        }

        private List<string> _LoadPartiallyList()
        {
            List<string> partiallyList = new List<string>();
            partiallyList.Add("GISS-2");
            return partiallyList;
        }

        private List<string> _LoadBadList()
        {
            List<string> badList = new List<string>();
            badList.Add("GISS-4");
            return badList;
        }

        public List<string> GoodList
        {
            get => _LoadGoodList();
        }

        public List<string> PartiallyList
        {
            get => _LoadPartiallyList();
        }

        public List<string> BadList
        {
            get => _LoadBadList();
        }

        private void RSoPPotView(string potName, int potRefId)
        {
           Messenger.Default.Send(new ChangeView(typeof(RSoPResultViewModel), potName, potRefId));

        }
    }
}
