namespace Viewer.Core.Geometry;

// Axis-aligned bounding box: the minimum and maximum corners of an extent.
public readonly record struct BoundingBox(Vec3 Min, Vec3 Max)
{
    public Vec3 Center => new(
        (Min.X + Max.X) / 2.0,
        (Min.Y + Max.Y) / 2.0,
        (Min.Z + Max.Z) / 2.0);

    // Length of the min -> max space diagonal.
    public double Diagonal
    {
        get
        {
            var dx = Max.X - Min.X;
            var dy = Max.Y - Min.Y;
            var dz = Max.Z - Min.Z;
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
    }
}
