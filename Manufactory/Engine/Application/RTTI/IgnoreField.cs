using System;

namespace MeteorEngine
{
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
	public class IgnoreField : Attribute
	{

	}
}