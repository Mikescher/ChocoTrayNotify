using AlephNote.PluginInterface.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace ChocoTrayNotify.Serialization
{
	public class CTNXMLSerializer<T> where T : ICTNSerializable
	{
		private class AttrObj { public PropertyInfo Info; public List<object> Attributes; }

		public const CTNXMLSerializationSettings DEFAULT_SERIALIZATION_SETTINGS =
			CTNXMLSerializationSettings.FormattedOutput |
			CTNXMLSerializationSettings.IncludeTypeInfo;

		private readonly string _rootNode;
		private readonly List<CTNXMLFieldInfo> _fields;

		// ReSharper disable once RedundantEnumerableCastCall
		public CTNXMLSerializer(string rootName)
		{
			_rootNode = rootName;

			_fields = typeof(T)
				.GetProperties()
				.Select(p => new AttrObj { Info = p, Attributes = p.GetCustomAttributes(typeof(CTNXMLFieldAttribute), false).Cast<object>().ToList() })
				.Where(p => p.Attributes.Count == 1)
				.Select(CreateFieldInfo)
				.ToList();
		}

		private CTNXMLFieldInfo CreateFieldInfo(AttrObj p)
		{
			var attr = (CTNXMLFieldAttribute)p.Attributes.Single();
			var type = GetSettingType(p.Info);

			return new CTNXMLFieldInfo(type, p.Info, attr);
		}

		private CTNXMLFieldInfo.SettingObjectTypeEnum GetSettingType(PropertyInfo prop)
		{
			if (prop.PropertyType == typeof(int))       return CTNXMLFieldInfo.SettingObjectTypeEnum.Integer;
			if (prop.PropertyType == typeof(double))    return CTNXMLFieldInfo.SettingObjectTypeEnum.Double;
			if (prop.PropertyType == typeof(int?))      return CTNXMLFieldInfo.SettingObjectTypeEnum.NullableInteger;
			if (prop.PropertyType == typeof(string))    return CTNXMLFieldInfo.SettingObjectTypeEnum.String;
			if (prop.PropertyType == typeof(bool))      return CTNXMLFieldInfo.SettingObjectTypeEnum.Boolean;
			if (prop.PropertyType == typeof(Guid))      return CTNXMLFieldInfo.SettingObjectTypeEnum.Guid;
			if (prop.PropertyType == typeof(Guid?))     return CTNXMLFieldInfo.SettingObjectTypeEnum.NGuid;
			if (prop.PropertyType.GetTypeInfo().IsEnum) return CTNXMLFieldInfo.SettingObjectTypeEnum.Enum;

			throw new NotSupportedException("Setting of type " + prop.PropertyType + " not supported");
		}

		public string Serialize(T obj, CTNXMLSerializationSettings opt)
		{
			obj.OnBeforeSerialize();

			var root = new XElement(_rootNode);

			foreach (var prop in _fields)
			{
				var data = prop.PropInfo.GetValue(obj);

				root.Add(prop.Serialize(data, opt));
			}

			if ((opt & CTNXMLSerializationSettings.FormattedOutput) != 0)
				return XHelper.ConvertToStringFormatted(new XDocument(root));
			else
				return XHelper.ConvertToStringRaw(new XDocument(root));
		}

		public void Deserialize(T obj, string xml, CTNXMLSerializationSettings opt)
		{
			var xd = XDocument.Parse(xml);
			var root = xd.Root;
			if (root == null) throw new Exception("XDocument needs root");

			foreach (var prop in _fields)
			{
				prop.Deserialize(obj, root, opt);
			}

			obj.OnAfterDeserialize();
		}

		public void Clone(T source, T target)
		{
			var opt = CTNXMLSerializationSettings.None;

			var xml = Serialize(source, opt);
			Deserialize(target, xml, opt);
		}

		public bool IsEqual(T a, T b)
		{
			var opt = CTNXMLSerializationSettings.None;

			var xml1 = Serialize(a, opt);
			var xml2 = Serialize(b, opt);

			return xml1 == xml2;
		}

		public IEnumerable<CTNXMLFieldInfo> Diff(CTNSettings as1, CTNSettings as2)
		{
			foreach (var prop in _fields)
			{
				var data1 = prop.PropInfo.GetValue(as1);
				var data2 = prop.PropInfo.GetValue(as2);

				var xml1 = XHelper.ConvertToStringRaw(prop.Serialize(data1, CTNXMLSerializationSettings.None));
				var xml2 = XHelper.ConvertToStringRaw(prop.Serialize(data2, CTNXMLSerializationSettings.None));

				if (xml1 != xml2) yield return prop;
			}
		}
	}
}
