
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using Alba;

using NMoneys.Api.Infrastructure.Auth;


namespace Api.Tests.Support.Auth;

internal static class ScenarioExtensions
{
	public static void Anonymous(this Scenario s) => s.RemoveRequestHeader("Authorization");

	public static void WithoutScope(this Scenario s) => s.RemoveClaim("scope");

	public static void WithScopes(this Scenario s, params AuthorizationScope[] scopes) =>
		s.WithClaim(new Claim("scope", string.Join(' ', scopes.Select(scope => scope.Value))));
}