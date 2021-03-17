using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngineInternal;
/// <summary>
/// Хранит тип фраз конкретного юнита
/// </summary>
public class UnitTexts : Object
{
    TextAsset text;
    private XmlDocument xml;
    public UnitTexts(string name)
    {
        
        text = Resources.Load(string.Format("DialoguePhrases/{0}",name)) as TextAsset;
        xml = new XmlDocument();
        xml.LoadXml(text.text);     
    }

    ~UnitTexts()
    {
        Destroy(text);
    }

    public List<string> GetNamesKey(string TypeDialoge)
    {
        var strings = new Hashtable();
        var element = (XmlElement)xml.DocumentElement[TypeDialoge];
        List<string> list=new List<string>();
        if (element != null)
        {
            var elemEnum = (IEnumerator)element.GetEnumerator();
            while (elemEnum.MoveNext())
            {
                var xmlItem = (XmlElement)elemEnum.Current;
                list.Add(xmlItem.Name);
            }
            return list;
        }

        return null;
    }
    

}
