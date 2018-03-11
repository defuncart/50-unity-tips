/*
 *  Written by James Leahy. (c) 2017-2018 DeFunc Art.
 *  https://github.com/defuncart/
 */
#if UNITY_EDITOR
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

// <summary>Part of the DeFuncArt.Utilities namespace.</summary>
namespace DeFuncArtEditor
{
	/// <summary>Performs operations on files and folders in the editor.</summary>
  public class FolderManager
  {
		/// <summary>Gets an array of assets of type T at a given path. This path is relative to /Assets.</summary>
		/// <returns>An array of assets of type T.</returns>
		/// <param name="path">The file path relative to /Assets.</param>
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

    /// <summary>Returns a full path for a given relative path.</summary>
    /// <param name="path">The relative path.</param>
    /// <returns>The full path.</returns>
    private static string FullPathForRelativePath(string path)
    {
      return Application.dataPath + "/" + path;
    }
  }
}
#endif
