using FastEndpoints;

namespace NMoneys.Api.Currencies;

internal record struct FormatAmounts(CurrencyIsoCode Code, decimal[] Amounts) : ICommand<AmountsFormattingResponse>;

internal class FormattingHandler : ICommandHandler<FormatAmounts, AmountsFormattingResponse>
{
	public Task<AmountsFormattingResponse> ExecuteAsync(FormatAmounts command, CancellationToken ct)
	{
		Currency currency = Currency.Get(command.Code);

		var formatted = command.Amounts
			.Select(amount => new Money(amount, currency))
			.Select(m => m.ToString())
			.ToArray();

		return Task.FromResult(new AmountsFormattingResponse(formatted));
	}
}
