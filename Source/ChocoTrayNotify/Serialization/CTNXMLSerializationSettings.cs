using System;

namespace ChocoTrayNotify.Serialization
{
	[Flags]
	public enum CTNXMLSerializationSettings
	{
		None            = 0x00,
		FormattedOutput = 0x01,
		IncludeTypeInfo = 0x02,
	}
}
