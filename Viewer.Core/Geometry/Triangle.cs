namespace Viewer.Core.Geometry;

// Three vertex indices forming a triangular face.
public readonly record struct Triangle(int V0, int V1, int V2);
