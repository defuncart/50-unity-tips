# 20 - Asset Postprocessor

*AssetPostprocessor* is an Editor class which allows access to the import pipeline and the ability to run scripts prior or after importing assets. Each asset to import has an *assetImporter* and an *assetPath*, both of which are accessible in *Preprocess* and *Postprocess* callbacks. The *assetImporter* itself can either be an *AudioImporter*, *IHVImageFormatImporter*, *ModelImporter*, *MovieImporter*, *PluginImporter*, *SpeedTreeImporter*, *SubstanceImporter*, *TextureImporter*, *TrueTypeFontImporter* or *VideoClipImporter*, depending on the asset being imported.

## OnPreprocessAudio

As a simple example, lets assume that all audio files that will be imported into the project are speech files that should always undergo the same import settings. In that case, we could write a simple script which has a callback before an audio file is imported, and adjust the importer's settings accordingly.

```C#
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>An editor script which listens to import events.</summary>
public class MyAssetPostprocessor : AssetPostprocessor
{
  /// <summary>Callback before an audio clip is imported.</summary>
  private void OnPreprocessAudio()
  {
    AudioImporter audioImporter = assetImporter as AudioImporter;
    audioImporter.forceToMono = true;
    audioImporter.preloadAudioData = false;
    AudioImporterSampleSettings settings = new AudioImporterSampleSettings() {
      loadType = AudioClipLoadType.DecompressOnLoad,
      compressionFormat = AudioCompressionFormat.Vorbis,
      quality = 0,
      sampleRateSetting = AudioSampleRateSetting.OverrideSampleRate,
      sampleRateOverride =  22050
    };
    audioImporter.defaultSampleSettings = settings;
  }
}
#end UNITY_EDITOR
```

## OnPostprocessSprites

As another simple example, lets assume that all UI sprites should have high quality compression. As all UI sprites will be imported into ```Assets/Sprites/UI```, we can use the *assetPath* to verify which textures are UI sprites, and **OnPostprocessSprites** to automatically set the asset's import settings.

```C#
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>An editor script which listens to import events.</summary>
public class MyAssetPostprocessor : AssetPostprocessor
{
  /// <summary>Callback after a texture of sprites has completed importing.</summary>
  /// <param name="texture">The texture.</param>
  /// <param name="sprites">The array of sprites.</param>
  private void OnPostprocessSprites(Texture2D texture, Sprite[] sprites)
  {
    if(System.IO.Path.GetDirectoryName(assetPath) == "Assets/Sprites/UI")
    {
      TextureImporter textureImporter = assetImporter as TextureImporter;
      textureImporter.textureCompression = TextureImporterCompression.CompressedHQ;
      textureImporter.crunchedCompression = true;
      textureImporter.compressionQuality = 100;
    }
  }
}
#end UNITY_EDITOR
```

## Conclusion

Although both these examples are simple and somewhat specific, one can appreciate the efficiency of correctly assigning settings on import as opposed to manually editing after import. Moreover, in combination with custom windows or menu items, the artist or sound engineer could define what the custom import settings should be, and use these as defaults.

## Further Reading

[Scripting API - AssetPostprocessor](https://docs.unity3d.com/ScriptReference/AssetPostprocessor.html)

[Scripting API - AssetPostprocessor.OnPreprocessAudio](https://docs.unity3d.com/ScriptReference/AssetPostprocessor.OnPreprocessAudio.html)

[Scripting API - AssetPostprocessor.OnPostprocessSprites](https://docs.unity3d.com/ScriptReference/AssetPostprocessor.OnPostprocessSprites.html)
