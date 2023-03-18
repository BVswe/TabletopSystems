using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TabletopSystems.Database_Access;
using TabletopSystems.Helper_Methods;
using TabletopSystems.ViewModels;

namespace TabletopSystems
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder().ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<MainWindow>(provider => new MainWindow
                {
                    DataContext = provider.GetRequiredService<MainWindowViewModel>()
                });
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<TestViewModel>();
                services.AddSingleton<ITabletopSystemRepository, SqlTabletopSystemRepository>();
                services.AddSingleton<UserConnection>(sp =>
                {
                    string connectionString = sp.GetService<MainWindowViewModel>().connection;
                    return new UserConnection(connectionString);
                });
                services.AddSingleton<SystemSelectionViewModel>(sp =>
                {
                    TabletopSystem tabltp = new TabletopSystem { SystemID = 1, SystemName = "HI" };
                    return new SystemSelectionViewModel(sp.GetService<UserConnection>(), tabltp);
                });
                services.AddSingleton<INavigationService, NavigationService>();
                services.AddSingleton<Func<Type, UserConnection>>(  
                    serviceProvider => connectionToGet => (UserConnection)serviceProvider.GetRequiredService(connectionToGet));
                services.AddSingleton<Func<Type, ObservableObject>>(
                serviceProvider => viewModelType => (ObservableObject)serviceProvider.GetRequiredService(viewModelType));
                
            }).Build();
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost!.StartAsync();

            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
