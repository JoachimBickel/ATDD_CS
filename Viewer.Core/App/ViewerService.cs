using Viewer.Core.IO;
using Viewer.Core.Ports;

namespace Viewer.Core.App;

// Inbound port / use case: the actions the UI can drive. Reads a model via the
// IModelSource port, parses it, and presents it through the IView port. Holds
// the current camera, framed on open and mutated by interaction commands.
public class ViewerService(IModelSource source, IView view)
{
    private readonly ObjImporter _importer = new();
    private CameraState _camera;

    public void OpenModel(string path)
    {
        var content = source.Read(path);
        var model = _importer.Parse(content);

        _camera = CameraControl.FrameModel(model);

        view.ShowModel(model);
        view.ShowModelInfo(Presentation.DescribeModel(model));
        view.ShowCamera(_camera);
    }

    public void Zoom(double factor)
    {
        _camera = CameraControl.Zoom(_camera, factor);
        view.ShowCamera(_camera);
    }
}
