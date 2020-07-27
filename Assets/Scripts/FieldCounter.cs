using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class FieldCounter : MonoBehaviour
{
	public TextMeshProUGUI textPoints;
	public TextMeshProUGUI textRegulations;

	public int points = 0;
	private float seconds = 61f;
	private int moves = 30;

	public bool blockTimer = false;

	private void Awake()
	{
		blockTimer = false;
		GameManager.Instance.fieldCounter = this;
		Initialize();
	}

	private void Update()
	{
		if (GameManager.Instance.gameMode == GameManager.GameMode.TIMER &&
			GameManager.Instance.gameState != GameManager.GameState.WINSCREEN)
		{
			if (seconds > 0)
			{
				seconds -= Time.deltaTime;
				textRegulations.text = "Time: " + ((int)seconds).ToString();
			}
			else if (!blockTimer)
			{
				blockTimer = true;
				GameEnd();
			}
		}
	}

	public void Initialize()
	{
		textPoints.text = "Points: 0";

		switch (GameManager.Instance.gameMode)
		{
			case GameManager.GameMode.MOVES:
				textRegulations.text = "Moves: 30";
				break;
			case GameManager.GameMode.TIMER:
				textRegulations.text = "Time: 60";
				break;
			default:
				textRegulations.text = "";
				break;
		}
	}

	public void AddTimer(float addTimer)
	{
		seconds += addTimer;
	}

	public void UseMove(int useMove)
	{
		if (GameManager.Instance.gameMode != GameManager.GameMode.MOVES)
			return;

		moves -= useMove;
		textRegulations.text = "Moves: " + moves.ToString();

		if (moves <= 0)
		{
			GameEnd();
		}
	}

	public void AddPoints(int addPoints, bool square = false)
	{
		points += addPoints;

		if (square)
		{
			points += 5;
			textPoints.transform.DOPunchRotation(new Vector3(0f, 0.2f, 0f), 0.5f);
			textPoints.transform.DOPunchScale(new Vector3(0.5f, 0.5f, 0f), 0.5f);
		}

		textPoints.text = "Points: " + points.ToString();
	}

	public void GameEnd()
	{
		GameManager.Instance.gameState = GameManager.GameState.WINSCREEN;
		GameManager.Instance.fieldWinScreen.TurnOn();
	}
}
