using Viewer.Core.Analysis;
using Viewer.Core.Geometry;
using Viewer.Core.Ports;

namespace Viewer.Core.App;

// Presentation mapping: turns a mesh into the view-facing summary shown in the
// info panel. Grows as more metrics are surfaced; kept out of ViewerService so
// that use case stays pure orchestration.
public static class Presentation
{
    public static ModelInfo DescribeModel(Mesh mesh) =>
        new(mesh.Vertices.Count, mesh.Triangles.Count, MeshBounds.Of(mesh));
}
