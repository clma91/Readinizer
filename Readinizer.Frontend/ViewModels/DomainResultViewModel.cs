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
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;

namespace Readinizer.Frontend.ViewModels
{
    public class DomainResultViewModel : ViewModelBase, IDomainResultViewModel
    {
        private readonly IADDomainService adDomainService;

        private ICommand rsopPotViewCommand;
        public ICommand RSoPPotViewCommand => rsopPotViewCommand ?? (rsopPotViewCommand = new RelayCommand(() => this.RSoPPotView(), () => this.CanRSoPPotView));

        public bool CanRSoPPotView { get; private set; }

        [Obsolete("Only for design data", true)]
        public DomainResultViewModel()
        {
            if (!this.IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public DomainResultViewModel(IADDomainService adDomainService)
        {
            this.adDomainService = adDomainService;
            CanRSoPPotView = true;
        }

        private string text = "Domainname";


        public string Text
        {
            get => text;
            set { Text = text; }
        }

        private string _rsopPot = null;
        public string RsopPot
        {
            get { return _rsopPot; }
            set
            {
                _rsopPot = value;
                RSoPPotView();
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
            goodList.Add("RsopPot-A");
            goodList.Add("RsopPot-D");
            goodList.Add("RsopPot-F");

            return goodList;
        }

        private List<string> _LoadPartiallyList()
        {
            List<string> partiallyList = new List<string>();
            partiallyList.Add("RsopPot-B");


            return partiallyList;
        }

        private List<string> _LoadBadList()
        {
            List<string> badList = new List<string>();
            badList.Add("RsopPot-C");
            badList.Add("RsopPot-H");


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

        private void RSoPPotView()
        {
           Messenger.Default.Send(new ChangeView(typeof(StartUpViewModel)));

        }
    }
}
