using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FieldVisuals : MonoBehaviour
{
	public List<Image> borderImage;
	public Image flashImage;

	private void Awake()
	{
		GameManager.Instance.fieldVisuals = this;
	}

	public void ProcessVisuals(int dotsInLine, bool loop, Color color)
	{
		foreach (var border in borderImage)
		{
			if (dotsInLine > 0)
				border.color = color;

			border.fillAmount = Mathf.Min(dotsInLine / 10f, 1f);
		}

		flashImage.gameObject.SetActive(loop);

		if (loop)
		{
			Color tempColor = color;
			tempColor.a = 0.15f;
			flashImage.color = tempColor;
		}
	}
}
