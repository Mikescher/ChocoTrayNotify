namespace ChocoTrayNotify.Serialization
{
	public interface ICTNSerializable
	{
		void OnBeforeSerialize();
		void OnAfterDeserialize();
	}
}
