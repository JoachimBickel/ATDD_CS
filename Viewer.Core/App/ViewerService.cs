using Viewer.Core.IO;
using Viewer.Core.Ports;

namespace Viewer.Core.App;

// Inbound port / use case: the actions the UI can drive. Reads a model via the
// IModelSource port, parses it, and presents it through the IView port.
public class ViewerService(IModelSource source, IView view)
{
    private readonly ObjImporter _importer = new();

    public void OpenModel(string path)
    {
        var content = source.Read(path);
        var model = _importer.Parse(content);

        view.ShowModel(model);
        view.ShowModelInfo(Presentation.DescribeModel(model));
    }
}
