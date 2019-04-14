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

        public ChangeView(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }
    }
}
