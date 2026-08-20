namespace Viewer.Core.Geometry;

// A point or direction in 3D space. Plain value type — no UI, no GPU concerns.
public readonly record struct Vec3(double X, double Y, double Z)
{
    public static Vec3 operator +(Vec3 a, Vec3 b) => new(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
    public static Vec3 operator -(Vec3 a, Vec3 b) => new(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
    public static Vec3 operator *(Vec3 v, double scalar) => new(v.X * scalar, v.Y * scalar, v.Z * scalar);
}
