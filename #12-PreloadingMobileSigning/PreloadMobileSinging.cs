/*
 *	Written by James Leahy. (c) 2017 DeFunc Art.
 */
#if UNITY_EDITOR
using UnityEditor;

/// <summary>A simple editor script which automatically sets up Android signing.</summary>
[InitializeOnLoad]
public class PreloadAndroidSigning
{
	/// <summary>The keystone password.</summary>
	private const string KEYSTORE_PASS = "KEYSTORE_PASS";
	/// <summary>The alias password.</summary>
	private const string ALIAS_PASSWORD = "ALIAS_PASSWORD";

	/// <summary>Initializes the class.</summary>
	static PreloadAndroidSigning()
	{
		PlayerSettings.Android.keystorePass = KEYSTORE_PASS;
		PlayerSettings.Android.keyaliasPass = ALIAS_PASSWORD;
	}
}

/// <summary>A simple editor script which automatically sets up iOS signing.</summary>
[InitializeOnLoad]
public class PreloadiOSSigning
{
	/// <summary>Whether the app should be automatically signed.</summary>
	private const bool AUTOMATICALLY_SIGN = true;
	/// <summary>The Apple Developer Team ID.</summary>
	private const string TEAM_ID = "TEAM_ID";

	/// <summary>Initializes the class.</summary>
	static PreloadiOSSigning()
	{
		if(PlayerSettings.iOS.appleEnableAutomaticSigning != AUTOMATICALLY_SIGN)
		{
			PlayerSettings.iOS.appleEnableAutomaticSigning = AUTOMATICALLY_SIGN;
		}
		if(AUTOMATICALLY_SIGN && string.IsNullOrEmpty(PlayerSettings.iOS.appleDeveloperTeamID))
		{
			PlayerSettings.iOS.appleDeveloperTeamID = TEAM_ID;
		}
	}
}

#endif
