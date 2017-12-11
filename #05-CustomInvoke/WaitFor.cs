/*
 *	Written by James Leahy. (c) 2017 DeFunc Art.
 *	https://github.com/defuncart/
 */
using System.Collections.Generic;
using UnityEngine;

/// <summary>Included in the DeFuncArt.Utilities namespace.</summary>
namespace DeFuncArt.Utilities
{
	/// <summary>A static class which contains cached values of WaitForSeconds, WaitForEndOfFrame, WaitForFixedUpdate.</summary>
	public static class WaitFor
	{
		/// <summary>A Float comparer used in the waitForSecondsDictionary.</summary>
		private class FloatComparer : IEqualityComparer<float>
		{
			bool IEqualityComparer<float>.Equals(float x, float y)
			{
				return x == y;
			}
			int IEqualityComparer<float>.GetHashCode(float obj)
			{
				return obj.GetHashCode();
			}
		}
		/// <summary>A dictionary of WaitForSeconds whose keys are the wait time.</summary>
		private static Dictionary<float, WaitForSeconds> waitForSecondsDictionary = new Dictionary<float, WaitForSeconds>(0, new FloatComparer());
		/// <summary>Suspends the coroutine execution for the given amount of seconds using scaled time.</summary>
		public static WaitForSeconds Seconds(float seconds)
		{
			//test if a WaitForSeconds with this wait time exists - if not, create one
			WaitForSeconds waitForSeconds;
			if(!waitForSecondsDictionary.TryGetValue(seconds, out waitForSeconds))
			{
				waitForSecondsDictionary.Add(seconds, waitForSeconds = new WaitForSeconds(seconds));
			}
			return waitForSeconds;
		}

		/// <summary>A backing variable for FixedUpdate.</summary>
		static WaitForFixedUpdate _FixedUpdate;
		/// <summary>Waits until next fixed frame rate update function.</summary>
		public static WaitForFixedUpdate FixedUpdate
		{
			get{ return _FixedUpdate ?? (_FixedUpdate = new WaitForFixedUpdate()); }
		}

		/// <summary>A backing variable for EndOfFrame.</summary>
		private static WaitForEndOfFrame _EndOfFrame;
		/// <summary>Waits until the end of the frame after all cameras and GUI is rendered, just before displaying the frame on screen.</summary>
		public static WaitForEndOfFrame EndOfFrame
		{
			get{ return _EndOfFrame ?? (_EndOfFrame = new WaitForEndOfFrame()); }
		}
	}
}
