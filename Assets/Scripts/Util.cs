using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
	public static float MapValue(float a, float min, float max, float _min, float _max)
	{
		return (a - min) * (_max - _min) / (max - min) + _min;
	}

	public static Vector2 ShakeVector2(float amt)
	{
		Vector2 v = UnityEngine.Random.insideUnitCircle * amt;
		return v;
	}
	public static Vector2 ShakeVector2Horizontal(float amt)
	{
		Vector2 v2 = new Vector2(UnityEngine.Random.insideUnitCircle.x, 0);
		return v2 * amt;
	}

	public static IEnumerator WaitAndCallRoutine(float time, Action callback)
	{
		yield return new WaitForSeconds(time);
		if (callback != null)
		{
			callback.Invoke();
		}
	}
}