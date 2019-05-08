using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Readinizer.Backend.DataAccess;
using Readinizer.Frontend.ViewModels;

namespace Readinizer.Frontend.Views
{
    /// <summary>
    /// Interaction logic for ApplicationView.xaml
    /// </summary>
    public partial class ApplicationView : Window
    {
        public ApplicationView(ApplicationViewModel applicationViewModel)
        {
            InitializeComponent();
            this.DataContext = applicationViewModel;
        }
    }
}
