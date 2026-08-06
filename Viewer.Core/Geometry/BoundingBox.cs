namespace Viewer.Core.Geometry;

// Axis-aligned bounding box: the minimum and maximum corners of an extent.
public readonly record struct BoundingBox(Vec3 Min, Vec3 Max)
{
    public Vec3 Center => new(
        (Min.X + Max.X) / 2.0,
        (Min.Y + Max.Y) / 2.0,
        (Min.Z + Max.Z) / 2.0);
}
