/*
 *  Written by James Leahy. (c) 2018 DeFunc Art.
 *  https://github.com/defuncart/
 */
#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>Included in the DeFuncArtEditor namespace.</summary>
namespace DeFuncArtEditor
{
    /// <summary>A class of folder options.</summary>
    public class DAFolderAsset
    {
        /// <summary>A class which contains folder descriptions for given folder paths.</summary>
        public static class Descriptions
        {
            /// <summary>A placeholder stating that no explicit description was given.</summary>
            private const string PLACEHOLDER = "<color=red>No</color> description given.";

            /// <summary>A dictionary of folder paths and descriptions.</summary>
            private static Dictionary<string, string> dict = new Dictionary<string, string>
            {
                { "Assets/Animations", "A folder for animations." },
                { "Assets/Audio", "A folder for audio files and audio databases. Music and SFX subfolders are expected." },
                { "Assets/Editor", "A folder for custom editor scripts and color palettes." },
                { "Assets/Fonts", "A folder for fonts." },
                { "Assets/Prefabs", "A folder for prefabs." },
                { "Assets/Scenes", "A folder for scenes." },
                { "Assets/Scripts", "A folder for scripts." },
                { "Assets/Sprites", "A folder for sprites." }
            };

            /// <summary>Adds a param list of folder path, folder descriptions key-value pairs.</summary>
            /// <param name="kvps">The key-value pairs.</param>
            public static void AddFolderDescriptions(params KeyValuePair<string, string>[] kvps)
            {
                foreach(KeyValuePair<string, string> kvp in kvps)
                {
                    if(!dict.ContainsKey(kvp.Key)) { dict[kvp.Key] = kvp.Value; }
                }
            }

            /// <summary>Returns a description for a given folder path.</summary>
            /// <param name="folder">The folder path.</param>
            public static string DescriptionForFolder(string folder)
            {
                return dict.ContainsKey(folder) ? dict[folder] : PLACEHOLDER;
            }
        }

        /// <summary>A class which contains valid file types for given folder paths.</summary>
        public static class FileTypes
        {
            /// <summary>Placeholder text stating that all files are valid.</summary>
            private const string TEXT_ALL_FILE_TYPES = "<color=green>all</color>";
            /// <summary>Text stating the valid file types.</summary>
            private const string TEXT_VALID_FILE_TYPES = "Valid file types: ";
            /// <summary>A string builder.</summary>
            private static StringBuilder sb;

            /// <summary>A dictionary of folder paths and valid filetypes.</summary>
            private static Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>
            {
                { "Assets/Audio", new List<string>{ "aif", "aiff", "mp3", "wav" } },
                { "Assets/Editor", new List<string>{ "cs", "colors" } },
                { "Assets/Fonts", new List<string>{ "tff" } },
                { "Assets/Prefabs", new List<string>{ "prefab" } },
                { "Assets/Scenes", new List<string>{ "unity" } },
                { "Assets/Scripts", new List<string>{ "cs" } },
                { "Assets/Sprites", new List<string>{ "png", "jpg", "psd" } }
            };

            /// <summary>Adds a param list of folder path, list of valid file types key-value pairs.</summary>
            /// <param name="kvps">The key-value pairs.</param>
            public static void AddFolderFileTypes(params KeyValuePair<string, List<string>>[] kvps)
            {
                foreach(KeyValuePair<string, List<string>> kvp in kvps)
                {
                    if(!dict.ContainsKey(kvp.Key)) { dict[kvp.Key] = kvp.Value; }
                }
            }

            /// <summary>Returns a list of valid file types for a given folder path.</summary>
            /// <param name="folder">The folder path.</param>
            /// <param name="checkParentFolders">If the path has no explicit valid file types, whether the parent's should be considered.</param>
            public static List<string> FileTypesForFolder(string folder, bool checkParentFolders = true)
            {
                string key = folder;
                while(!dict.ContainsKey(key) && checkParentFolders)
                {
                    DirectoryInfo info = Directory.GetParent(key);
                    if(info == null) { break; }

                    key = info.ToString();
                    if(key == "Assets") { break; } //this isn't necessary as Directory.GetParent("Assets") should return null
                }

                if (dict.ContainsKey(key)) { return dict[key]; }
                return null;
            }

            /// <summary>Returns a string representation of the valid folder types for a given folder path.</summary>
            /// <param name="folder">The folder path.</param>
            /// <param name="checkParentFolders">If the path has no explicit valid file types, whether the parent's should be considered.</param>
            public static string FileTypesForFolderAsString(string folder, bool checkParentFolders = true)
            {
                //initialize the string builder
                if(sb == null) { sb = new StringBuilder(); } else { sb.Length = 0; sb.Append(TEXT_VALID_FILE_TYPES); }

                List<string> fileTypes = FileTypesForFolder(folder, checkParentFolders);
                if(fileTypes == null)
                {
                    sb.Append(TEXT_ALL_FILE_TYPES);
                }
                else //write the list to string with commas between elements
                {
                    for(int i=0; i < fileTypes.Count; i++)
                    {
                        if(i != 0) { sb.Append(", "); }
                        sb.Append(fileTypes[i]);
                    }
                }
                //return the string builder contents
                return sb.ToString();
            }
        }

        /// <summary>A class which contains file naming conventions for given folder paths.</summary>
        public static class FileNamingConventions
        {
            /// <summary>A placeholder stating that there are no explicit file naming conventions.</summary>
            private const string PLACEHOLDER = "<color=green>None</color>.";
            /// <summary>The file naming conventions text.</summary>
            private const string TEXT_FILE_NAMING_CONVENTIONS = "File naming conventions: ";

            /// <summary>A dictionary of folder paths and naming conventions.</summary>
            private static Dictionary<string, string> dict = new Dictionary<string, string>
            {
            };

            /// <summary>Adds a param list of folder path, folder file naming conventions key-value pairs.</summary>
            /// <param name="kvps">The key-value pairs.</param>
            public static void AddFolderFileNamingConventions(params KeyValuePair<string, string>[] kvps)
            {
                foreach(KeyValuePair<string, string> kvp in kvps)
                {
                    if(!dict.ContainsKey(kvp.Key)) { dict[kvp.Key] = kvp.Value; }
                }
            }

            /// <summary>Returns a file naming convention for a given folder path.</summary>
            /// <param name="folder">The folder path.</param>
            public static string FileNamingConventionsForFolder(string folder)
            {
                return string.Format("{0}{1}", TEXT_FILE_NAMING_CONVENTIONS, dict.ContainsKey(folder) ? dict[folder] : PLACEHOLDER);
            }
        }
    }
}
#endif
