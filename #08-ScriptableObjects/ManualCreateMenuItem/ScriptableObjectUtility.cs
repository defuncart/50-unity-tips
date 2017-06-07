/*
 * 	Written by James Leahy (c) 2017 DeFunc Art.
 */
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>Utilities for the ScriptableObject class.</summary>
public static class ScriptableObjectUtility
{
	/// <summary>A helper method which creates a new asset of a given type.</summary>
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