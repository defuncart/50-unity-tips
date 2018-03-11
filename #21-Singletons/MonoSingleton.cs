/*
 *	Written by James Leahy. (c) 2017-2018 DeFunc Art.
 *	https://github.com/defuncart/
 */
using UnityEngine;

/// <summary>A base abstract class which can be extented to make a singleton component attachable to a game object.</summary>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
	/// <summary>A static instance which is created on first lauch and thereafter never destroyed.</summary>
	public static T instance { get; private set; }

	/// <summary>Callback when the instance is awoken.
	/// Ensure that there is only one instance of the class and that it cannot be destroyed.</summary>
	private void Awake()
	{
		if(instance == null) { instance = this as T; DontDestroyOnLoad(gameObject); instance.Init(); }
		else if(instance != this) { Destroy(gameObject); }
	}

	/// <summary>Init the specific inherited class.</summary>
	protected virtual void Init() {}
}
