using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TabletopSystems.Database_Access;
using TabletopSystems.Factories;
using TabletopSystems.Helper_Methods;
using TabletopSystems.ViewModels;
using TabletopSystems.Views;

namespace TabletopSystems
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        public App()
        {
            AppHost = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<UserConnection>();
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<MainWindow>(provider => new MainWindow
                {
                    DataContext = provider.GetRequiredService<MainWindowViewModel>()
                });
                services.AddSingleton<Func<Type, ObservableObject>>(
                serviceProvider => viewModelType => (ObservableObject)serviceProvider.GetRequiredService(viewModelType));
                //NavigationService is transient because some screens need a separate NavigationService
                services.AddSingleton<Func<Type, IViewVM>>(
                serviceProvider => viewModelType => (IViewVM)serviceProvider.GetRequiredService(viewModelType));
                services.AddSingleton<DisplayItemViewFactory>();
                services.AddTransient<INavigationService, NavigationService>();
                services.AddTransient<SystemSelectionViewModel>();
                services.AddTransient<SystemMainPageViewModel>();
                services.AddTransient<ITabletopSystemRepository, TabletopSystemRepository>();
                services.AddTransient<AddSystemViewModel>();
                services.AddTransient<AddItemMainViewModel>();
                services.AddTransient<CharactersViewModel>();
                services.AddTransient<SearchViewModel>();
                services.AddTransient<AddCapabilityViewModel>();
                services.AddTransient<AddClassViewModel>();
                services.AddTransient<AddGearViewModel>();
                services.AddTransient<AddRaceViewModel>();
                services.AddTransient<AddTagViewModel>();
                services.AddTransient<AddMonsterViewModel>();
                services.AddTransient<DisplayCapabilityViewModel>();
                services.AddTransient<DisplayGearViewModel>();

                //Make Character/monster viewmodels scoped so that you can switch back and forth between tabs?
                //Might be unnecessary bc tabcontrol doesnt go out of scope and its binded?

            }).Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();
            base.OnStartup(e);
            ((MainWindowViewModel)startupForm.DataContext).Navi.NavigateTo<SystemSelectionViewModel>();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
