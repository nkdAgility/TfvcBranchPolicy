using System;
using System.Collections.Generic;
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
using TfvcBranchPolicy.CheckinPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy.Editor
{
    /// <summary>
    /// Interaction logic for BranchLockPolicyEditorWindow.xaml
    /// </summary>
    public partial class BranchLockPolicyEditorWindow : Window
    {
        public IBranchPatternPolicyEditorViewModel ViewModel;
        public BranchLockPolicyEditorWindow(IEnumerable<BranchPattern> collection)
        {
            InitializeComponent();
            ViewModel = new BranchPatternPolicyEditorViewModel(collection);
            this.DataContext = ViewModel;
   
        }



    }
}
