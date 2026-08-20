using System.Windows;

using Viewer.Core.App;

namespace Viewer.Wpf;

// Composition root: wires the core use case to the WPF implementations of its
// ports. The view model sits on both sides — it implements the View outbound
// port and forwards the views' commands to the service.
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var viewModel = new ViewerViewModel();
        var service = new ViewerService(new FileModelSource(), viewModel);
        viewModel.Service = service;

        var window = new MainWindow { DataContext = viewModel };
        window.Show();
    }
}