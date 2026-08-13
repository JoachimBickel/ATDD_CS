using Viewer.Core.Geometry;

namespace Viewer.Core.Analysis;

// Axis-aligned bounding box of a mesh's vertices. An empty mesh yields a
// zero-sized box at the origin.
public static class MeshBounds
{
    public static BoundingBox Of(Mesh mesh)
    {
        if (mesh.Vertices.Count == 0)
        {
            return default;
        }

        var min = mesh.Vertices[0];
        var max = mesh.Vertices[0];
        foreach (var vertex in mesh.Vertices)
        {
            min = new Vec3(Math.Min(min.X, vertex.X), Math.Min(min.Y, vertex.Y), Math.Min(min.Z, vertex.Z));
            max = new Vec3(Math.Max(max.X, vertex.X), Math.Max(max.Y, vertex.Y), Math.Max(max.Z, vertex.Z));
        }

        return new BoundingBox(min, max);
    }
}
