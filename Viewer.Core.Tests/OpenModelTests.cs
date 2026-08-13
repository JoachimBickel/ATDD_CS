using Viewer.Core.Geometry;
using Viewer.Core.Tests.Support;
using Xunit;

namespace Viewer.Core.Tests;

public class OpenModelTests : OpenedTriangleModel
{
    [Fact]
    public void Opening_a_model_displays_it_in_the_view()
    {
        Assert.True(View.ModelShown);
        Assert.Equal(3, View.ShownMesh.Vertices.Count);
        Assert.Single(View.ShownMesh.Triangles);
    }

    [Fact]
    public void Opening_a_model_shows_its_info()
    {
        Assert.True(View.InfoShown);
        Assert.Equal(3, View.ShownInfo.VertexCount);
        Assert.Equal(1, View.ShownInfo.TriangleCount);
    }

    [Fact]
    public void Opening_a_model_shows_its_bounding_box()
    {
        Assert.Equal(new Vec3(0, 0, 0), View.ShownInfo.Bounds.Min);
        Assert.Equal(new Vec3(1, 1, 0), View.ShownInfo.Bounds.Max);
    }

    [Fact]
    public void Opening_a_model_frames_it()
    {
        Assert.True(View.CameraShown);
        Assert.Equal(new Vec3(0.5, 0.5, 0), View.ShownCamera.Target);
    }

    [Fact]
    public void Framing_places_the_camera_back_from_the_target()
    {
        Assert.Equal(new Vec3(0, 1, 0), View.ShownCamera.Up);

        Assert.Equal(0.5, View.ShownCamera.Eye.X);
        Assert.Equal(0.5, View.ShownCamera.Eye.Y);
        Assert.Equal(Math.Sqrt(2), View.ShownCamera.Eye.Z, 1e-9);
    }

    [Fact]
    public void Parses_real_world_obj_with_comments_blanks_and_slash_faces()
    {
        var view = OpenModelWith("""
            # exported by something
            o triangle

            v 0 0 0
            v 1 0 0
            v 0 1 0

            # the face
            f 1/1/1 2/2/2 3/3/3
            """);

        Assert.Equal(3, view.ShownMesh.Vertices.Count);
        Assert.Single(view.ShownMesh.Triangles);
        Assert.Equal(new Triangle(0, 1, 2), view.ShownMesh.Triangles[0]);
    }
}