using FastEndpoints;

using Mapster;

using NMoneys.Api.Currencies.DataTypes;

namespace NMoneys.Api.Currencies;

internal record struct ListCurrencies : ICommand<CurrenciesListingResponse>;

internal class ListingHandler : ICommandHandler<ListCurrencies, CurrenciesListingResponse>
{
	public Task<CurrenciesListingResponse> ExecuteAsync(ListCurrencies command, CancellationToken ct)
	{
		var snapshots = Currency.FindAll()
			.OrderBy(c => c.AlphabeticCode, StringComparer.Ordinal)
			.Adapt<CurrencySnapshot[]>();
		
		return Task.FromResult(new CurrenciesListingResponse(snapshots));
	}
}