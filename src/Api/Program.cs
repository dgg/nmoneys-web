using NMoneys;
using NMoneys.Api.Currencies.DataTypes;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () =>
{
	var snapshots = Currency.FindAll()
		.OrderBy(c => c.AlphabeticCode, StringComparer.Ordinal)
		.Take(5)
		.Select(c => new CurrencySnapshot(c.AlphabeticCode, c.EnglishName, c.NativeName))
		.ToArray();
	return snapshots;
});

app.Run();