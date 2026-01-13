using FastEndpoints;

using FluentValidation;

namespace NMoneys.Api.Currencies.Validation;

internal class AmountsFormattingRequestValidator : Validator<AmountsFormattingRequest>
{
	public AmountsFormattingRequestValidator() => _ = RuleFor(x => x.Amounts).NotEmpty();
}