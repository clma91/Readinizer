using System;
using System.Collections.Generic;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Frontend.Interfaces;


namespace Readinizer.Frontend.ViewModels
{
    public class DomainResultViewModel : ViewModelBase, IDomainResultViewModel
    {
        private readonly IADDomainService adDomainService;
        


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
        }

        private string text = "Domainname";
        

        public string Text
        {
            get => text;
            set { Text = text; }
        }


        private List<KeyValuePair<string, int>> _LoadPieChartData()
        {
            
                List<KeyValuePair<string, int>> valueList = new List<KeyValuePair<string, int>>();
                valueList.Add(new KeyValuePair<string, int>("Good", 50));
                valueList.Add(new KeyValuePair<string, int>("Bad", 25));
                valueList.Add(new KeyValuePair<string, int>("Not Configured", 25));


                return valueList;
        }

        public List<KeyValuePair<string, int>> LoadPieChartData
        {
            get => _LoadPieChartData();
            
        }
    }
}
