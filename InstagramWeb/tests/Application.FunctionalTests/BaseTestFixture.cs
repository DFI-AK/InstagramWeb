namespace InstagramWeb.Application.FunctionalTests;

using static InstagramWeb.Application.FunctionalTests.Testing;

[TestFixture]
public abstract class BaseTestFixture
{
    [SetUp]
    public async Task TestSetUp()
    {
        await ResetState();
    }
}
