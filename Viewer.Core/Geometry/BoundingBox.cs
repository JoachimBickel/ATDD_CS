namespace Viewer.Core.Geometry;

// Axis-aligned bounding box: the minimum and maximum corners of an extent.
public readonly record struct BoundingBox(Vec3 Min, Vec3 Max);
