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
}

public class OpenModelTests
{
    [Fact]
    public void Opening_a_model_displays_it_in_the_view()
    {
        var source = new FakeModelSource("""
            v 0 0 0
            v 1 0 0
            v 0 1 0
            f 1 2 3
            """);
        var view = new FakeView();
        var service = new ViewerService(source, view);

        service.OpenModel("triangle.obj");

        Assert.True(view.ModelShown);
        Assert.Equal(3, view.ShownMesh.Vertices.Count);
        Assert.Single(view.ShownMesh.Triangles);
    }

    [Fact]
    public void Opening_a_model_shows_its_info()
    {
        var source = new FakeModelSource("""
            v 0 0 0
            v 1 0 0
            v 0 1 0
            f 1 2 3
            """);
        var view = new FakeView();
        var service = new ViewerService(source, view);

        service.OpenModel("triangle.obj");

        Assert.True(view.InfoShown);
        Assert.Equal(3, view.ShownInfo.VertexCount);
        Assert.Equal(1, view.ShownInfo.TriangleCount);
    }
}
