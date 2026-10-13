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
        Service.Orbit(Math.PI / 2, 0.0);

        // Framed eye (0.5, 0.5, sqrt(2)) rotated 90 deg about the vertical axis
        // through the target (0.5, 0.5, 0) lands at (0.5 + sqrt(2), 0.5, 0).
        Assert.Equal(0.5 + Math.Sqrt(2), View.ShownCamera.Eye.X, 1e-9);
        Assert.Equal(0.5, View.ShownCamera.Eye.Y);
        Assert.Equal(0.0, View.ShownCamera.Eye.Z, 1e-9);

        Assert.Equal(new Vec3(0.5, 0.5, 0), View.ShownCamera.Target);
    }

    [Fact]
    public void Pitching_up_raises_the_eye_and_keeps_its_distance()
    {
        Service.Orbit(0.0, Math.PI / 4);

        // The framed eye (0.5, 0.5, sqrt(2)) tilted 45 deg up over the target
        // (0.5, 0.5, 0) lands at (0.5, 1.5, 1): still sqrt(2) away.
        Assert.Equal(0.5, View.ShownCamera.Eye.X, 1e-9);
        Assert.Equal(1.5, View.ShownCamera.Eye.Y, 1e-9);
        Assert.Equal(1.0, View.ShownCamera.Eye.Z, 1e-9);

        Assert.Equal(new Vec3(0.5, 0.5, 0), View.ShownCamera.Target);
    }

    [Fact]
    public void Orbiting_continues_over_the_top_and_turns_the_view_upside_down()
    {
        // Two quarter tilts pass straight over the target, where a fixed up
        // direction would leave the camera's sideways direction undefined.
        Service.Orbit(0.0, Math.PI / 2);
        Service.Orbit(0.0, Math.PI / 2);

        // Behind the model at (0.5, 0.5, -sqrt(2)), looking back, upside down.
        Assert.Equal(0.5, View.ShownCamera.Eye.X, 1e-9);
        Assert.Equal(0.5, View.ShownCamera.Eye.Y, 1e-9);
        Assert.Equal(-Math.Sqrt(2), View.ShownCamera.Eye.Z, 1e-9);

        Assert.Equal(0.0, View.ShownCamera.Up.X, 1e-9);
        Assert.Equal(-1.0, View.ShownCamera.Up.Y, 1e-9);
        Assert.Equal(0.0, View.ShownCamera.Up.Z, 1e-9);
    }

    [Fact]
    public void Turning_after_tilting_keeps_the_height_and_never_rolls()
    {
        Service.Orbit(0.0, Math.PI / 4);
        Service.Orbit(Math.PI / 3, 0.0);

        var offset = View.ShownCamera.Eye - View.ShownCamera.Target;
        var sideways = Vec3.Cross(offset, View.ShownCamera.Up);

        // Left/right turns about the world's vertical: the eye keeps its height,
        // and the camera's sideways direction stays level, so nothing rolls.
        Assert.Equal(1.5, View.ShownCamera.Eye.Y, 1e-9);
        Assert.Equal(0.0, sideways.Y, 1e-9);

        // Up stays at right angles to the viewing direction.
        Assert.Equal(0.0, Vec3.Dot(View.ShownCamera.Up, offset), 1e-9);
    }

    [Fact]
    public void Turning_while_upside_down_still_follows_the_mouse()
    {
        Service.Orbit(0.0, Math.PI);
        Service.Orbit(Math.PI / 2, 0.0);

        // Upside down behind the model, the same turn lands where it does from
        // the upright front view, (0.5 + sqrt(2), 0.5, 0); unreversed it would
        // land on the opposite side.
        Assert.Equal(0.5 + Math.Sqrt(2), View.ShownCamera.Eye.X, 1e-9);
        Assert.Equal(0.5, View.ShownCamera.Eye.Y, 1e-9);
        Assert.Equal(0.0, View.ShownCamera.Eye.Z, 1e-9);
    }
}
