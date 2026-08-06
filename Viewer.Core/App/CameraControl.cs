using Viewer.Core.Analysis;
using Viewer.Core.Geometry;
using Viewer.Core.Ports;

namespace Viewer.Core.App;

// Pure camera math for the viewer's interaction commands.
public static class CameraControl
{
    // Positions the camera to frame a model: the target is the bounding-box
    // center. Eye and up are left at their defaults for now and driven by
    // later cycles.
    public static CameraState FrameModel(Mesh mesh)
    {
        var bounds = MeshBounds.Of(mesh);
        var target = new Vec3(
            (bounds.Min.X + bounds.Max.X) / 2.0,
            (bounds.Min.Y + bounds.Max.Y) / 2.0,
            (bounds.Min.Z + bounds.Max.Z) / 2.0);
        return new CameraState { Target = target };
    }
}
