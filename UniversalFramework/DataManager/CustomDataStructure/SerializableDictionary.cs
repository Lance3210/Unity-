using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
/// <summary>
/// ��XML���л��뷴���л���Dictionary
/// </summary>
/// <typeparam name="K">��</typeparam>
/// <typeparam name="V">ֵ</typeparam>
public class SerializableDictionary<K, V> : Dictionary<K, V>, IXmlSerializable
{
	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		XmlSerializer keyStr = new XmlSerializer(typeof(K));
		XmlSerializer valueStr = new XmlSerializer(typeof(V));
		reader.Read();
		while (reader.NodeType != XmlNodeType.EndElement)
		{
			K key = (K)keyStr.Deserialize(reader);
			V value = (V)valueStr.Deserialize(reader);
			Add(key, value);
		}
	}

	public void WriteXml(XmlWriter writer)
	{
		XmlSerializer keyStr = new XmlSerializer(typeof(K));
		XmlSerializer valueStr = new XmlSerializer(typeof(V));
		foreach (KeyValuePair<K, V> item in this)
		{
			keyStr.Serialize(writer, item.Key);
			valueStr.Serialize(writer, item.Value);
		}
	}
}
