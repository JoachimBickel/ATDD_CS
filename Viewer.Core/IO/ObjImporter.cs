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
            else if (tokens is ["f", var a, var b, var c])
            {
                // OBJ indices are 1-based; the mesh stores them 0-based.
                mesh.AddTriangle(new Triangle(
                    int.Parse(a, CultureInfo.InvariantCulture) - 1,
                    int.Parse(b, CultureInfo.InvariantCulture) - 1,
                    int.Parse(c, CultureInfo.InvariantCulture) - 1));
            }
        }

        return mesh;
    }
}
