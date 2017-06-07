/*
 * 	Written by James Leahy (c) 2017 DeFunc Art.
 */
using UnityEngine;

[CreateAssetMenu(fileName = "New LevelData", menuName = "LevelData", order = 1000)]
public class LevelData : ScriptableObject
{
	public int levelIndex;
	public int numberOfPlayerLives;
	public bool isTimed;
	public float timeLimit;

	public override string ToString ()
	{
		return string.Format("[LevelData:levelIndex={0}, numberOfPlayerLives={1}, isTimed={2}, timeLimit={3}]", 
			levelIndex, numberOfPlayerLives, isTimed, timeLimit);
	}
}
