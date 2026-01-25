using System.Runtime.CompilerServices;

using Ardalis.SmartEnum;

namespace NMoneys.Api.Infrastructure.Auth;

internal sealed class AuthorizationScope : SmartEnum<AuthorizationScope, string> 
{
	public static readonly AuthorizationScope ReadCurrencies = new ("read:currencies");
	public static readonly AuthorizationScope FormatAmounts = new ("format:amounts");
	
	private AuthorizationScope(string scope, [CallerMemberName]string name = "") : base(name, scope) { }
}