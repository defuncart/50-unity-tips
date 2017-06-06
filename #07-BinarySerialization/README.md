# 07 - Binary Serialization

*Serialization* is the process of converting an object (or an entire graph of connected objects) into a stream of bytes so that it can be recreated again when needed (deserialization) [[MSDN](https://msdn.microsoft.com/en-us/library/7ay27kt9(v=vs.110).aspx)]. In Unity there are four predominate ways to serialize objects:
1. Binary Format
2. Scriptable Objects
3. JSON
4. XML

As you might guess from the name, *Binary Serialization* serializes and deserializes an object into binary format. There are a few points to consider:

- Any serializable Unity Object or class is automatically compatible.
  - Non serializable classes like *Vector*, *Color* etc. aren't compatible. One could instead store the individual values as an array and write helper methods, or investigate [ISerializationSurrogate](https://msdn.microsoft.com/en-us/library/system.runtime.serialization.surrogateselector).
- The resulting data is binary and thus not human readable. 
  - This works well for something that should be secure like game saves, but not anything that your team may need to edit like level variables, enemy properties etc.
- One must code the process from scratch. 
  - This isn't difficult and allows maximum versatility, but may seen  more intimidating than JSON or XML.
 
BinaryFormatter allows us to serialize/deserialze a class to binary

```C#

public static void Save<T>(string path, T data) where T: class
{
  using(Stream stream = File.OpenWrite(path))
  {    
    BinaryFormatter formatter = new BinaryFormatter();
    formatter.Serialize(stream, data);
  }
}

public static T Load<T>(string path) where T: class
{
  if(File.Exists(path))
  {
    try
    {
      using(Stream stream = File.OpenRead(path))
      {
        BinaryFormatter formatter = new BinaryFormatter();
        return formatter.Deserialize(stream) as T;
      }
    }
    catch(Exception e) { Debug.LogWarning(e.Message); }
  }
  return default(T);
}
```

Thus, given a class of player variables

```C#
[System.Serializable]
public class PlayerData
{
  public string name { get; private set; }
  public int score { get; private set; }
  [System.NonSerialized] private int tempValue;
}
```

we can easily serialize/deserialze this class to/from disk:

```C#
string path = "PlayerData.sav";
PlayerData data;
if(File.Exists(path))
{
	data = Serializer.Load<PlayerData>(path);
}
else
{
	data = new PlayerData();
	Serializer.Save<PlayerData>(path, data);
}
Debug.Log(data.ToString());
```

In summary,
- Any class marked *Serializable* can be serialized to disk. 
- Public, private and protected members will be saved. If you don't want a certain property to be saved, mark it as *NonSerialized*.
- If the implementation of a class changes, then loading may fail. New properties can be appended to the class and saved, but the alteration of existing properties will stop the BinaryFormatter from loading the class. Maybe not a big deal when developing, but for a shipped product...