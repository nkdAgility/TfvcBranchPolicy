using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using TfvcBranchPolicy.CheckinPolicy.Common;
using TfvcBranchPolicy.CheckinPolicy.Editor;
using TfvcBranchPolicy.Common;

namespace TfvcBranchPolicy.CheckinPolicy.DemoUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<BranchPattern> branchPatterns = new List<BranchPattern>();
            var wpfwindow = new BranchLockPolicyEditorWindow(branchPatterns);
            ElementHost.EnableModelessKeyboardInterop(wpfwindow);
            wpfwindow.ShowDialog();
            branchPatterns = wpfwindow.ViewModel.GetBranchPatterns().ToList();
            this.Close();
        }
    }
}
