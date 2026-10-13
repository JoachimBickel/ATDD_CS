using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Viewer.Wpf;

// The 3D viewport: renders whatever the view model exposes — bound camera,
// bound headlight, bound flat-shaded geometry. Mouse input is only translated
// here — drag/wheel become orbit/zoom calls on the view model.
public partial class ViewportControl : UserControl
{
    private Point _lastMousePosition;

    public ViewportControl()
    {
        InitializeComponent();
    }

    private ViewerViewModel ViewModel => (ViewerViewModel)DataContext;

    protected override void OnMouseDown(MouseButtonEventArgs e)
    {
        base.OnMouseDown(e);
        _lastMousePosition = e.GetPosition(this);
        CaptureMouse();
    }

    protected override void OnMouseUp(MouseButtonEventArgs e)
    {
        base.OnMouseUp(e);
        ReleaseMouseCapture();
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);

        var position = e.GetPosition(this);
        var deltaX = position.X - _lastMousePosition.X;
        var deltaY = position.Y - _lastMousePosition.Y;
        _lastMousePosition = position;

        // Only orbit drags that started on this control (capture is taken in
        // OnMouseDown). WPF reports the global button state here — without
        // this guard, closing the file dialog by double-click leaks a
        // still-pressed button into a huge phantom first drag.
        if (IsMouseCaptured && e.LeftButton == MouseButtonState.Pressed)
        {
            // Translation only: pixels -> radians; the camera logic lives in
            // the core. The model follows the mouse: dragging right turns its
            // right side away from you (negative yaw), dragging down tips its
            // top towards you (positive pitch, as screen Y grows downward).
            const double radiansPerPixel = 0.005;
            ViewModel.Orbit(-deltaX * radiansPerPixel, deltaY * radiansPerPixel);
        }
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

        // One 15-degree wheel notch (delta 120) scales the distance by 0.9.
        var notches = e.Delta / 120.0;
        ViewModel.Zoom(Math.Pow(0.9, notches));
    }
}
