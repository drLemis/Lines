using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FieldBoosters : MonoBehaviour
{
	public TextMeshProUGUI textTimer;
	public TextMeshProUGUI textDestroyOne;
	public TextMeshProUGUI textDestroyColor;

	public TextMeshProUGUI textDestroyOneHelp;
	public TextMeshProUGUI textDestroyColorHelp;

	private void Awake()
	{
		GameManager.Instance.fieldBoosters = this;
		SyncText();
	}

	public void SyncText()
	{
		textTimer.text = SaveSystem.Instance.boosterTimer.ToString();
		textDestroyOne.text = SaveSystem.Instance.boosterDestroyOne.ToString();
		textDestroyColor.text = SaveSystem.Instance.boosterDestroyColor.ToString();
	}

	public void CheckBoosters(FieldHole fieldHole)
	{
		switch (GameManager.Instance.gameState)
		{
			case GameManager.GameState.BOOSTERDESTROYONE:
				UseDestroyOne(fieldHole);
				break;
			case GameManager.GameState.BOOSTERDESTROYCOLOR:
				UseDestroyColor(fieldHole);
				break;
			default:
				GameManager.Instance.fieldDrag.ProcessPoint(fieldHole);
				break;
		}
	}

	public void UseTimer()
	{
		if (SaveSystem.Instance.boosterTimer > 0)
		{
			SaveSystem.Instance.boosterTimer--;
			GameManager.Instance.fieldCounter.UseMove(-5);
			GameManager.Instance.fieldCounter.AddTimer(5f);
			SyncText();
		}
	}

	public void TurnDestroyOne()
	{
		if (GameManager.Instance.gameState == GameManager.GameState.BOOSTERDESTROYONE)
		{
			textDestroyOneHelp.gameObject.SetActive(false);
			GameManager.Instance.gameState = GameManager.GameState.IDLE;
			GameManager.Instance.fieldSpawn.DotsHelper(false);
		}
		else if (SaveSystem.Instance.boosterDestroyOne > 0)
		{
			textDestroyOneHelp.gameObject.SetActive(true);
			GameManager.Instance.gameState = GameManager.GameState.BOOSTERDESTROYONE;
			GameManager.Instance.fieldSpawn.DotsHelper(true);
		}
	}

	public void UseDestroyOne(FieldHole fieldHole)
	{
		if (SaveSystem.Instance.boosterDestroyOne > 0)
		{
			textDestroyOneHelp.gameObject.SetActive(false);
			SaveSystem.Instance.boosterDestroyOne--;
			GameManager.Instance.fieldDrag.activeLine.Add(fieldHole);
			GameManager.Instance.fieldDrag.ValidateLine(true);
			SyncText();
			GameManager.Instance.gameState = GameManager.GameState.IDLE;

			GameManager.Instance.fieldSpawn.DotsHelper(false);
		}
	}

	public void TurnDestroyColor()
	{
		if (GameManager.Instance.gameState == GameManager.GameState.BOOSTERDESTROYCOLOR)
		{
			textDestroyColorHelp.gameObject.SetActive(false);
			GameManager.Instance.gameState = GameManager.GameState.IDLE;
			GameManager.Instance.fieldSpawn.DotsHelper(false);
		}
		else if (SaveSystem.Instance.boosterDestroyColor > 0)
		{
			textDestroyColorHelp.gameObject.SetActive(true);
			GameManager.Instance.gameState = GameManager.GameState.BOOSTERDESTROYCOLOR;
			GameManager.Instance.fieldSpawn.DotsHelper(true);
		}
	}

	public void UseDestroyColor(FieldHole fieldHole, bool doNotSpend = false)
	{
		if (SaveSystem.Instance.boosterDestroyColor > 0 || doNotSpend)
		{
			if (!doNotSpend)
				SaveSystem.Instance.boosterDestroyColor--;

			textDestroyColorHelp.gameObject.SetActive(false);
			GameManager.Instance.fieldDrag.activeLine.Clear();

			foreach (var hole in GameManager.Instance.fieldSpawn.GetByColor(fieldHole.dotType))
			{
				GameManager.Instance.fieldDrag.activeLine.Add(hole);
			}

			GameManager.Instance.fieldDrag.ValidateLine(true);
			SyncText();
			GameManager.Instance.gameState = GameManager.GameState.IDLE;

			GameManager.Instance.fieldSpawn.DotsHelper(false);
		}
	}
}
