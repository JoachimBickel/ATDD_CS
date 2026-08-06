using Viewer.Core.Analysis;
using Viewer.Core.Geometry;
using Viewer.Core.Ports;

namespace Viewer.Core.App;

// Pure camera math for the viewer's interaction commands.
public static class CameraControl
{
    // Positions the camera to frame a model: it targets the bounding-box center
    // and sits back along +Z by the box diagonal (a simple, fov-independent
    // fit), Y up.
    public static CameraState FrameModel(Mesh mesh)
    {
        var bounds = MeshBounds.Of(mesh);
        var center = bounds.Center;
        var distance = bounds.Diagonal;

        return new CameraState
        {
            Target = center,
            Eye = new Vec3(center.X, center.Y, center.Z + distance),
            Up = new Vec3(0.0, 1.0, 0.0),
        };
    }

    // Moves the eye along the eye->target line by the given factor (< 1 zooms
    // in, > 1 zooms out). Target and up are unchanged.
    public static CameraState Zoom(CameraState camera, double factor) =>
        camera with
        {
            Eye = new Vec3(
                camera.Target.X + (camera.Eye.X - camera.Target.X) * factor,
                camera.Target.Y + (camera.Eye.Y - camera.Target.Y) * factor,
                camera.Target.Z + (camera.Eye.Z - camera.Target.Z) * factor),
        };
}
