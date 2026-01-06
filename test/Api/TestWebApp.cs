using Alba;

using Microsoft.AspNetCore.Hosting;

namespace Api.Tests;

[SetUpFixture]
public class TestWebApp
{
	public static IAlbaHost Host { get; private set; }

	[OneTimeSetUp]
	public async Task Setup()
	{
		Host = await AlbaHost.For<Program>(app =>
		{
			app.UseEnvironment("Testing");
		});
	}

	[OneTimeTearDown]
	public void Teardown() => Host.Dispose();
}