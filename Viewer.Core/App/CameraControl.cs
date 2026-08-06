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

        var dx = bounds.Max.X - bounds.Min.X;
        var dy = bounds.Max.Y - bounds.Min.Y;
        var dz = bounds.Max.Z - bounds.Min.Z;
        var diagonal = Math.Sqrt(dx * dx + dy * dy + dz * dz);

        return new CameraState
        {
            Target = center,
            Eye = new Vec3(center.X, center.Y, center.Z + diagonal),
            Up = new Vec3(0.0, 1.0, 0.0),
        };
    }
}
