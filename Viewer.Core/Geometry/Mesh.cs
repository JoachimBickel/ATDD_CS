namespace Viewer.Core.Geometry;

// Indexed triangle mesh: a list of vertices and the triangles referencing them.
public class Mesh
{
    private readonly List<Vec3> _vertices = [];
    private readonly List<Triangle> _triangles = [];

    public IReadOnlyList<Vec3> Vertices => _vertices;
    public IReadOnlyList<Triangle> Triangles => _triangles;

    public void AddVertex(Vec3 vertex) => _vertices.Add(vertex);
    public void AddTriangle(Triangle triangle) => _triangles.Add(triangle);
}
