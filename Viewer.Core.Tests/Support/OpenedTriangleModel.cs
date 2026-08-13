using Viewer.Core.App;

namespace Viewer.Core.Tests.Support;

// The one-triangle model reused across the loading and interaction cases:
// deriving test classes start with a service that has just opened it and the
// view it presented to. Holds the live service so tests can drive further
// commands (zoom/orbit).
public abstract class OpenedTriangleModel
{
    protected const string TriangleObj = """
        v 0 0 0
        v 1 0 0
        v 0 1 0
        f 1 2 3
        """;

    protected readonly FakeView View = new();
    protected readonly ViewerService Service;

    protected OpenedTriangleModel()
    {
        Service = new ViewerService(new FakeModelSource(TriangleObj), View);
        Service.OpenModel("triangle.obj");
    }

    // Arrange + act: open a model from the given OBJ text, returning what the
    // view was shown. For one-off content tests that only inspect the view.
    protected static FakeView OpenModelWith(string objText)
    {
        var view = new FakeView();
        var service = new ViewerService(new FakeModelSource(objText), view);
        service.OpenModel("model.obj");
        return view;
    }
}