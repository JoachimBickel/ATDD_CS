using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

using Viewer.Core.App;
using Viewer.Core.Geometry;
using Viewer.Core.Ports;

namespace Viewer.Wpf;

// The view model: implements the View outbound port. The core pushes state;
// the view model converts it once into bindable values and raises change
// notifications; the XAML views just bind. Kept humble — pass-through only;
// anything algorithmic lives in extracted conversion functions.
public class ViewerViewModel : IView, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    // Inbound: the core service, wired in App; the views drive it through us.
    public ViewerService? Service { get; set; }

    private int _vertexCount;
    public int VertexCount
    {
        get => _vertexCount;
        private set => SetField(ref _vertexCount, value);
    }

    private int _triangleCount;
    public int TriangleCount
    {
        get => _triangleCount;
        private set => SetField(ref _triangleCount, value);
    }

    private string _sizeText = "–";
    public string SizeText
    {
        get => _sizeText;
        private set => SetField(ref _sizeText, value);
    }

    public void OpenModel(string path) => Service?.OpenModel(path);

    public void ShowModel(Mesh mesh)
    {
        // Presented in stage 2 (viewport rendering).
    }

    public void ShowModelInfo(ModelInfo info)
    {
        VertexCount = info.VertexCount;
        TriangleCount = info.TriangleCount;

        var extent = info.Bounds.Max - info.Bounds.Min;
        SizeText = string.Create(CultureInfo.InvariantCulture,
            $"{extent.X:G4} × {extent.Y:G4} × {extent.Z:G4}");
    }

    public void ShowCamera(CameraState camera)
    {
        // Presented in stage 2 (viewport rendering).
    }

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (!EqualityComparer<T>.Default.Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}