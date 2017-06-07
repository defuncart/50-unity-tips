# Manually CreateMenuItem

To turn a scriptable object into an asset, we could simply place a [*CreateAssetMenu*](https://docs.unity3d.com/ScriptReference/CreateAssetMenuAttribute.html) attribute above our class declaration:

```
[CreateAssetMenu(fileName = "New LevelData", menuName = "LevelData", order = 1000)]
```

Another option is to create an editor script that resides within an Editor folder:

```C#
public class LevelDataAsset
{
  [MenuItem("Assets/Create/LevelData")]
  public static void CreateAsset()
  {
    LevelData asset = ScriptableObject.CreateInstance<LevelData>();
    AssetDatabase.CreateAsset(asset, "Assets/New LevelData.asset");
    AssetDatabase.SaveAssets();
    AssetDatabase.Refresh();
    EditorUtility.FocusProjectWindow();
    Selection.activeObject = asset;
  }
}

```

However, it seems pointless to write the same code for different scriptable objects over and over again, while it would be nice that the asset is saved where we are clicking, as opposed to the root of the Assets folder. So using this utility

```C#
public static class ScriptableObjectUtility
{
  public static void CreateAsset<T>() where T : ScriptableObject
  {
    //firstly we need to determine where to save the asset to. Try to get path of the editor's active object
    string path = AssetDatabase.GetAssetPath(Selection.activeObject);
    //if there is no selected object, set default path to Assets folder
    if(path.IsNullOrEmpty()) { path = "Assets"; }
    //else if the current object is a file, then remove the file's name from the path
    else if(Path.GetExtension(path) != string.Empty) { path = path.Replace(Path.GetFileName(path), string.Empty); }
    //add the asset's filename, e.g. "New LevelData.asset"
    path = AssetDatabase.GenerateUniqueAssetPath(string.Format("{0}/New {1}.asset", path, typeof(T).ToString()));

    //create a new instance of T, save it as an asset, and set it as active in the editor
    T asset = ScriptableObject.CreateInstance<T>();
    AssetDatabase.CreateAsset(asset, path);
    AssetDatabase.SaveAssets();
    AssetDatabase.Refresh();
    EditorUtility.FocusProjectWindow();
    Selection.activeObject = asset;
  }
}
```

we could easily create our asset as follows:

```C#
public class LevelDataAsset
{
  [MenuItem("Assets/Create/LevelData")]
  public static void CreateAsset()
  {
    ScriptableObjectUtility.CreateAsset<LevelData>();
  }
}

```

However, as we always need to write a new editor script for each scrictable object, I still feel that *CreateAssetMenu* is a better approach.