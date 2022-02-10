﻿using System;
using System.Windows.Controls;
using ValorantLauncher.ViewModels;

namespace ValorantLauncher.Views.ContentViews
{
    public partial class StoreView : UserControl
    {
        public StoreView()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
        }

        private async void OnLoaded(object sender, EventArgs e)
        {
            if (ItemsControl.Items.Count <= 0)
                await ((StoreViewModel)DataContext).GetStoreData();
        }
    }
}
