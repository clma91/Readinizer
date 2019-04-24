using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Readinizer.Backend.Business.Interfaces;
using Readinizer.Frontend.Interfaces;
using Readinizer.Frontend.Messages;

namespace Readinizer.Frontend.ViewModels
{
    public class SpinnerViewModel : ViewModelBase, ISpinnerViewModel
    {
        private readonly IADDomainService adDomainService;

        [Obsolete("Only for design data", true)]
        public SpinnerViewModel()
        {
            if (!IsInDesignMode)
            {
                throw new Exception("Use only for design mode");
            }
        }

        public SpinnerViewModel(IADDomainService adDomainService)
        {
            this.adDomainService = adDomainService;
        }
    }
}
