using Viewer.Core.Geometry;

namespace Viewer.Core.Ports;

// View-facing summary of the loaded model, shown in the info panel. Part of the
// View port's data contract, so it lives here rather than in the app layer.
public readonly record struct ModelInfo(int VertexCount, int TriangleCount, BoundingBox Bounds);
