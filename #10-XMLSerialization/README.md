# 10 - XML Serialization

*XML* (Extensible Markup Language) is a human-readable and machine-parsable markup language which defines a set of rules for encoding a document. C# has built-in XML serialization with the following noteworthy points:

- Need to import System.Xml.Serialization which adds about 1MB to the final build.
  - There are lightweight readers such as [TinyXML](http://www.grinninglizard.com/tinyxml2/index.html) and [XMLParser](https://forum.unity3d.com/threads/free-lightweight-xml-reader-needs-road-testing.77383/).
- Like JSON, XML can only serialize public fields.
- The serializable class must have a parameterless constructor.

## XML Format

Like JSON strings, XML documents are human-readable and contain:

- Tags, both start-tags <level> and end-tags </level>
- Elements, the contents between two tags, which may contain child elements for instance &lt;enemy>Big Boss&lt;/enemy> or &lt;level>&lt;numberOfLives>3&lt;/numberOfLives>&lt;/level>
- Attributes, name-value pairs contained within the start tag, for instance &lt;level index=0>

## XML Serialization Attributes

**[XmlElement]** indicates that a field will be represented as an XML element. By default all public fields are treated as elements, with their name as the tag, so the only real practical use of [XmlElement] is to change the tag from the default value. 

<table>
  <tr>
    <td><pre><code>public class PlayerData
{
  [XmlElement("n")]
  public string name;
}</code></pre></td>
    <td><pre>&lt;PlayerData>
  &lt;n>Gordon Freeman&lt;/n>
&lt;/PlayerData></pre></td>
  </tr>
</table>

**[XmlAttribute]** indicates that the field will be represented as an XML attribute

<table>
  <tr>
    <td><pre><code>public class PlayerData
{
  [XmlAttribute("name")]
  public string name;
}</code></pre></td>
    <td><pre>&lt;PlayerData name="Gordon Freeman">
&lt;/PlayerData></pre></td>
  </tr>
</table>

**[XmlIgnore]** is used to skip the serialization of a public field.

<table>
  <tr>
    <td><pre><code>public class PlayerData
{
  public string name;
  [XmlIgnore] public int somePublicUnserializedInt;
}</code></pre></td>
    <td><pre>&lt;PlayerData>
    &lt;name>Gordon Freeman&lt;/name>
&lt;/PlayerData></pre></td>
  </tr>
</table>

**[XmlRoot]** is used to alter the document's root tag (which by default is the object's class name)

<table>
  <tr>
    <td><pre><code>[XmlRoot("Player")]
public class PlayerData
{
  public string name;
}</code></pre></td>
    <td><pre>&lt;Player>
    &lt;name>Gordon Freeman&lt;/name>
&lt;/Player></pre></td>
  </tr>
</table>

**[XmlArray]** is used to specify the array's tag, while **[XmlArrayItem]** is used to specify the individual element's tag.

<table>
  <tr>
    <td><pre><code>public class PlayerData
{
  public string name;
  [XmlArray("scores"), XmlArrayItem("score")]
  public int[] levelScores;
}</code></pre></td>
    <td><pre>&lt;PlayerData>
    &lt;name>Gordon Freeman&lt;/name>
    &lt;scores>
        &lt;score>50&lt;/score>
        &lt;score>76&lt;/score>
        &lt;score>19&lt;/score>
    &lt;/scores>
&lt;/PlayerData></pre></td>
  </tr>
</table>

## Saving/Loading Classes/Structs to Disk

Like the [Binary](https://github.com/defuncart/50-unity-tips/tree/master/%2307-BinarySerialization) and [JSON](https://github.com/defuncart/50-unity-tips/tree/master/%2309-JSONSerialization) Serialization tips, given the *PlayerData* class

```C#
[System.Serializable]
public class PlayerData
{
  public string name;
  public int score;
  [System.NonSerialized] private int tempValue;
  private int somePrivateVariable;
}
```

and using an XMLSerializer (which could be better named)

```C#
using System.Xml.Serialization;

public static T Load<T>(string filename) where T: class
{
  string path = PathForFilename(filename);
  if(XMLSerializer.PathExists(path))
  {
    try
    {
      using(Stream stream = File.OpenRead(path))
      {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        return serializer.Deserialize(stream) as T;
      }
    }
    catch(Exception e) { Debug.LogWarning(e.Message); }
  }
  return default(T);
}

public static void Save<T>(string filename, T data) where T: class
{
  string path = PathForFilename(filename);
  using(Stream stream = File.OpenWrite(path))
  {    
    XmlSerializer serializer = new XmlSerializer(typeof(T));
    serializer.Serialize(stream, data);
  }
}
```

we can easily save our data to file and reload it

```C#
string filename = "PlayerData.xml";
PlayerData data;
if(XMLSerializer.FileExists(filename))
{
  data = XMLSerializer.Load<PlayerData>(filename);
}
else
{
  data = new PlayerData();
  XMLSerializer.Save<PlayerData>(filename, data);
}
Debug.Log(data.ToString());
```

As per JSON, *somePrivateVariable* is not serializable and the resulting XML output is human readable and thus potentially player-modifiable. Nevertheless, XML is commonly used for saving data and game states - one approach is to encyrpt the file using [*System.Security.Cryptography*](https://support.microsoft.com/en-us/help/307010/how-to-encrypt-and-decrypt-a-file-by-using-visual-c).

## ToXML, FromXML

Like JSON, we can easily convert an object to an XML string and back again using the following methods:

```C#
public static string ToXML<T>(T data) where T : class
{
  using(StringWriter textWriter = new StringWriter())
  {
    XmlSerializer serializer = new XmlSerializer(data.GetType());
    serializer.Serialize(textWriter, data);
    return textWriter.ToString();
  }
}

public static T FromXML<T>(string xml) where T : class
{
  //load the xml into an xml document to remove the header
  XmlDocument xmlDoc = new XmlDocument(); xmlDoc.LoadXml(xml);
  if(xmlDoc.FirstChild.NodeType == XmlNodeType.XmlDeclaration) { xmlDoc.RemoveChild(xmlDoc.FirstChild); }

  using(Stream stream = ToStream(xmlDoc.InnerXml))
  {
    XmlSerializer serializer = new XmlSerializer(typeof(T));
    return serializer.Deserialize(stream) as T;
  }
}
```

This is something I haven't really tested, but it seems to work :p Could be useful saving an object to PlayerPrefs or pulling objects from RemoteSettings.

## XML vs JSON

One could argue that JSON is more user-readable as it isn't cluttered with tags, which could benefit non-technical people who may need to edit the files. JSON built-in support isn't as encompasing as XML, so, for instance, dictionaries and top-level arrays will need their own custom serializers. XML requires no custom code or formatting, but System.Xml.Serialization will add 1MB to the final build.

## Example

Scriptable Objects are a great way to vary certain parameters during gameplay and then persist to the project. However what if the Game Designer is not comfortable with Unity's interface? Then using a standalone build and an XML database is a one option - an XML file can be easily downloaded at runtime and deserialized into a game object.

## Conclusion

Hopefully this series of five posts on serialization was useful. Next week we'll talk about UI tips, import settings, caching and more!