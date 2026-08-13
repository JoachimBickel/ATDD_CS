using System.Windows.Controls;

namespace Viewer.Wpf;

// The 3D viewport. Stage 1: a dark surface; rendering arrives in stage 2,
// mouse interaction in stage 3.
public partial class ViewportControl : UserControl
{
    public ViewportControl()
    {
        InitializeComponent();
    }
}