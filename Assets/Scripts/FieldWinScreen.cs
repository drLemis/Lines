using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class FieldWinScreen : MonoBehaviour
{
	public GameObject winScreenObject;
	public GameObject fieldObject;
	public TextMeshProUGUI textPointsCurrent;
	public TextMeshProUGUI textPointsMax;
	public TextMeshProUGUI textPointsBank;
	public TextMeshProUGUI textBoosterTimer;
	public TextMeshProUGUI textBoosterDestroyOne;
	public TextMeshProUGUI textBoosterDestroyColor;

	private void Awake()
	{
		GameManager.Instance.fieldWinScreen = this;
	}

	public void SyncText()
	{
		textPointsCurrent.text = GameManager.Instance.fieldCounter.points.ToString();
		textPointsMax.text = SaveSystem.Instance.pointsMax.ToString();
		textPointsBank.text = SaveSystem.Instance.pointsBank.ToString();

		textBoosterTimer.text = SaveSystem.Instance.boosterTimer.ToString();
		textBoosterDestroyOne.text = SaveSystem.Instance.boosterDestroyOne.ToString();
		textBoosterDestroyColor.text = SaveSystem.Instance.boosterDestroyColor.ToString();
	}

	public void TurnOn()
	{
		SaveSystem.Instance.pointsBank += GameManager.Instance.fieldCounter.points;

		if (GameManager.Instance.fieldCounter.points > SaveSystem.Instance.pointsMax)
			SaveSystem.Instance.pointsMax = GameManager.Instance.fieldCounter.points;

		winScreenObject.SetActive(true);
		winScreenObject.transform.DOLocalMove(Vector3.zero, 1f).SetDelay(0.5f);
		fieldObject.transform.DOLocalMove(new Vector3(-2500f, 0f, 0f), 1f).SetDelay(0.5f);

		SyncText();
		SaveSystem.Instance.Save();
	}

	public void BuyBooster(string boosterType)
	{
		switch (boosterType)
		{
			case "TIMER":
				if (SaveSystem.Instance.pointsBank >= 1000)
				{
					SaveSystem.Instance.pointsBank -= 1000;
					SaveSystem.Instance.boosterTimer += 5;
				}
				break;
			case "DESTROYONE":
				if (SaveSystem.Instance.pointsBank >= 500)
				{
					SaveSystem.Instance.pointsBank -= 500;
					SaveSystem.Instance.boosterDestroyOne += 5;
				}
				break;
			case "DESTROYCOLOR":
				if (SaveSystem.Instance.pointsBank >= 5000)
				{
					SaveSystem.Instance.pointsBank -= 5000;
					SaveSystem.Instance.boosterDestroyColor += 5;
				}
				break;
			default:
				break;
		}

		SyncText();
		SaveSystem.Instance.Save();
	}

	public void RestartGame()
	{
		GameManager.Instance.gameState = GameManager.GameState.IDLE;
		GameManager.Instance.ChangeScene("Game");
	}

	public void EndGame()
	{
		GameManager.Instance.ChangeScene("Menu");
	}
}
