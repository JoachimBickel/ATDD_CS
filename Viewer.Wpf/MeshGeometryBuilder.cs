using System.Windows.Media.Media3D;

using Viewer.Core.Geometry;

namespace Viewer.Wpf;

// The adapter's extracted conversion logic: turns a core mesh into WPF
// geometry. Pure function — unit-tested headless in Viewer.Wpf.Tests.
public static class MeshGeometryBuilder
{
    public static MeshGeometry3D ToGeometry(Mesh mesh)
    {
        var geometry = new MeshGeometry3D();

        // De-index into a triangle soup: every face gets its own three corner
        // positions, so shared vertices can carry a different normal per face.
        foreach (var triangle in mesh.Triangles)
        {
            var a = ToPoint(mesh.Vertices[triangle.V0]);
            var b = ToPoint(mesh.Vertices[triangle.V1]);
            var c = ToPoint(mesh.Vertices[triangle.V2]);

            var normal = Vector3D.CrossProduct(b - a, c - a);
            normal.Normalize();

            geometry.Positions.Add(a);
            geometry.Positions.Add(b);
            geometry.Positions.Add(c);

            geometry.Normals.Add(normal);
            geometry.Normals.Add(normal);
            geometry.Normals.Add(normal);
        }

        return geometry;
    }

    private static Point3D ToPoint(Vec3 v) => new(v.X, v.Y, v.Z);
}