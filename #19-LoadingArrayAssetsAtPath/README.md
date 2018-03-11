# 19 - Loading an Array of Assets at Path

AssetDatabase allows us to easily load an asset in editor mode using the ```LoadAssetAtPath``` method:

```C#
MyAsset myAsset = AssetDatabase.LoadAssetAtPath<MyAsset>(pathToAsset);
```

Thus once would expect that ```LoadAllAssetsAtPath``` would return an object array of all assets of type T at *pathToAssets*:

```C#
MyAsset[] myAssets = AssetDatabase.LoadAllAssetsAtPath(pathToAssets);
```

and is then somewhat bemused when they receive an empty array.

So how come ```AssetDatabase.LoadAllAssetsAtPath("Assets/Data/")``` returns nothing when  ```AssetDatabase.LoadAssetAtPath<MyAsset>("Assets/Data/MyAsset.asset")``` returns a valid asset?

Well it turns out that my (and potentially your) interpretation of this method signature (and description *Returns an array of all asset objects at assetPath*) is incorrect. ```LoadAllAssetsAtPath``` doesn't load all assets for a given directory, it loads all objects for a given asset. This is clear when visiting the API page, but not when coding in the IDE.

```C#
Object[] data = AssetDatabase.LoadAllAssetsAtPath("Assets/MyMaterial.mat");
```

> Some asset files may contain multiple objects (such as a Maya file which may contain multiple Meshes and GameObjects). This function returns all asset objects at a given path including hidden in the Project view.

## Rolling our own

Luckily we can easily roll our own solution by using **System.IO.Directory** to get the contents of a folder, and using **System.Linq** to ignore all *.meta* files.

```C#
/// <summary>Gets an array of assets of type T at a given path. This path is relative to /Assets.</summary>
/// <returns>An array of assets of type T.</returns>
/// <param name="path">The file path relative to /Assets.</param>
public static T[] GetAssetsAtPath<T>(string path) where T : Object
{  
  List<T> returnList = new List<T>();

  //get the contents of the folder's full path (excluding any meta files) sorted alphabetically
  IEnumerable<string> fullpaths = Directory.GetFiles(FullPathForRelativePath(path)).Where(x => !x.EndsWith(".meta")).OrderBy(s => s);
  //loop through the folder contents
  foreach (string fullpath in fullpaths)
  {
      //determine a path starting with Assets
      string assetPath = fullpath.Replace(Application.dataPath, "Assets");
      //load the asset at this relative path
      Object obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);
      //and add it to the list if it is of type T
      if(obj is T) { returnList.Add(obj as T); }
  }

  return returnList.ToArray();
}
```

This can then be used as follows:

```C#
MyAsset[] myAssets = GetAssetsAtPath<MyAsset>("/Data");
```

where the path supplied is relative to */Assets*.

## Search Subdirectories

The above solution works pretty well when we want to load an array of assets from a top-level directory, however, what if we would like to search subdirectories too? Again this is easily achieved using **System.IO.Directory** and recursion.

```C#
/// <summary>Gets an array of assets of type T at a given path. This path is relative to /Assets.</summary>
/// <returns>An array of assets of type T.</returns>
/// <param name="path">The file path relative to /Assets.</param>
/// <param name="recursively">Whether subdirectories should be considered.</param>
public static T[] GetAssetsAtPath<T>(string path, bool recursively = false) where T : Object
{  
  //create a list to store results
  List<T> returnList = new List<T>();
  //process the given filepath
  ProcessDirectory(path, ref returnList, recursively);
  //return results as an array
	return returnList.ToArray();
}

/// <summary>Processes a directory to find files of type T.</summary>
/// <param name="path">The file path relative to /Assets).</param>
/// <param name="returnList">A ref list to add results to.</param>
/// <param name="recursively">Whether subdirectories should be considered.</param>
/// <typeparam name="T">The type parameter.</typeparam>
private static void ProcessDirectory<T>(string path, ref List<T> returnList, bool recursively = false) where T : Object
{
    //get the contents of the folder's full path (excluding any meta files) sorted alphabetically
    IEnumerable<string> fullpaths = Directory.GetFiles(FullPathForRelativePath(path)).Where(x => !x.EndsWith(".meta")).OrderBy(s => s);
    //loop through the folder contents
    foreach (string fullpath in fullpaths)
    {
        //determine a path starting with Assets
        string assetPath = fullpath.Replace(Application.dataPath, "Assets");
        //load the asset at this relative path
        Object obj = AssetDatabase.LoadAssetAtPath<T>(assetPath);
        //and add it to the list if it is of type T
        if(obj is T) { returnList.Add(obj as T); }
    }

    if(recursively)
    {
        //path is relative to /Assets - to determine subdirectories, we need to query the full data
        string[] subdirectories = Directory.GetDirectories(FullPathForRelativePath(path));
        //loop through all subdirectories and recursively call ProcessDirectory on a relative path
        foreach(string subdirectory in subdirectories)
        {
            string subPath = path + "/" + Path.GetFileName(subdirectory);
            ProcessDirectory(subPath, ref returnList);
        }
     }
}
```

## Conclusion

This is a short yet powerful editor script which I use all the time, especially when developing tools to automatically import assets from a given directory.

## Further Reading

[Scripting Reference - AssetDatabase](https://docs.unity3d.com/ScriptReference/AssetDatabase.html)

[Scripting Reference - AssetDatabase.LoadAssetAtPath](https://docs.unity3d.com/ScriptReference/AssetDatabase.LoadAssetAtPath.html)

[Scripting Reference - AssetDatabase.LoadAllAssetsAtPath](https://docs.unity3d.com/ScriptReference/AssetDatabase.LoadAllAssetsAtPath.html)
