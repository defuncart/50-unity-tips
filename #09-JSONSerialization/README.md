# 09 - JSON Serialization

JSON is a human-readable and machine-parsable lightweight data-interchange format which serializes data objects as text strings. Unity's *JSONUtility* supports serializable classes but is somewhat limited in that **1)** it does not support dictionaries or top level arrays, and **2)** all properties must be public. 

Today we will discuss a couple of potential uses of JSON within a typical Unity Project:
- saving/loading classes/structs to disk
- saving/loading arrays to PlayerPrefs or pulling arrays from RemoteSettings
- deserialization of a database
- parsing web requests

## Saving and Loading Classes/Structs to Disk

Considering our *PlayerData* class 

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

and a simple JSONSerializer

```C#
public class JSONSerializer
{
  public static T Load<T>(string filename) where T: class
  {
    string path = PathForFilename(filename);
    if(JSONSerializer.PathExists(path))
    {
      return JsonUtility.FromJson<T>(File.ReadAllText(path));
    }
    return default(T);
  }
}

public static void Save<T>(string filename, T data) where T: class
{
  string path = PathForFilename(filename);
  File.WriteAllText(path, JsonUtility.ToJson(data));
}
```

we could easily save our data to file and reload it as per the [BinarySerializer tip](https://github.com/defuncart/50-unity-tips/tree/master/%2307-BinarySerialization)

```C#
string filename = "PlayerData.json";
PlayerData data;
if(JSONSerializer.FileExists(filename))
{
  data = JSONSerializer.Load<PlayerData>(filename);
}
else
{
  data = new PlayerData();
  JSONSerializer.Save<PlayerData>(filename, data);
}
Debug.Log(data.ToString());
```

As mentioned, JSON strings are human readable - for instance our PlayerData would simply be ```{"name":"Player", "score":100}``` - and thus without encryption, these strings would be player modifiable (if discovered). Moreover, as only public fields can be serialized, *somePrivateVariable* will thus always have a default value from the constructor - if one actually needs to serialize this variable, then Binary Serialization would be a better solution.

## Serializing Dictionaries and Top-Level Arrays

*JSONUtility* does not support top-level arrays, but if the array is stored in an object

```json
{ "array": [ 0, 1, 2, 3 ] }
```

and a wrapper class is written to handle serialization

```C#
[Serializable]
public class TopLevelIntArray
{
   public int[] array;
}
```

then the array can be easily serialized and deserialized:

```C#
public string ToJson(int[] array)
{
  TopLevelIntArray topLevelArray = new TopLevelIntArray() { array = array };
  return JsonUtility.ToJson(topLevelArray);
}

public static int[] FromJson(string json)
{
  TopLevelIntArray topLevel = JsonUtility.FromJson<TopLevelIntArray>(json);
  return topLevel.array;
}
```

Similarly for dictionaries, we can define a custom object

```C#
[System.Serializable]
public class StringStringDictionary
{
  public string key;
  public string value;
}
```

and an additional object containing an array of our custom object
   
```C#
[System.Serializable]
private class StringStringDictionaryArray
{
  public StringStringDictionary[] items;
}
```
   
so that such an array can be natively serialized as a custom object and then converted into the required dictionary:

```C#
private static string ToJson(Dictionary<string, string> dictionary)
{
  List<StringStringDictionary> dictionaryItemsList = new List<StringStringDictionary>();
  foreach(KeyValuePair<string, string> kvp in dictionary)
  {
    dictionaryItemsList.Add( new StringStringDictionary(){ key = kvp.Key, value = kvp.Value } );
  }

  StringStringDictionaryArray dictionaryArray = new StringStringDictionaryArray(){ items = dictionaryItemsList.ToArray() };
  return JsonUtility.ToJson(dictionaryArray);
}

private static Dictionary<string, string> FromJson(string json)
{
  StringStringDictionaryArray loadedData = JsonUtility.FromJson<StringStringDictionaryArray>(json);
  Dictionary<string, string> dictionary = new Dictionary<string, string>();
  for(int i=0; i < loadedData.items.Length; i++)
  {
    dictionary.Add(loadedData.items[i].key, loadedData.items[i].value);
  }
  return dictionary;
}
```

By using *Generics* and *typeof* we can utilize *ToJson*, *FromJson* to call individual serializers/deserializers for a given type. See included code for details.

## Saving/loading arrays to/from PlayerPrefs, RemoteSettings

By default *PlayerPrefs* does not support the saving/loading of arrays or dictionaries, but it does support strings, and using the above mentioned *JSONSerializer*, one could convert, for example, an array to JSON and then save to/load from PlayerPrefs.

```C#
int[] myArray = new int[]{ 0, 1, 2, 3 };
PlayerPrefs.SetString("myArray", JSONSerializer.ToJson(myArray));
int[] loadedArray = JSONSerializer.FromJson<int[]>(PlayerPrefs.GetString("myArray"));
```

Another potential use is Unity's new [*RemoteSettings*](https://blogs.unity3d.com/2017/06/02/introducing-remote-settings-update-your-game-in-an-instant/) feature which allows the real-time updating of ints, floats, booleans and strings values. Arrays presently aren't supported, but by storing them as JSON strings, one could easily pull them into the game.

## Deserialization of a Database

Consider that the game supports localization and we wish to import a simple database. One approach would be to export the database to a custom json of the form 

```json
{ "items" : [
{ "key": "GameWon", "value": "You completed the level!"},
{ "key": "GameLost", "value": "You failed to score enough points."}
]}
```

and then import it into our game using ```FromJson<Dictionary<String, String>>``` as shown above. More on localization in a future tip!

## Parsing Web-Requests

When one makes a web request, generally a JSON dictionary is returned. This could be deserialized to Dictionary using custom classes or probably more efficiently using the [*MiniJson*](https://gist.github.com/darktable/1411710) class

```C#
using MiniJSON;

Dictionary<string,object> dict = Json.Deserialize(json) as Dictionary<string,object>;
```

## Conclusion

*JSON Serialization* is a versatile serialization method that I find particularly useful for database deserialization and web requests. Although the built-in *JSONUtility* class isn't yet fully compatible with all objects, as we've seen in this post, one can easily add new functionality to enable the serialization of dictionaries and top-level arrays.