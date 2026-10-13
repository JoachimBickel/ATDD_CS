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

    // Turntable orbit around the target: yaw turns the eye about the world's
    // vertical axis, then pitch tilts it about the camera's sideways axis
    // (positive pitch raises the eye). Up turns and tilts along with the eye, so
    // it stays at right angles to the view and the orbit can pass over the top;
    // turning about the world's vertical keeps the sideways axis level, so the
    // view never rolls. While upside down the turn is reversed, so the model
    // still follows the mouse; exactly overhead counts as upright. The target is
    // unchanged.
    public static CameraState Orbit(CameraState camera, double yaw, double pitch)
    {
        var worldUp = new Vec3(0.0, 1.0, 0.0);
        var turn = camera.Up.Y < 0.0 ? -yaw : yaw;
        var offset = RotateAbout(camera.Eye - camera.Target, worldUp, turn);
        var up = RotateAbout(camera.Up, worldUp, turn);

        var sideways = Vec3.Cross(offset, up).Normalized();
        offset = RotateAbout(offset, sideways, pitch);
        up = RotateAbout(up, sideways, pitch);

        return camera with { Eye = camera.Target + offset, Up = up };
    }

    // Rotates v by angle radians about the unit-length axis (Rodrigues'
    // formula), counter-clockwise when looking down the axis towards the origin.
    private static Vec3 RotateAbout(Vec3 v, Vec3 axis, double angle)
    {
        var cosAngle = Math.Cos(angle);
        var sinAngle = Math.Sin(angle);
        return v * cosAngle + Vec3.Cross(axis, v) * sinAngle
            + axis * (Vec3.Dot(axis, v) * (1.0 - cosAngle));
    }
}
