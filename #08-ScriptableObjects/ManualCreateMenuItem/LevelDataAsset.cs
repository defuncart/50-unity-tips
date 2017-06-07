/*
 * 	Written by James Leahy (c) 2017 DeFunc Art.
 */
using UnityEngine;
using UnityEditor;

public class LevelDataAsset
{
	[MenuItem("Assets/Create/LevelData")]
	public static void CreateAsset()
	{
		ScriptableObjectUtility.CreateAsset<LevelData>();
	}
}