using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
/// <summary>
/// ��XML���л��뷴���л���Dictionary
/// </summary>
/// <typeparam name="TKey">��</typeparam>
/// <typeparam name="TValue">ֵ</typeparam>
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
{
    public XmlSchema GetSchema()
    {
        return null;
    }

    public void ReadXml(XmlReader reader)
    {
        XmlSerializer keyStr = new XmlSerializer(typeof(TKey));
        XmlSerializer valueStr = new XmlSerializer(typeof(TValue));
        reader.Read();
        while(reader.NodeType != XmlNodeType.EndElement)
        {
            TKey key = (TKey)keyStr.Deserialize(reader);
            TValue value = (TValue)valueStr.Deserialize(reader);
            this.Add(key, value);
        }
    }

    public void WriteXml(XmlWriter writer)
    {
        XmlSerializer keyStr = new XmlSerializer(typeof(TKey));
        XmlSerializer valueStr = new XmlSerializer(typeof(TValue));
        foreach (KeyValuePair<TKey,TValue> item in this)
        {
            keyStr.Serialize(writer,item.Key);
            valueStr.Serialize(writer,item.Value);
        }
    }
}
