using System.Windows.Media.Media3D;

using Viewer.Core.Geometry;
using Xunit;

namespace Viewer.Wpf.Tests;

// Unit tests for the adapter's one piece of real logic: converting a core mesh
// into WPF geometry. Conversions only — behavior scenarios live in
// Viewer.Core.Tests, never here.
public class MeshGeometryBuilderTests
{
    // A unit right triangle in the XY plane, counter-clockwise seen from +Z.
    private static Mesh TriangleMesh()
    {
        var mesh = new Mesh();
        mesh.AddVertex(new Vec3(0, 0, 0));
        mesh.AddVertex(new Vec3(1, 0, 0));
        mesh.AddVertex(new Vec3(0, 1, 0));
        mesh.AddTriangle(new Triangle(0, 1, 2));
        return mesh;
    }

    [Fact]
    public void Converts_a_triangle_to_its_corner_positions()
    {
        var geometry = MeshGeometryBuilder.ToGeometry(TriangleMesh());

        Assert.Equal(3, geometry.Positions.Count);
        Assert.Equal(new Point3D(0, 0, 0), geometry.Positions[0]);
        Assert.Equal(new Point3D(1, 0, 0), geometry.Positions[1]);
        Assert.Equal(new Point3D(0, 1, 0), geometry.Positions[2]);
    }
}