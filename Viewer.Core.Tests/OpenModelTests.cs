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
    private const string TriangleObj = """
        v 0 0 0
        v 1 0 0
        v 0 1 0
        f 1 2 3
        """;

    private readonly FakeView _view = new();

    // Shared arrange + act: xUnit constructs the test class fresh per case, so
    // the constructor is the fixture — a service that has just opened a
    // one-triangle model.
    public OpenModelTests()
    {
        var service = new ViewerService(new FakeModelSource(TriangleObj), _view);
        service.OpenModel("triangle.obj");
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
}
