/*
 *  Written by James Leahy. (c) 2018 DeFunc Art.
 *  https://github.com/defuncart/
 */
#if UNITY_EDITOR
using DeFuncArtEditor;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class MyFolderDescriptions
{
    static MyFolderDescriptions()
    {
        DAFolderAsset.Descriptions.AddFolderDescriptions(
            new KeyValuePair<string, string>("Assets/Localization", "A folder of localization files.")
        );
    }
}

[InitializeOnLoad]
public class MyFolderFileTypes
{
    static MyFolderFileTypes()
    {
        DAFolderAsset.FileTypes.AddFolderFileTypes(
            new KeyValuePair<string, List<string>>("Assets/Localization", new List<string> { "json" })
        );
    }
}

[InitializeOnLoad]
public class MyFolderFileNamingConventions
{
    static MyFolderFileNamingConventions()
    {
        DAFolderAsset.FileNamingConventions.AddFolderFileNamingConventions(
            new KeyValuePair<string, string>("Assets/Localization", "All files are expected to be named English, German etc., that is, the name of the language in English with the first letter capitalized.")
        );
    }
}
#endif
