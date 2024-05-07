using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Animation
{
	public static IEnumerator ShakeTransformHorizontal(float time, Transform transform, float scale)
	{
		float timer = time;
		Vector2 startPos = transform.localPosition;

		while (true)
		{
			timer -= Time.deltaTime;
			Vector2 position = startPos + Util.ShakeVector2Horizontal(scale);
			transform.localPosition = position;
			if (timer > 0)
			{
				yield return 0f;
			}
			else
			{
				transform.localPosition = startPos;
				yield break;
			}
		}
	}

	public static IEnumerator ShakeTransform2D(float time, Transform transform, float scale)
	{
		float timer = time;
		Vector2 startPos = transform.localPosition;

		while (true)
		{
			timer -= Time.deltaTime;
			Vector2 position = transform.localPosition;
			position += Util.ShakeVector2(scale);
			transform.localPosition = position;
			if (timer > 0)
			{
				yield return 0f;
			}
			else
			{
				transform.localPosition = startPos;
				yield break;
			}
		}
	}

	public static IEnumerator FadeImage(Image image, float target, float time, AnimationCurve curve)
	{
		float timer = time;
		Color startColor = image.color;

		while (true)
		{
			timer -= Time.deltaTime;

			image.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(startColor.a, target, curve.Evaluate(Util.MapValue(timer, time, 0, 0, 1))));
			if (image.color.a > target)
			{
				image.color = new Color(0, 0, 0, 0);
				yield break;
			}
			yield return 0f;
		}
	}

	public static IEnumerator FadeCanvasGroup(CanvasGroup group, float time, float startPos, float endPos, Action callback = null)
	{
		float timer = time;
		float simTime = 0;
		group.alpha = startPos;

		while (timer > 0)
		{
			timer -= Time.deltaTime;

			simTime = Util.MapValue(timer, time, 0, 0, 1);

			group.alpha = Util.MapValue(simTime, 0, 1, startPos, endPos);

			yield return 0f;
		}

		if (callback != null)
		{
			callback.Invoke();
		}
	}

	public static IEnumerator MoveAnchoredPosition(RectTransform t, Vector2 targetPos, float time = 1, AnimationCurve curve = null)
	{
		float timer = time;

		while (timer > 0)
		{
			timer -= Time.deltaTime;
			Vector2 currentPos = t.anchoredPosition;

			if (curve == null)
			{
				t.anchoredPosition = Vector2.Lerp(currentPos, targetPos, Util.MapValue(timer, time, 0, 0, 1));
			}
			else
			{
				t.anchoredPosition = Vector2.Lerp(currentPos, targetPos, curve.Evaluate(Util.MapValue(timer, time, 0, 0, 1)));
			}

			yield return 0f;
		}
	}
}