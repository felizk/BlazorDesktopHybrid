using System.IO;
using System.Windows;
using BlazorDesktopHybrid.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorDesktopHybrid.Desktop;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        var appDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), nameof(BlazorDesktopHybrid));

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddWpfBlazorWebView();
        serviceCollection.AddBlazorWebViewDeveloperTools();
        serviceCollection.AddSharedServices();
        serviceCollection.AddScoped<ILocalStorage, AppDataUserLocalStorage>(x => new AppDataUserLocalStorage(appDataFolder));
        Resources.Add("services", serviceCollection.BuildServiceProvider());
    }
}