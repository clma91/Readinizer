using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readinizer.Frontend.Messages
{
    public class ChangeView
    {
        public Type ViewModelType { get; private set; }
        public int RefId { get; set; }
        public string Visability { get; set; }

        public ChangeView(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public ChangeView(Type viewModelType, int refId)
        {
            ViewModelType = viewModelType;
            RefId = refId;
        }

        public ChangeView(Type viewModelType, string visability)
        {
            ViewModelType = viewModelType;
            Visability = visability;
        }
    }
}
