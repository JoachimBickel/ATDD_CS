using System.IO;
using Viewer.Core.Ports;

namespace Viewer.Wpf;

// Filesystem adapter for the IModelSource port: reads model files from disk.
public class FileModelSource : IModelSource
{
    public string Read(string path) => File.ReadAllText(path);
}
