/*
 *  Written by James Leahy. (c) 2017-2018 DeFunc Art.
 *  https://github.com/defuncart/
 */
#if UNITY_EDITOR
using DeFuncArt.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>An EditorWindow to gerate Pseudolocalizations.</summary>
public class PseudoLocalizationWindow : EditorWindow
{
    /// <summary>An enum representing the types of languages that can be rendered in Pseudotext.</summary>
    public enum Language
    {
        German, Polish, Russian
    }
    /// <summary>The chosen language to render.</summary>
    private Language langugage;

    /// <summary>The special characters for German.</summary>
    private string[] specialCharacters_DE = { "ä", "ö", "ü", "ß", "Ä", "Ö", "Ü" };
    /// <summary>The special characters for Polish.</summary>
    private string[] specialCharacters_PL = { "ą", "ć", "ę", "ł", "ń", "ó", "ś", "ż", "ź", "Ą", "Ć", "Ę", "Ł", "Ń", "Ó", "Ś", "Ż", "Ź" };
    /// <summary>The special characters for Russian.</summary>
    private string[] specialCharacters_RU = { "а", "б", "в", "г", "д", "е", "ё", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я", "А", "Б", "В", "Г", "Д", "Е", "Ё", "Ж", "З", "И", "Й", "К", "Л", "М", "Н", "О", "П", "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ", "Ъ", "Ы", "Ь", "Э", "Ю", "Я" };
    /// <summary>The special characters the selected language.</summary>
    private string[] specialCharacters
    {
        get
        {
            if (langugage == Language.German) { return specialCharacters_DE; }
            else if (langugage == Language.Polish) { return specialCharacters_PL; }
            else { return specialCharacters_RU; }
        }
    }
    /// <summary>A random special character for the selected language.</summary>
    private string randomSpecialCharacter
    {
        get { return specialCharacters[Random.Range(0, specialCharacters.Length)]; }
    }

    /// <summary>A dictionary of mapping characters for German.</summary>
    private Dictionary<string, string[]> mappingCharacters_DE = new Dictionary<string, string[]>(){
        {"a" , new string[]{"ä"} }, {"A" , new string[]{"Ä"} },
        {"o" , new string[]{"ö"} }, {"O" , new string[]{"Ö"} },
        {"u" , new string[]{"ü"} }, {"U" , new string[]{"Ü"} },
        {"s" , new string[]{"ß"} }
    };
    /// <summary>A dictionary of mapping characters for Polish.</summary>
    private Dictionary<string, string[]> mappingCharacters_PL = new Dictionary<string, string[]>(){
        {"a" , new string[]{"ą"} }, {"A" , new string[]{"Ą"} },
        {"c" , new string[]{"ć"} }, {"C" , new string[]{"Ć"} },
        {"e" , new string[]{"ę"} }, {"E" , new string[]{"Ę"} },
        {"l" , new string[]{"ł"} }, {"L" , new string[]{"Ł"} },
        {"n" , new string[]{"ń"} }, {"N" , new string[]{"Ń"} },
        {"o" , new string[]{"ó"} }, {"O" , new string[]{"Ó"} },
        {"s" , new string[]{"ś"} }, {"S" , new string[]{"Ś"} },
        {"z" , new string[]{"ż", "ź"} }, {"Z" , new string[]{"Ż", "Ź"} }
    };
    /// <summary>A dictionary of mapping characters for Russian.</summary>
    private Dictionary<string, string[]> mappingCharacters_RU = new Dictionary<string, string[]>(){
        {"a" , new string[]{"а"} }, {"A" , new string[]{"А"} },
        {"b" , new string[]{"ь", "в", "б", "ъ"} }, {"B" , new string[]{"Ь", "В", "Б", "Ъ"} },
        {"c" , new string[]{"с"} }, {"C" , new string[]{"С"} },
        {"d" , new string[]{"д"} }, {"D" , new string[]{"Д"} },
        {"e" , new string[]{"е", "ё", "э"} }, {"E" , new string[]{"Е", "Ё", "Э"} },
        {"f" , new string[]{"ф"} }, {"F" , new string[]{"Ф"} },
        {"g" , new string[]{"г"} }, {"G" , new string[]{"Г"} },
        {"h" , new string[]{"н"} }, {"H" , new string[]{"Н"} },
        {"i" , new string[]{"и"} }, {"I" , new string[]{"И"} },
        {"j" , new string[]{"й"} }, {"J" , new string[]{"Й"} },
        {"k" , new string[]{"к"} }, {"K" , new string[]{"К"} },
        {"l" , new string[]{"л"} }, {"L" , new string[]{"Л"} },
        {"m" , new string[]{"м"} }, {"M" , new string[]{"М"} },
        {"n" , new string[]{"п"} }, {"N" , new string[]{"П"} },
        {"o" , new string[]{"о"} }, {"O" , new string[]{"О"} },
        {"p" , new string[]{"р"} }, {"P" , new string[]{"Р"} },
        {"q" , new string[]{"ч"} }, {"Q" , new string[]{"Ч"} },
        {"r" , new string[]{"я"} }, {"R" , new string[]{"Я"} },
        {"s" , new string[]{"з"} }, {"S" , new string[]{"З"} },
        {"t" , new string[]{"т"} }, {"T" , new string[]{"Т"} },
        {"u" , new string[]{"ц"} }, {"U" , new string[]{"Ц"} },
        {"v" , new string[]{"ч"} }, {"V" , new string[]{"Ч"} },
        {"w" , new string[]{"ш", "щ"} }, {"W" , new string[]{"Ш", "Щ"} },
        {"x" , new string[]{"х", "ж"} }, {"X" , new string[]{"Х", "Ж"} },
        {"y" , new string[]{"у"} }, {"Y" , new string[]{"У"} },
        {"z" , new string[]{"з"} }, {"Z" , new string[]{"З"} }
    };
    /// <summary>A dictionary of mapping characters for the selected language.</summary>
    private Dictionary<string, string[]> mappingCharacters
    {
        get
        {
            if(langugage == Language.German) { return mappingCharacters_DE; }
            else if(langugage == Language.Polish) { return mappingCharacters_PL; }
            else { return mappingCharacters_RU; }
        }
    }

    /// <summary>A reference to the TextAsset JSON file with English strings.</summary>
    public Object englishJSONAsset;

    //add a menu item named "Pseudolocalization" to the Tools menu
    [MenuItem("Tools/Pseudolocalization")]
    public static void ShowWindow()
    {
        //show existing window instance - if one doesn't exist, create one
        GetWindow(typeof(PseudoLocalizationWindow));
    }

    /// <summary>Draws the window.</summary>
    private void OnGUI()
    {
        //set the label's style to wrap words
        EditorStyles.label.wordWrap = true;

        //draw an info label
        EditorGUILayout.LabelField("Input English strings (as JSON):");

        //draw the englishJSONAsset
        englishJSONAsset = EditorGUILayout.ObjectField(englishJSONAsset, typeof(TextAsset), true);

        //draw a space
        EditorGUILayout.Space();

        //draw a language popup
        langugage = (Language) EditorGUILayout.EnumPopup("Language to render:", langugage);

        //draw an info label
        if(englishJSONAsset != null)
        {
            string relativeOutputFilepath = AssetDatabase.GetAssetPath(englishJSONAsset).Replace(Path.GetFileName(AssetDatabase.GetAssetPath(englishJSONAsset)), string.Format("{0}Pseudo.json", langugage.ToString()));
            EditorGUILayout.LabelField(string.Format("The output file will be saved to {0}", relativeOutputFilepath));
        }

        //draw a space
        EditorGUILayout.Space();

        //draw a button which, if triggered, render the Pseudolocalization for the selected language
        if(GUILayout.Button("Render Pseudolocalization")) { RenderPseudolocalization(); }
    }

    /// <summary>Render the Pseudolocalization for the selected language.</summary>
    private void RenderPseudolocalization()
    {
        //display an error if there is no input file
        if(englishJSONAsset == null) { Debug.LogErrorFormat("No input file."); return; }

        //load the json file as a dictionary
        TextAsset asset = englishJSONAsset as TextAsset;
        Dictionary<string, string> inputJSON = JSONSerializer.FromJson<Dictionary<string, string>>(asset.text);
        Dictionary<string, string> outputJSON = new Dictionary<string, string>();

        //loop through each kvp
        foreach(KeyValuePair<string, string> kvp in inputJSON)
        {
            //determine the pseudotranslation
            string englishText = kvp.Value;
            int numberOfRandomSpecialCharactersToGenerate = PseudotranslationLengthForText(englishText) - englishText.Length;
            string pseudoTranslation = string.Format("{0}|{1}", AddSpecialCharactersToText(englishText), GenerateXRandomSpecialCharacters(numberOfRandomSpecialCharactersToGenerate));

            //add to output dictionary
            outputJSON[kvp.Key] = pseudoTranslation;
        }

        //convert the dictionary to json
        string outputJSONString = JSONSerializer.ToJson<Dictionary<string, string>>(outputJSON);

        //determine a path for the output file
        string path = string.Format("{0}/{1}Pseudo.json", Path.GetFullPath(Directory.GetParent(AssetDatabase.GetAssetPath(englishJSONAsset)).FullName), langugage.ToString());

        //save the file to disk
        File.WriteAllText(path, outputJSONString);

        //update asset database
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    /// <summary>Returns a string containing mapped special characters (a => ä) for the selected language.</summary>
    private string AddSpecialCharactersToText(string text)
    {
        StringBuilder sb = new StringBuilder();
        char[] characters = text.ToCharArray();
        string[] keys = mappingCharacters.Keys.ToArray();
        foreach(char character in characters)
        {
            int index = System.Array.IndexOf(keys, character.ToString());
            if(index > 0)
            {
                string[] possibleMappings = mappingCharacters[character.ToString()];
                sb.Append(possibleMappings[Random.Range(0, possibleMappings.Length)]);
            }
            else { sb.Append(character); }
        }

        return sb.ToString();
    }

    /// <summary>Returns a string contain X random special characters for the selected language.</summary>
    private string GenerateXRandomSpecialCharacters(int count)
    {
        StringBuilder sb = new StringBuilder();
        for(int i=0; i < count; i++)
        {
            sb.Append(randomSpecialCharacter);
        }
        return sb.ToString();
    }

    /// <summary>Determine the Pseudotranslation length for a given text string.</summary>
    private int PseudotranslationLengthForText(string text)
    {
        if(text.Length > 20) { return Mathf.CeilToInt(text.Length * 1.3f); }
        else if(text.Length > 10) { return Mathf.CeilToInt(text.Length * 1.4f); }
        else { return Mathf.CeilToInt(text.Length * 1.5f); }
    }
}
#endif
