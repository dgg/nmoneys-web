using Lamar;

namespace NMoneys.Api.Infrastructure;

public class CurrencyInitRegistry : ServiceRegistry
{
	public CurrencyInitRegistry() => Currency.InitializeAllCurrencies();
}