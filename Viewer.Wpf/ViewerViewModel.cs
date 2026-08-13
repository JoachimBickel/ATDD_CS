using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Media.Media3D;

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

    private MeshGeometry3D _geometry = new();
    public MeshGeometry3D Geometry
    {
        get => _geometry;
        private set => SetField(ref _geometry, value);
    }

    private PerspectiveCamera _camera = new() { FieldOfView = 45 };
    public PerspectiveCamera Camera
    {
        get => _camera;
        private set => SetField(ref _camera, value);
    }

    private Vector3D _headlightDirection = new(0, 0, -1);
    public Vector3D HeadlightDirection
    {
        get => _headlightDirection;
        private set => SetField(ref _headlightDirection, value);
    }

    public void OpenModel(string path) => Service?.OpenModel(path);

    public void ShowModel(Mesh mesh) => Geometry = MeshGeometryBuilder.ToGeometry(mesh);

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
        var eye = new Point3D(camera.Eye.X, camera.Eye.Y, camera.Eye.Z);
        var target = new Point3D(camera.Target.X, camera.Target.Y, camera.Target.Z);
        var look = target - eye;

        // The adapter's one piece of camera work: feed the core's pose into
        // WPF's retained-mode camera; near/far bracket the framing distance.
        Camera = new PerspectiveCamera
        {
            Position = eye,
            LookDirection = look,
            UpDirection = new Vector3D(camera.Up.X, camera.Up.Y, camera.Up.Z),
            FieldOfView = 45,
            NearPlaneDistance = look.Length * 0.01,
            FarPlaneDistance = look.Length * 100,
        };

        // The light follows the camera, so form is readable from any angle.
        HeadlightDirection = look;
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