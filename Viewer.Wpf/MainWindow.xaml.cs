using System.Windows;
using System.Windows.Input;

using Microsoft.Win32;

namespace Viewer.Wpf;

// The application shell: viewport in the center, info panel docked right,
// File -> Open. Code-behind holds only the dialog; everything else binds.
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private ViewerViewModel ViewModel => (ViewerViewModel)DataContext;

    private void OpenModel_Executed(object sender, ExecutedRoutedEventArgs e)
    {
        var dialog = new OpenFileDialog { Filter = "Wavefront OBJ (*.obj)|*.obj" };
        if (dialog.ShowDialog(this) == true)
        {
            ViewModel.OpenModel(dialog.FileName);
        }
    }
}