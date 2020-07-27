using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;

public class Menu : MonoBehaviour
{
	public GameObject menuObject;
	public GameObject boostersObject;
	public TextMeshProUGUI textPointsBank;
	public TextMeshProUGUI textBoosterTimer;
	public TextMeshProUGUI textBoosterDestroyOne;
	public TextMeshProUGUI textBoosterDestroyColor;

	public void StartGame(string mode = "TIMER")
	{
		switch (mode)
		{
			case "TIMER":
				GameManager.Instance.gameMode = GameManager.GameMode.TIMER;
				GameManager.Instance.gameState = GameManager.GameState.IDLE;
				GameManager.Instance.ChangeScene("Game");
				break;
			case "MOVES":
				GameManager.Instance.gameMode = GameManager.GameMode.MOVES;
				GameManager.Instance.gameState = GameManager.GameState.IDLE;
				GameManager.Instance.ChangeScene("Game");
				break;
			default:
				break;
		}
	}

	public void TurnOnBoosters()
	{
		boostersObject.SetActive(true);
		boostersObject.transform.DOLocalMove(Vector3.zero, 1.5f);
		menuObject.transform.DOLocalMove(new Vector3(-2500f, 0f, 0f), 1f);

		SyncText();
	}

	public void TurnOffBoosters()
	{
		boostersObject.transform.DOLocalMove(new Vector3(2500f, 0f, 0f), 1.5f);
		menuObject.transform.DOLocalMove(Vector3.zero, 1f);
	}

	public void SyncText()
	{
		textPointsBank.text = SaveSystem.Instance.pointsBank.ToString();
		textBoosterTimer.text = SaveSystem.Instance.boosterTimer.ToString();
		textBoosterDestroyOne.text = SaveSystem.Instance.boosterDestroyOne.ToString();
		textBoosterDestroyColor.text = SaveSystem.Instance.boosterDestroyColor.ToString();
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
}
