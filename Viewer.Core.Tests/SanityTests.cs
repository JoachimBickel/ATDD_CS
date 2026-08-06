using Xunit;

namespace Viewer.Core.Tests;

// Proves the test harness builds, discovers, and runs before any real tests exist.
public class SanityTests
{
    [Fact]
    public void Harness_works()
    {
        Assert.Equal(2, 1 + 1);
    }
}
