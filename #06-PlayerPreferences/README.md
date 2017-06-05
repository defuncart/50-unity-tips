# 06 - Player Preferences

[*PlayerPrefs*](https://docs.unity3d.com/ScriptReference/PlayerPrefs.html) is a static class which one can store and access player preferences between scenes and game sessions. The class is useful for storing basic values but has two main drawbacks:

1. it is severely limited in that it can only store strings, ints and floats.
2. the data is completely unsecure

With regard to security, on Desktop the values are stored to a PLIST on macOS or registry on Windows, both of which are accessible and modifiable by the user. [*EncryptedPlayerPrefs*](https://gist.github.com/ftvs/5299600) by Sven Magnus is one approach to rectify this issue, while there are numerous paid assets available on the asset store.

The lack of support for booleans can be easily solved by using 1 and 0, but for readability I would prefer *SetBool*, *GetBool* methods. As *PlayerPrefs* is called statically, it is impossible to add an Extention Method. Thus I created the *PlayerPreferences* class which, although guilty of being unadulterated syntactic sugar, is arguably more readable.

```C#
public static void SetBool(string key, bool value)
{
  SetInt(key, value ? 1 : 0);
}

public static bool GetBool(string key)
{
  if(!HasKey(key)) { Debug.LogError(string.Format("Key \"{0}\" not found!", key)); }
  return GetInt(key) == 1;
}
```

Also to be less error prone, I prefer the use of constant string keys

```C#
public struct Keys
{
  public static readonly string gameInitialized = "gameInitialized";
}
```

which makes the function calls much more readable

```C#
if(!PlayerPreferences.GetBool(PlayerPreferences.Keys.gameInitialized))
{
  //do stuff
  PlayerPreferences.SetBool(PlayerPreferences.Keys.gameInitialized, true);
}
```

So today's tip is not only somewhat subjective, but is also probably not too useful. Nevertheless it is something I ocassionally use so I felt it was worth mentioning.