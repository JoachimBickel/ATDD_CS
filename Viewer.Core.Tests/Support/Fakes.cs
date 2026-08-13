using Viewer.Core.Geometry;
using Viewer.Core.Ports;

namespace Viewer.Core.Tests.Support;

// Outbound-port fake: hands back canned file content, no filesystem involved.
public sealed class FakeModelSource(string content) : IModelSource
{
    public string Read(string path) => content;
}

// Outbound-port fake: captures what the core asks the UI to display.
public sealed class FakeView : IView
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