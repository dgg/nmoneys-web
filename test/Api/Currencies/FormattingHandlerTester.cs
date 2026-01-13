using NMoneys;
using NMoneys.Api.Currencies;

using Subject = NMoneys.Api.Currencies.FormattingHandler;

namespace Api.Tests.Currencies;

[TestFixture]
public class FormattingHandlerTester
{
	[Test]
	public async Task ExecuteAsync_SingleAmount_ReturnsFormattedString()
	{
		decimal[] single = [-123.456m];
		var command = new FormatAmounts(CurrencyIsoCode.USD, single);

		AmountsFormattingResponse result = await new Subject().ExecuteAsync(command, CancellationToken.None);

		Assert.That(result.Formatted, Has.Length.EqualTo(1));
		Assert.That(result.Formatted[0], Is.EqualTo("-$123.46"), "round-up");
	}

	[Test]
	public async Task ExecuteAsync_MultipleAmounts_ReturnsAllFormatted()
	{
		decimal[] multiple = [10.5m, 99.99m, 1234.563m];
		var command = new FormatAmounts(CurrencyIsoCode.EUR, multiple);

		AmountsFormattingResponse response = await new Subject().ExecuteAsync(command, CancellationToken.None);

		Assert.That(response.Formatted, Has.Length.EqualTo(3));
		Assert.Multiple(() =>
		{
			Assert.That(response.Formatted[0], Is.EqualTo("10,50 €"));
			Assert.That(response.Formatted[1], Is.EqualTo("99,99 €"));
			Assert.That(response.Formatted[2], Is.EqualTo("1.234,56 €"));
		});
	}

	[Test]
	public async Task ExecuteAsync_CurrencyWithNoDecimalDigits_FormatsCorrectly()
	{
		decimal[] amounts = [1000m, 1500.50m];
		var command = new FormatAmounts(CurrencyIsoCode.JPY, amounts);

		var response = await new Subject().ExecuteAsync(command, CancellationToken.None);

		Assert.That(response.Formatted, Has.Length.EqualTo(2));
		Assert.Multiple(() =>
		{
			Assert.That(response.Formatted[0], Is.EqualTo("¥1,000"));
			Assert.That(response.Formatted[1], Is.EqualTo("¥1,501"));
		});
	}

	[Test]
	public async Task ExecuteAsync_EmptyAmounts_NoException()
	{
		decimal[] empty = [];
		var command = new FormatAmounts(CurrencyIsoCode.CHF, empty);

		await Assert.ThatAsync(
			() => new Subject().ExecuteAsync(command, CancellationToken.None),
			Throws.Nothing);
	}

	[Test]
	public async Task ExecuteAsync_UndefinedCurrency_Exception()
	{
		CurrencyIsoCode undefined = (CurrencyIsoCode)1000;
		decimal[] amounts = [100m];
		var command = new FormatAmounts(undefined, amounts);

		await Assert.ThatAsync(
			() => new Subject().ExecuteAsync(command, CancellationToken.None),
			Throws.InstanceOf<UndefinedCodeException>());
	}
}