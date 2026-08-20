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

    [Fact]
    public void Gives_every_corner_the_face_normal()
    {
        var geometry = MeshGeometryBuilder.ToGeometry(TriangleMesh());

        // Counter-clockwise seen from +Z, so the unit normal points at +Z.
        Assert.Equal(3, geometry.Normals.Count);
        Assert.All(geometry.Normals, normal => Assert.Equal(new Vector3D(0, 0, 1), normal));
    }

    [Fact]
    public void Duplicates_shared_vertices_so_each_face_keeps_its_normal()
    {
        // Two faces folded along the shared edge v0-v1: one in the XY plane
        // (normal +Z), one in the XZ plane (normal +Y). Flat shading needs one
        // position per face corner — shared vertices must not share normals.
        var mesh = new Mesh();
        mesh.AddVertex(new Vec3(0, 0, 0));
        mesh.AddVertex(new Vec3(1, 0, 0));
        mesh.AddVertex(new Vec3(0, 1, 0));
        mesh.AddVertex(new Vec3(0, 0, 1));
        mesh.AddTriangle(new Triangle(0, 1, 2));
        mesh.AddTriangle(new Triangle(1, 0, 3));

        var geometry = MeshGeometryBuilder.ToGeometry(mesh);

        Assert.Equal(6, geometry.Positions.Count);
        Assert.Equal(new Point3D(1, 0, 0), geometry.Positions[3]);
        Assert.Equal(new Point3D(0, 0, 0), geometry.Positions[4]);
        Assert.Equal(new Point3D(0, 0, 1), geometry.Positions[5]);

        Assert.Equal(6, geometry.Normals.Count);
        Assert.All(geometry.Normals.Take(3), normal => Assert.Equal(new Vector3D(0, 0, 1), normal));
        Assert.All(geometry.Normals.Skip(3), normal => Assert.Equal(new Vector3D(0, 1, 0), normal));
    }
}