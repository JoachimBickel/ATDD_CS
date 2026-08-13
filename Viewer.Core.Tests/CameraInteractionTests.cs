using Viewer.Core.Geometry;
using Viewer.Core.Tests.Support;
using Xunit;

namespace Viewer.Core.Tests;

public class CameraInteractionTests : OpenedTriangleModel
{
    [Fact]
    public void Zooming_in_moves_the_eye_toward_the_target()
    {
        Service.Zoom(0.5);

        // Framing put the eye at (0.5, 0.5, sqrt(2)) aimed at (0.5, 0.5, 0);
        // zooming by 0.5 halves the eye->target distance, target unchanged.
        Assert.Equal(new Vec3(0.5, 0.5, 0), View.ShownCamera.Target);
        Assert.Equal(0.5, View.ShownCamera.Eye.X);
        Assert.Equal(0.5, View.ShownCamera.Eye.Y);
        Assert.Equal(Math.Sqrt(2) / 2, View.ShownCamera.Eye.Z, 1e-9);
    }

    [Fact]
    public void Orbiting_rotates_the_eye_around_the_target()
    {
        const double quarterTurn = Math.PI / 2;  // 90 degrees, in radians
        Service.Orbit(quarterTurn);

        // Framed eye (0.5, 0.5, sqrt(2)) rotated 90 deg about the vertical axis
        // through the target (0.5, 0.5, 0) lands at (0.5 + sqrt(2), 0.5, 0).
        Assert.Equal(0.5 + Math.Sqrt(2), View.ShownCamera.Eye.X, 1e-9);
        Assert.Equal(0.5, View.ShownCamera.Eye.Y);
        Assert.Equal(0.0, View.ShownCamera.Eye.Z, 1e-9);

        Assert.Equal(new Vec3(0.5, 0.5, 0), View.ShownCamera.Target);
    }
}