using Viewer.Core.Geometry;

namespace Viewer.Core.Ports;

// Outbound port: how the core asks the UI to display things. Implemented by the
// UI adapter; faked in tests. Speaks only core/domain types (never UI types).
public interface IView
{
    void ShowModel(Mesh mesh);
    void ShowModelInfo(ModelInfo info);
}
