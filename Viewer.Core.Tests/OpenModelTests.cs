using Viewer.Core.App;
using Viewer.Core.Geometry;
using Viewer.Core.Ports;
using Xunit;

namespace Viewer.Core.Tests;

// Outbound-port fake: hands back canned file content, no filesystem involved.
internal sealed class FakeModelSource(string content) : IModelSource
{
    public string Read(string path) => content;
}

// Outbound-port fake: captures what the core asks the UI to display.
internal sealed class FakeView : IView
{
    public bool ModelShown { get; private set; }
    public Mesh ShownMesh { get; private set; } = new();

    public bool InfoShown { get; private set; }
    public ModelInfo ShownInfo { get; private set; }

    public bool CameraShown { get; private set; }
    public CameraState ShownCamera { get; private set; }

    public void ShowModel(Mesh mesh)
    {
        ModelShown = true;
        ShownMesh = mesh;
    }

    public void ShowModelInfo(ModelInfo info)
    {
        InfoShown = true;
        ShownInfo = info;
    }

    public void ShowCamera(CameraState camera)
    {
        CameraShown = true;
        ShownCamera = camera;
    }
}

public class OpenModelTests
{
    private const string TriangleObj = """
        v 0 0 0
        v 1 0 0
        v 0 1 0
        f 1 2 3
        """;

    // The one-triangle model reused across the loading cases.
    private readonly FakeView _view = OpenModelWith(TriangleObj);

    // Arrange + act: open a model from the given OBJ text, returning what the
    // view was shown. The single home for wiring the fakes to the service.
    private static FakeView OpenModelWith(string objText)
    {
        var view = new FakeView();
        var service = new ViewerService(new FakeModelSource(objText), view);
        service.OpenModel("model.obj");
        return view;
    }

    [Fact]
    public void Opening_a_model_displays_it_in_the_view()
    {
        Assert.True(_view.ModelShown);
        Assert.Equal(3, _view.ShownMesh.Vertices.Count);
        Assert.Single(_view.ShownMesh.Triangles);
    }

    [Fact]
    public void Opening_a_model_shows_its_info()
    {
        Assert.True(_view.InfoShown);
        Assert.Equal(3, _view.ShownInfo.VertexCount);
        Assert.Equal(1, _view.ShownInfo.TriangleCount);
    }

    [Fact]
    public void Opening_a_model_shows_its_bounding_box()
    {
        Assert.Equal(new Vec3(0, 0, 0), _view.ShownInfo.Bounds.Min);
        Assert.Equal(new Vec3(1, 1, 0), _view.ShownInfo.Bounds.Max);
    }

    [Fact]
    public void Opening_a_model_frames_it()
    {
        Assert.True(_view.CameraShown);
        Assert.Equal(new Vec3(0.5, 0.5, 0), _view.ShownCamera.Target);
    }

    [Fact]
    public void Framing_places_the_camera_back_from_the_target()
    {
        Assert.Equal(new Vec3(0, 1, 0), _view.ShownCamera.Up);

        Assert.Equal(0.5, _view.ShownCamera.Eye.X);
        Assert.Equal(0.5, _view.ShownCamera.Eye.Y);
        Assert.Equal(Math.Sqrt(2), _view.ShownCamera.Eye.Z, 1e-9);
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
