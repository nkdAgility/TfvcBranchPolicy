﻿using Microsoft.TeamFoundation.VersionControl.Client;
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
        public BranchLockPolicyEditorWindow(IPolicyEditArgs policyEditArgs, IBranchPatternsRepository repo)
        {
            InitializeComponent();
            ViewModel = new BranchPatternPolicyEditorViewModel( policyEditArgs,repo);
            this.DataContext = ViewModel;
   
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



    }
}
