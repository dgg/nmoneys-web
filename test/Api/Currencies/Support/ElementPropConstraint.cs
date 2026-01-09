using System.Text.Json;

using NUnit.Framework.Constraints;

namespace Api.Tests.Currencies.Support;

/// <summary>
/// Constraint that checks if a JsonElement has a specific property.
/// </summary>
internal class ElementPropConstraint : Constraint
{
	private readonly string _propertyName;

	public ElementPropConstraint(string propertyName)
	{
		_propertyName = propertyName;
	}

	public override ConstraintResult ApplyTo<TActual>(TActual actual)
	{
		if (actual is not JsonElement element)
		{
			return new ConstraintResult(this, actual, false);
		}

		bool hasProperty = element.TryGetProperty(_propertyName, out _);
		return new ConstraintResult(this, actual, hasProperty);
	}

	public override string Description => $"JsonElement.{_propertyName}";
}

internal partial class Haz : Has
{
	public static ElementPropConstraint Prop(string propertyName) => new (propertyName);
}
