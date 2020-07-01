namespace ChocoTrayNotify.Serialization
{
	public interface ICTNCustomFlatSerializableField
	{
		object GetTypeStr();
		string Serialize(CTNXMLSerializationSettings opt);
		object DeserializeNew(string source, CTNXMLSerializationSettings opt);
	}
}
