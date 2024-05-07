using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Anything with a sprite sets their render position by default, and has the ability to be reset.
/// </summary>

public class SpriteViewHandler : MonoBehaviour
{
	[SerializeField] private SpriteRenderer spriteRenderer;
	[SerializeField] private Transform refTform;
	[SerializeField] private TextMeshProUGUI text;
	[SerializeField] private Offset offsetDirection;
	[SerializeField] private int amtOffset;

	public enum Offset
	{
		forward,
		backward
	}

	private void Start()
	{
		Initialize();
	}

	public void SetMaterial(Material mat)
	{
		spriteRenderer.material = mat;
		ResetRenderPos();
	}

	private void Initialize()
	{
		SetSortingPos();
	}

	public void ResetRenderPos()
	{
		SetSortingPos();
	}

	public void SetRenderPosition(Transform renderPos)
	{
		this.refTform = renderPos;
		SetSortingPos();
	}

	public void SetSortingPos()
	{
		if (spriteRenderer != null)
		{
			if (refTform == null)
			{
				refTform = this.transform;
			}
			int num = -Mathf.RoundToInt(refTform.position.y * 100);
			spriteRenderer.sortingOrder = num + (amtOffset * CalculateOffsetDir(offsetDirection));
		}

		if (text != null)
		{
			text.text = (-Mathf.RoundToInt(refTform.position.y * 100)).ToString();
		}
	}

	private int CalculateOffsetDir(Offset dir)
	{
		int returnVal = 0;
		if (dir == Offset.forward)
		{
			returnVal = 1;
		}
		else
		{
			returnVal = -1;
		}
		return returnVal;
	}
}