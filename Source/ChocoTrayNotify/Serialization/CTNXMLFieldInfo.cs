using System;
using System.Reflection;
using System.Xml.Linq;
using AlephNote.PluginInterface.Util;

namespace ChocoTrayNotify.Serialization
{
	public class CTNXMLFieldInfo
	{
		public enum SettingObjectTypeEnum
		{
			Integer,
			Double,
			NullableInteger,
			Boolean,
			Guid,
			NGuid,
			String,
			Enum,
		}

		private readonly SettingObjectTypeEnum _objectType;
		public readonly PropertyInfo PropInfo;
		public readonly CTNXMLFieldAttribute Attribute;

		public CTNXMLFieldInfo(SettingObjectTypeEnum t, PropertyInfo i, CTNXMLFieldAttribute a)
		{
			_objectType = t;
			PropInfo = i;
			Attribute = a;
		}

		public XElement Serialize(object objdata, CTNXMLSerializationSettings opt)
		{
			string resultdata;

			switch (_objectType)
			{
				case SettingObjectTypeEnum.Integer:
					resultdata = Convert.ToString((int)objdata);
					break;

				case SettingObjectTypeEnum.NullableInteger:
					var nint = (int?)objdata;
					resultdata = nint == null ? string.Empty : nint.ToString();
					break;

				case SettingObjectTypeEnum.Boolean:
					resultdata = Convert.ToString((bool)objdata);
					break;

				case SettingObjectTypeEnum.Guid:
					resultdata = ((Guid)objdata).ToString("B");
					break;

				case SettingObjectTypeEnum.NGuid:
					resultdata = ((Guid?)objdata)?.ToString("B") ?? "";
					break;

				case SettingObjectTypeEnum.String:
					resultdata = (string)objdata;
					break;

				case SettingObjectTypeEnum.Double:
					resultdata = ((double)objdata).ToString("R");
					break;

				case SettingObjectTypeEnum.Enum:
					resultdata = Convert.ToString(objdata);
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(objdata), _objectType, null);
			}

			return CreateXElem(PropInfo.Name, _objectType, resultdata, opt);
		}

		public void Deserialize(object obj, XElement root, CTNXMLSerializationSettings opt)
		{
			var current = PropInfo.GetValue(obj);

			switch (_objectType)
			{
				case SettingObjectTypeEnum.Integer:
					PropInfo.SetValue(obj, XHelper.GetChildValue(root, PropInfo.Name, (int)current));
					return;

				case SettingObjectTypeEnum.Double:
					PropInfo.SetValue(obj, XHelper.GetChildValue(root, PropInfo.Name, (double)current));
					return;

				case SettingObjectTypeEnum.NullableInteger:
					PropInfo.SetValue(obj, XHelper.GetChildValue(root, PropInfo.Name, (int?)current));
					return;

				case SettingObjectTypeEnum.Boolean:
					PropInfo.SetValue(obj, XHelper.GetChildValue(root, PropInfo.Name, (bool)current));
					return;

				case SettingObjectTypeEnum.Guid:
					PropInfo.SetValue(obj, XHelper.GetChildValue(root, PropInfo.Name, (Guid)current));
					return;

				case SettingObjectTypeEnum.NGuid:
					PropInfo.SetValue(obj, XHelper.GetChildValue(root, PropInfo.Name, (Guid?)current));
					return;

				case SettingObjectTypeEnum.String:
					PropInfo.SetValue(obj, XHelper.GetChildValue(root, PropInfo.Name, (string)current));
					return;

				case SettingObjectTypeEnum.Enum:
					PropInfo.SetValue(obj, XHelper.GetChildValue(root, PropInfo.Name, current, PropInfo.PropertyType));
					break;

				default:
					throw new ArgumentOutOfRangeException(nameof(obj), _objectType, null);
			}
		}

		private XElement CreateXElem(string name, object type, object content, CTNXMLSerializationSettings opt)
		{
			var r = content!= null ? new XElement(name, content) : new XElement(name);
			if (type != null && (opt & CTNXMLSerializationSettings.IncludeTypeInfo) != 0) r.Add(new XAttribute("type", type));
			return r;
		}
	}


}
