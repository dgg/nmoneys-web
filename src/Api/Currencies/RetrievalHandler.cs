using FastEndpoints;

using Mapster;

using NMoneys.Api.Currencies.DataTypes;

namespace NMoneys.Api.Currencies;

internal record struct RetrieveCurrency(CurrencyIsoCode Code) : ICommand<CurrencyRetrievalResponse>;

internal class RetrievalHandler : ICommandHandler<RetrieveCurrency, CurrencyRetrievalResponse>
{
	public Task<CurrencyRetrievalResponse> ExecuteAsync(RetrieveCurrency command, CancellationToken ct)
	{
		Currency currency = Currency.Get(command.Code);
		var detail = currency.Adapt<CurrencyDetail>();
		return Task.FromResult(new CurrencyRetrievalResponse(detail));
	}
}
