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

        foreach (var vertex in mesh.Vertices)
        {
            geometry.Positions.Add(new Point3D(vertex.X, vertex.Y, vertex.Z));
        }

        return geometry;
    }
}