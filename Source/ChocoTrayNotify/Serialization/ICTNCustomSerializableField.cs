using System.Xml.Linq;

namespace ChocoTrayNotify.Serialization
{
	public interface ICTNCustomSerializableField
	{
		object GetTypeStr();
		void Serialize(XElement target, CTNXMLSerializationSettings opt);
		object DeserializeNew(XElement source, CTNXMLSerializationSettings opt);
	}
}
