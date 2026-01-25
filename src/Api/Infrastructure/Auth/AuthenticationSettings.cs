using System.ComponentModel.DataAnnotations;

using Microsoft.Extensions.Options;

namespace NMoneys.Api.Infrastructure.Auth;

public record AuthenticationSettings
{
    public const string SectionName = "Authn";

    /// <summary>
    /// Auth0 domain
    /// </summary>
    [Required, MinLength(1)] public required string Domain { get; init; } = string.Empty;

    /// <summary>
    /// The API identifier/audience
    /// </summary>
    [Required, MinLength(1)] public required string Audience { get; init; } = string.Empty;

    /// <summary>
    /// Settings for interacting with the management API to provision applications
    /// </summary>
    [Required, ValidateObjectMembers] public required ApplicationsManagerSettings ApplicationsManager { get; init; }
}

public record ApplicationsManagerSettings
{
	[Required, MinLength(1)] public required string ClientId { get; init; } = string.Empty; 
	[Required, MinLength(1)] public required string ClientSecret { get; init; } = string.Empty;
}
