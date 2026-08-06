using System.Globalization;

using Viewer.Core.Geometry;

namespace Viewer.Core.IO;

// Parses Wavefront OBJ text into a Mesh. Pure core logic: works on in-memory
// text, so it is fully testable without touching the filesystem.
public class ObjImporter
{
    // Deliberately an instance method (not static): Parse becomes an interface
    // member on a MeshImporter port once more formats (e.g. STL) arrive.
    public Mesh Parse(string text)
    {
        var mesh = new Mesh();

        foreach (var line in text.Split('\n'))
        {
            var tokens = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (tokens is ["v", var x, var y, var z])
            {
                mesh.AddVertex(new Vec3(
                    double.Parse(x, CultureInfo.InvariantCulture),
                    double.Parse(y, CultureInfo.InvariantCulture),
                    double.Parse(z, CultureInfo.InvariantCulture)));
            }
            else if (tokens is ["f", .. var corners] && corners.Length >= 3)
            {
                mesh.AddTriangle(new Triangle(
                    VertexIndex(corners[0]), VertexIndex(corners[1]), VertexIndex(corners[2])));
            }
        }

        return mesh;
    }

    // A face corner is "v", "v/vt", "v/vt/vn" or "v//vn"; keep only the leading
    // vertex index. OBJ indices are 1-based; the mesh stores them 0-based.
    private static int VertexIndex(string faceCorner) =>
        int.Parse(faceCorner.Split('/')[0], CultureInfo.InvariantCulture) - 1;
}
