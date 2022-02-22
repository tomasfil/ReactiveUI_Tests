using ReactiveUI;
using ReactiveUI_Akavache_Suspension.Suspension;
using ReactiveUI_Akavache_Suspension.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;

namespace ReactiveUI_Akavache_Suspension
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly AutoSuspendHelper autoSuspendHelper;
        public App()
        {
            // Initialize the suspension driver after AutoSuspendHelper.
            this.autoSuspendHelper = new AutoSuspendHelper(this);
            RxApp.SuspensionHost.CreateNewAppState = () => new MainViewModel();
            RxApp.SuspensionHost.SetupDefaultSuspendResume(new AkavacheSuspensionDriver<MainViewModel>());
        }
    }
}
