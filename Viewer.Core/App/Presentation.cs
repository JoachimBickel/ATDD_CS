using Viewer.Core.Analysis;
using Viewer.Core.Geometry;
using Viewer.Core.Ports;

namespace Viewer.Core.App;

// Presentation mapping: turns a mesh, and the path it was opened from, into the
// view-facing summary shown in the info panel. Grows as more metrics are
// surfaced; kept out of ViewerService so that use case stays pure orchestration.
public static class Presentation
{
    public static ModelInfo DescribeModel(Mesh mesh, string path) =>
        new(
            mesh.Name.Length > 0 ? mesh.Name : FileStem(path),
            mesh.Vertices.Count,
            mesh.Triangles.Count,
            MeshBounds.Of(mesh));

    // The file name without directory or extension. Both '/' and '\' count as
    // separators on every OS, unlike System.IO.Path off Windows.
    private static string FileStem(string path)
    {
        var fileName = path[(path.LastIndexOfAny(['/', '\\']) + 1)..];
        var dot = fileName.LastIndexOf('.');
        return dot < 0 ? fileName : fileName[..dot];
    }
}
