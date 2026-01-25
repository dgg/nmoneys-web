using System.Security.Claims;

using Lamar;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace NMoneys.Api.Infrastructure.Auth;

public class AuthenticationRegistry : ServiceRegistry
{
	public AuthenticationRegistry()
	{
		bindConfiguration();
		addOAuthClient();
		addManagementClient();
		addJwtAuthentication();

		// Use AddSingleton to register IConfigureOptions with access to Auth0Settings
		this.AddSingleton<IConfigureOptions<JwtBearerOptions>>(buildOptions);
	}

	private static ConfigureNamedOptions<JwtBearerOptions> buildOptions(IServiceProvider sp)
	{
		var settings = sp.GetRequiredService<IOptions<AuthenticationSettings>>().Value;

		return new ConfigureNamedOptions<JwtBearerOptions>(
			JwtBearerDefaults.AuthenticationScheme,
			options => configureJwtOptions(options, settings));
	}

	private static void configureJwtOptions(JwtBearerOptions options, AuthenticationSettings settings)
	{
		string authority = new UriBuilder(Uri.UriSchemeHttps, settings.Domain).Uri.ToString();

		// Authority URL (where JWKS endpoint is located)
		options.Authority = authority;

		// Expected audience in JWT tokens
		options.Audience = settings.Audience;

		// Token validation settings
		options.TokenValidationParameters = validationParameters(authority, settings.Audience);

		// Event handlers for debugging (optional - remove in production)
		options.Events = new() { OnAuthenticationFailed = onAuthenticationFailed, OnTokenValidated = onTokenValidated };
	}

	private static TokenValidationParameters validationParameters(string authority, string audience) => new()
	{
		ValidateIssuer = true,
		ValidIssuer = authority,
		ValidateAudience = true,
		ValidAudience = audience,
		ValidateLifetime = true,
		ClockSkew = TimeSpan.Zero, // No tolerance for expired tokens

		// JWKS validation (asymmetric RS256)
		ValidateIssuerSigningKey = true,
		// IssuerSigningKey is automatically fetched from JWKS endpoint

		// Map Auth0's "sub" claim to NameIdentifier
		NameClaimType = ClaimTypes.NameIdentifier,
		RoleClaimType = ClaimTypes.Role
	};

	private static Task onAuthenticationFailed(AuthenticationFailedContext context)
	{
		var logger = context.HttpContext.RequestServices
			.GetRequiredService<ILogger<AuthenticationRegistry>>();
		logger.LogWarning(context.Exception,
			"JWT authentication failed: {Error}",
			context.Exception.Message);
		return Task.CompletedTask;
	}

	private static Task onTokenValidated(TokenValidatedContext context)
	{
		var logger = context.HttpContext.RequestServices
			.GetRequiredService<ILogger<AuthenticationRegistry>>();
		var clientId = context.Principal?.FindFirst("client_id")?.Value
			?? context.Principal?.FindFirst("sub")?.Value
			?? "unknown";
		logger.LogDebug("JWT token validated for client: {ClientId}", clientId);
		return Task.CompletedTask;
	}

	private void addJwtAuthentication()
	{
		this
			.AddAuthorization()
			.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer();
	}

	/// <summary>
	/// Configure service to create authenticable api clients
	/// </summary>
	private void addManagementClient()
	{
		this
			// to cache access tokens to the management service
			.AddMemoryCache()
			// transient, but instantiation controlled by .SetHandlerLifetime()
			.AddTransient<ManagementHttpHandler>()
			// client for the management service (provision API client authentication)
			.AddHttpClient<IManagementClient, ManagementClient>((sp, http) =>
			{
				var settings = sp.GetRequiredService<IOptions<AuthenticationSettings>>().Value;
				http.BaseAddress = ManagementClient.BaseUrl(settings.Domain);
			})
			// to add authentication to the management service requests
			.AddHttpMessageHandler<ManagementHttpHandler>()
			.SetHandlerLifetime(TimeSpan.FromMinutes(5));
	}

	/// <summary>
	/// Register supporting client to fetch access tokens
	/// </summary>
	private void addOAuthClient() =>
		this.AddHttpClient<IOAuthClient, OAuthClient>((sp, http) =>
		{
			var settings = sp.GetRequiredService<IOptions<AuthenticationSettings>>().Value;
			http.BaseAddress = new UriBuilder(Uri.UriSchemeHttps, settings.Domain) { Path = "/oauth/" }.Uri;
		});

	/// <summary>
	/// Configure authn settings from appsettings.json
	/// </summary>
	private void bindConfiguration() =>
		this.AddOptions<AuthenticationSettings>()
			.BindConfiguration(AuthenticationSettings.SectionName)
			.ValidateDataAnnotations()
			.ValidateOnStart();
}