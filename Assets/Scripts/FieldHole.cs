using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FieldHole : MonoBehaviour
{
	public int dotType = 0;
	public Image dotSprite;
	public Image dotHelper;
	public Transform droppableObject;

	public int column = 0;
	public int row = 0;

	public int dropPositions = 0;

	public void DropPositions()
	{
		if (dropPositions == -1)
		{
			GameManager.Instance.fieldSpawn.DotSpawn(this);

			droppableObject.position = transform.position + new Vector3(0f, 10f, 0f);
			droppableObject.DOLocalMoveY(0, 0.75f).SetEase(Ease.OutBounce);
			dropPositions = 0;
		}
		else if (dropPositions > 0)
		{
			FieldHole target = GameManager.Instance.fieldSpawn.GetByPos(column, row - dropPositions);

			if (target == null)
			{
				dropPositions = -1;
				DropPositions();
			}
			else
			{
				dotType = target.dotType;
				dotSprite.color = target.dotSprite.color;

				droppableObject.position = target.transform.position;
				droppableObject.DOLocalMoveY(0, 0.25f * dropPositions).SetEase(Ease.OutBounce);
				dropPositions = 0;
			}
		}
	}

	public void HelperFlash()
	{
		dotHelper.color = dotSprite.color;

		dotHelper.transform.localScale = Vector3.one;
		dotHelper.transform.DOScale(Vector3.one * 1.1f, 0.25f);
		dotHelper.DOFade(0f, 0.25f);
	}

	public void HelperOn()
	{
		dotHelper.color = dotSprite.color;

		dotHelper.transform.localScale = Vector3.one;
		dotHelper.transform.DOScale(Vector3.one * 1.1f, 0.25f);
		dotHelper.DOFade(0.5f, 0.25f);
	}

	public void HelperOff(bool outwards = false)
	{
		dotHelper.color = dotSprite.color;

		if (outwards)
			dotHelper.transform.DOScale(Vector3.one * 2f, 0.25f);
		else
			dotHelper.transform.DOScale(Vector3.zero, 0.25f);

		dotHelper.DOFade(0f, 0.25f);
	}

	public void OnPointerDown()
	{
		GameManager.Instance.fieldDrag.pointerDown = true;
		GameManager.Instance.fieldBoosters.CheckBoosters(this);
	}

	public void OnPointerEnter()
	{
		GameManager.Instance.fieldDrag.pointerIn = true;

		if (GameManager.Instance.fieldDrag.pointerDown)
			GameManager.Instance.fieldBoosters.CheckBoosters(this);
	}

	public void OnPointerExit()
	{
		GameManager.Instance.fieldDrag.pointerIn = false;
	}

	public void OnPointerUp()
	{
		GameManager.Instance.fieldDrag.pointerDown = false;
		GameManager.Instance.fieldDrag.ValidateLine();
	}

}
