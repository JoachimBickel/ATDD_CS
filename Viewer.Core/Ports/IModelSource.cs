namespace Viewer.Core.Ports;

// Outbound port: supplies the raw contents of a model file. Implemented by an
// adapter (e.g. the filesystem); faked in tests so parsing needs no real files.
public interface IModelSource
{
    string Read(string path);
}
