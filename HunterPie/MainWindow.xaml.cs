﻿using HunterPie.Domain.Sidebar;
using HunterPie.GUI.Parts.Sidebar;
using System.Windows;
using HunterPie.Core.Domain.Dialog;
using System.ComponentModel;
using HunterPie.UI.Overlay;
using HunterPie.UI.Overlay.Widgets.Monster;
using System;
using HunterPie.Core.Logger;
using HunterPie.UI.Overlay.Widgets.Abnormality.View;
using HunterPie.UI.Overlay.Widgets.Metrics.View;
using HunterPie.Internal;

namespace HunterPie
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            Log.Info("Initializing HunterPie GUI");
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {

            NativeDialogResult result = DialogManager.Info("Confirmation", "Are you sure you want to exit HunterPie?", NativeDialogButtons.Accept | NativeDialogButtons.Cancel);  
            
            if (result != NativeDialogResult.Accept)
            {
                e.Cancel = true;
                return;
            }

            base.OnClosing(e);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitializerManager.InitializeGUI();
        }
    }
}
