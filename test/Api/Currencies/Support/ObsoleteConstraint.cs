using NMoneys.Api.Currencies.DataTypes;

using NUnit.Framework.Constraints;

namespace Api.Tests.Currencies.Support;

/// <summary>
/// Constraint that checks if a CurrencySnapshot is obsolete.
/// </summary>
internal class ObsoleteConstraint : Constraint
{
	public override ConstraintResult ApplyTo<TActual>(TActual actual)
	{
		var snapshot = actual as CurrencySnapshot;
		bool isSuccess = snapshot?.IsObsolete == true;
		return new ConstraintResult(this, actual, isSuccess);
	}

	public override string Description => "obsolete currency";
}

internal static partial class CustomConstraintExtensions
{
	/// <summary>
	/// Allows chaining the constraint to Has.Some expression
	/// </summary>
	public static ObsoleteConstraint Obsolete(this ConstraintExpression expression) =>
		expression.Append(new ObsoleteConstraint());
}