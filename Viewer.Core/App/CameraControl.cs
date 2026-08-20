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
            Eye = center + new Vec3(0.0, 0.0, distance),
            Up = new Vec3(0.0, 1.0, 0.0),
        };
    }

    // Moves the eye along the eye->target line by the given factor (< 1 zooms
    // in, > 1 zooms out). Target and up are unchanged.
    public static CameraState Zoom(CameraState camera, double factor) =>
        camera with { Eye = camera.Target + (camera.Eye - camera.Target) * factor };

    // Rotates the eye around the target about the world-up (Y) axis by yaw
    // radians (a turntable orbit). Target and up are unchanged.
    public static CameraState Orbit(CameraState camera, double yaw)
    {
        var offset = camera.Eye - camera.Target;
        var cosYaw = Math.Cos(yaw);
        var sinYaw = Math.Sin(yaw);

        var rotated = new Vec3(
            offset.X * cosYaw + offset.Z * sinYaw,
            offset.Y,
            -offset.X * sinYaw + offset.Z * cosYaw);

        return camera with { Eye = camera.Target + rotated };
    }
}
