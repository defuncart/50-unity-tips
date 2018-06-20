/*
 *  Written by James Leahy. (c) 2018 DeFunc Art.
 *  https://github.com/defuncart/
 */
#if UNITY_EDITOR
using DeFuncArtEditor;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>A custom asset postprocessor which verifies that assets are imported into/moved to valid folders.</summary>
public class DAFolderAssetPostprocessor : AssetPostprocessor
{
    /// <summary>Callback after the importing of assets has completed. This call can occur after a manual reimport, or any time an asset/folder of assets 
    /// are moved to a new location in the Project View. All string arrays are filepaths relative to the Project's root Assets folder.</summary>
    /// <param name="importedAssets">The imported assets.</param>
    /// <param name="deletedAssets">The deleted assets.</param>
    /// <param name="movedAssets">The moved assets.</param>
    /// <param name="movedFromAssetPaths">The moved-from assets.</param>
    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        Assert.AreEqual(movedAssets.Length, movedFromAssetPaths.Length);

        //check that all imported assets were imported into a valid folder
        foreach(string assetPath in importedAssets)
        {
            if(File.Exists(assetPath)) //file as opposed to a directory
            {
                string folder = Path.GetDirectoryName(assetPath);
                List<string> fileTypes = DAFolderAsset.FileTypes.FileTypesForFolder(folder);
                if(fileTypes != null)
                {
                    string extension = Path.GetExtension(assetPath).Replace(".", "");
                    if(!fileTypes.Contains(extension))
                    {
                        Debug.LogErrorFormat("Asset <color=red>{0}</color> could not be imported into folder <color=blue>{1}</color> because <color=green>{2}</color> is an invalid filetype.", Path.GetFileName(assetPath), folder, extension);
                        AssetDatabase.DeleteAsset(assetPath);
                    }
                }
            }
        }

        //check that all moved assets were moved into a valid folder
        for(int i=0; i < movedAssets.Length; i++)
        {
            string assetPath = movedAssets[i];
            if(File.Exists(assetPath)) //file as opposed to a directory
            {
                string folder = Path.GetDirectoryName(assetPath);
                List<string> fileTypes = DAFolderAsset.FileTypes.FileTypesForFolder(folder);
                if(fileTypes != null)
                {
                    string extension = Path.GetExtension(assetPath).Replace(".", "");
                    if(!fileTypes.Contains(extension))
                    {
                        Debug.LogErrorFormat("Asset <color=red>{0}</color> could not be moved to folder <color=blue>{1}</color> because <color=green>{2}</color> is an invalid filetype.", Path.GetFileName(assetPath), folder, extension);
                        AssetDatabase.MoveAsset(movedAssets[i], movedFromAssetPaths[i]);
                    }
                }
            }
        }
    }
}
#endif
