using FastEndpoints;
using FluentValidation;

namespace NMoneys.Api.Amounts.Validation;

internal class AmountsFormattingRequestValidator : Validator<AmountsFormattingRequest>
{
	public AmountsFormattingRequestValidator() => _ = RuleFor(x => x.Amounts).NotEmpty();
}