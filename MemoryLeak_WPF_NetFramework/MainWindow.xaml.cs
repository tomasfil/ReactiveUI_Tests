﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MemoryLeak_WPF_NetFramework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<TestModel> Models = new ObservableCollection<TestModel>();
        private bool doIt = true;
        public MainWindow()
        {
            InitializeComponent();
            this.Main_DataGrid.ItemsSource = Models;
            // this.Main_ItemsControl.ItemsSource = Models;

            Task.Run(StartCycle);
            Task.Run(BreakCycle);
        }

        private async Task BreakCycle()
        {
            await Task.Delay(15000);
            doIt = false;
        }

        private async Task StartCycle()
        {
            while (doIt)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    foreach (var model in Enumerable.Range(0, 500).Select(_ => new TestModel()).ToList())
                    {
                        Models.Add(model);
                    }
                });
                await Task.Delay(200);
            }
            Application.Current.Dispatcher.Invoke(() => Models.Clear());
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            while (doIt)
            {
                    foreach (var model in Enumerable.Range(0, 500).Select(_ => new TestModel()).ToList())
                    {
                        Models.Add(model);
                    }
                await Task.Delay(200);
            }
            await Task.Delay(2000);
            Models.Clear();
        }
    }
}
