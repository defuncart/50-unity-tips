/*
 *  Written by James Leahy. (c) 2018 DeFunc Art.
 *  https://github.com/defuncart/
 */
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>Included in the DeFuncArtEditor namespace.</summary>
namespace DeFuncArtEditor
{
    /// <summary>Custom Editor for the DefaultAsset.</summary>
    [CustomEditor(typeof(DefaultAsset), true)]
    public class DefaultAssetEditor : Editor
    {
        /// <summary>A large font size.</summary>
        private const int FONT_SIZE_LARGE = 42;
        /// <summary>A medium font size.</summary>
        private const int FONT_SIZE_MEDIUM = 28;
        /// <summary>A small font size.</summary>
        private const int FONT_SIZE_SMALL = 18;

        /// <summary>Callback to draw the inspector.</summary>
        public override void OnInspectorGUI()
        {
            //draw the default inspector
            DrawDefaultInspector();

            //get the asset's path
            string assetPath = AssetDatabase.GetAssetPath(target);

            //if the asset is a folder
            if(Directory.Exists(assetPath))
            {
                //draw the folder description
                string description = DAFolderAsset.Descriptions.DescriptionForFolder(assetPath);
                DrawLabel(text: description, fontSize: FONT_SIZE_MEDIUM, height: Screen.height * 0.3f);

                //draw the folder's valid filetypes
                string validFileTypes = DAFolderAsset.FileTypes.FileTypesForFolderAsString(assetPath);
                DrawLabel(text: validFileTypes, fontSize: FONT_SIZE_MEDIUM, height: Screen.height * 0.2f);

                //draw the folder's naming conventions
                string namingConventions = DAFolderAsset.FileNamingConventions.FileNamingConventionsForFolder(assetPath);
                DrawLabel(text: namingConventions, fontSize: FONT_SIZE_MEDIUM);
            }

        }

        /// <summary>Draws a label with a given text, font size and height.</summary>
        /// <param name="text">The text.</param>
        /// <param name="fontSize">The font size.</param>
        /// <param name="height">The height (defaults to required height).</param>
        private void DrawLabel(string text, int fontSize, float height=-1)
        {
            if(height > 0) { EditorGUILayout.LabelField(text, new GUIStyle{ fontSize = fontSize, richText = true, wordWrap = true }, GUILayout.Height(height)); }
            else { EditorGUILayout.LabelField(text, new GUIStyle { fontSize = fontSize, richText = true, wordWrap = true }); }
        }
    }
}
#endif
