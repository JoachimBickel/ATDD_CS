using Viewer.Core.Geometry;

namespace Viewer.Core.Ports;

// View-facing camera pose. The adapter feeds these three vectors to its
// framework's look-at (and a projection with the viewport aspect) to render;
// no matrix math lives in the core. Part of the View port's data contract.
public readonly record struct CameraState
{
    public Vec3 Eye { get; init; }
    public Vec3 Target { get; init; }
    public Vec3 Up { get; init; }
}
