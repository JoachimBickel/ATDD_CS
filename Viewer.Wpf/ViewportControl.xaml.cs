using System.Windows.Controls;

namespace Viewer.Wpf;

// The 3D viewport: renders whatever the view model exposes — bound camera,
// bound headlight, bound flat-shaded geometry. Mouse interaction in stage 3.
public partial class ViewportControl : UserControl
{
    public ViewportControl()
    {
        InitializeComponent();
    }
}