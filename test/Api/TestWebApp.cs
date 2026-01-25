using System.IdentityModel.Tokens.Jwt;

using Alba;
using Alba.Security;

using Api.Tests.Support.Auth;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Tests;

[SetUpFixture]
public class TestWebApp
{
	public static IAlbaHost Host { get; private set; } = null!;

	[OneTimeSetUp]
	public async Task Setup()
	{
		string audience = "urn:nmoneys:test:api";
		string domain = "test.auth0.com";
		string issuer = $"https://{domain}/";

		var authStub = new JwtSecurityStub()
			.With(JwtRegisteredClaimNames.Aud, audience)
			.With(JwtRegisteredClaimNames.Iss, issuer);

		var configuration = new Dictionary<string, string?>
		{
			{ "Authn:Domain", domain }, { "Authn:Audience", audience }
		};

		Host = await AlbaHost.For<Program>(
			app =>
			{
				app.UseEnvironment("Testing");
			},
			authStub,
			ConfigurationOverride.Create(configuration));
	}

	[OneTimeTearDown]
	public void Teardown() => Host.Dispose();
}