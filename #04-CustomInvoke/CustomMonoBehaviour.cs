/*
 * 	Written by James Leahy (c) 2017 DeFunc Art.
 */ 
using System;
using System.Collections;
using UnityEngine;

/// <summary>A custom base class.</summary>
public class CustomMonoBehaviour : MonoBehaviour
{
	/// <summary>Invokes an action after a given time.</summary>
	/// <param name="action">The action to invoke.</param>
	/// <param name="time">The time in seconds.</param>
	public Coroutine Invoke(Action action, float time)
	{
		return StartCoroutine(InvokeImplementation(action, time));
	}

	/// <summary>The coroutine implementation of Invoke.</summary>
	private IEnumerator InvokeImplementation(Action action, float time)
	{
		yield return new WaitForSeconds(time);
		action();
	}

	/// <summary>Invokes an action after a given time, then repeatedly every repeateRate seconds.</summary>
	/// <param name="action">The action to invoke.</param>
	/// <param name="time">The time in seconds.</param>
	/// <param name="repeatRate">The repeat rate in seconds.</param>
	public Coroutine InvokeRepeating(Action action, float time, float repeatRate)
	{
		return StartCoroutine(InvokeRepeatingImplementation(action, time, repeatRate));
	}

	/// <summary>The coroutine implementation of InvokeRepeating.
	private IEnumerator InvokeRepeatingImplementation(Action action, float time, float repeatRate)
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
	public void CancelInvoke(Coroutine coroutine)
	{
		StopCoroutine(coroutine);
	}
}
