namespace Viewer.Core.Geometry;

// A point or direction in 3D space. Plain value type — no UI, no GPU concerns.
public readonly record struct Vec3(double X, double Y, double Z);
