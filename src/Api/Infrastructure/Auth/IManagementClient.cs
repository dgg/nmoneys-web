using System.Net.Mail;

using NMoneys.Api.Infrastructure.Auth.Models;

namespace NMoneys.Api.Infrastructure.Auth;

public interface IManagementClient
{
	Task<ApiClient> CreateApiClient(MailAddress email, CancellationToken ct);
	Task GrantApiClient(string clientId, CancellationToken ct);
}
