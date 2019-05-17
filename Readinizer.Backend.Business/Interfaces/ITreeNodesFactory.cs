using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Readinizer.Backend.Domain.Models;

namespace Readinizer.Backend.Business.Interfaces
{
    public interface ITreeNodesFactory
    {
        Task<ObservableCollection<TreeNode>> CreateTree();
    }
}
