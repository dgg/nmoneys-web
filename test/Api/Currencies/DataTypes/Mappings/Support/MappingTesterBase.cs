using Mapster;

namespace Api.Tests.Currencies.DataTypes.Mappings.Support;

/// <summary>
/// Base class for mapping tests that sets up the TypeAdapterConfig instance.
/// </summary>
public abstract class MappingTesterBase
{
	protected TypeAdapterConfig Config { get; private set; } = null!;

	[OneTimeSetUp]
	public void SetupMapping()
	{
		Config = new TypeAdapterConfig();
		foreach (IRegister mappings in Mappings)
		{
			mappings.Register(Config);
		}

		// catch errors
		Config.Compile();
	}

	/// <summary>
	/// Derived classes must implement this to provide their specific mapping register.
	/// </summary>
	protected abstract IRegister[] Mappings { get; }
}