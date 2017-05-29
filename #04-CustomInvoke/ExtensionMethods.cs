	/*
 * 	Written by James Leahy (c) 2017 DeFunc Art.
 */ 
using System;
using System.Collections;
using UnityEngine;

/// <summary>A collection of extension methods for exisiting classes.</summary>
public static class ExtensionMethods
{
	/// <summary>Invokes an action after a given time.</summary>
	/// <param name="action">The action to invoke.</param>
	/// <param name="time">The time in seconds.</param>
	public static Coroutine Invoke(this MonoBehaviour monoBehaviour, Action action, float time)
	{
		return monoBehaviour.StartCoroutine(InvokeImplementation(action, time));
	}

	/// <summary>The coroutine implementation of Invoke.</summary>
	private static IEnumerator InvokeImplementation(Action action, float time)
	{
		yield return new WaitForSeconds(time);
		action();
	}

	/// <summary>Invokes an action after a given time, then repeatedly every repeateRate seconds.</summary>
	/// <param name="action">The action to invoke.</param>
	/// <param name="time">The time in seconds.</param>
	/// <param name="repeatRate">The repeat rate in seconds.</param>
	public static Coroutine InvokeRepeating(this MonoBehaviour monoBehaviour, Action action, float time, float repeatRate)
	{
		return monoBehaviour.StartCoroutine(InvokeRepeatingImplementation(action, time, repeatRate));
	}

	/// <summary>The coroutine implementation of InvokeRepeating.
	private static IEnumerator InvokeRepeatingImplementation(Action action, float time, float repeatRate)
	{
		yield return new WaitForSeconds(time);

		while(true)
		{
			action();
			yield return new WaitForSeconds(repeatRate);
		}
	}

	/// <summary>Cancel a given custom Invoke.</summary>
	/// <param name="coroutine">The Invoke's Coroutine.</param>
	public static void CancelInvoke(this MonoBehaviour monoBehaviour, Coroutine coroutine)
	{
		monoBehaviour.StopCoroutine(coroutine);
	}
}
