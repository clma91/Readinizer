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
        public string Key { get; set; }
        public int RefId { get; set; }

        public ChangeView(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public ChangeView(Type viewModelType, string key, int refId)
        {
            ViewModelType = viewModelType;
            Key = key;
            RefId = refId;
        }
    }
}
