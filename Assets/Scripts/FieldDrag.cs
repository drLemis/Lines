using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldDrag : MonoBehaviour
{
	// FieldHole.OnPointerDown() -> FieldBoosters.CheckBoosters() -> FieldDrag.ProcessPoint() ->
	// FieldDrag.ValidateLine() -> FieldDrag.DropField() -> FieldHole.DropPositions()

	public bool pointerIn;
	public bool pointerDown;
	public LineRenderer lineRenderer;
	public List<FieldHole> activeLine;

	private bool activeSquare = false;

	private void Awake()
	{
		GameManager.Instance.fieldDrag = this;
	}

	private void Update()
	{
		DrawLine();
	}

	public void ProcessPoint(FieldHole newHole)
	{
		if (GameManager.Instance.gameState == GameManager.GameState.WINSCREEN)
		{
			activeLine.Clear();
			return;
		}

		Color lineColor = newHole.dotSprite.color;

		if (activeLine.Count > 0)
		{
			lineColor = activeLine[0].dotSprite.color;
			FieldHole lastHole = activeLine[activeLine.Count - 1];

			if (newHole == lastHole && activeLine.Count > 1)
			{
				activeLine.RemoveAt(activeLine.Count - 1);
				newHole.HelperOff();
				GameManager.Instance.fieldVisuals.ProcessVisuals(activeLine.Count, activeSquare, lineColor);
				return;
			}

			// same color
			if (lastHole.dotType == newHole.dotType)
			{
				// neighbouring dots
				if (GameManager.Instance.fieldSpawn.IsNeighbours(lastHole, newHole))
				{
					// was already connected by same line
					if (activeLine.Count > 1 && activeLine.Contains(newHole))
					{
						for (int i = 0; i < activeLine.Count; i++)
						{
							if (activeLine[i] == newHole)
							{
								if (i > 0 && activeLine[i - 1] == lastHole)
									return;

								if (i < activeLine.Count - 1 && activeLine[i + 1] == lastHole)
									return;
							}
						}
					}
					activeLine.Add(newHole);
					newHole.HelperOn();
				}
			}
		}
		else
		{
			activeLine.Add(newHole);
			newHole.HelperOn();
		}

		ProcessLineForLoop();

		GameManager.Instance.fieldVisuals.ProcessVisuals(activeLine.Count, activeSquare, lineColor);
	}

	public void ProcessLineForLoop()
	{
		activeSquare = false;

		foreach (var hole in activeLine)
		{
			if (activeLine.Count > 3)
			{
				List<FieldHole> results = activeLine.FindAll(s => s.Equals(hole));
				if (results.Count > 1)
				{
					activeSquare = true;
					return;
				}
			}
		}
	}

	public void DrawLine()
	{
		if (GameManager.Instance.gameState == GameManager.GameState.WINSCREEN || activeLine.Count == 0)
		{
			activeLine.Clear();
			lineRenderer.positionCount = 0;
			return;
		}

		List<Vector3> positions = new List<Vector3>();

		Vector3 cursorPosition = Input.mousePosition;

		if (Input.touchCount > 0)
			cursorPosition = Input.GetTouch(0).position;

		foreach (var point in activeLine)
		{
			positions.Add(point.transform.position - new Vector3(0, 0, point.transform.position.z));
		}

		cursorPosition = Camera.main.ScreenToWorldPoint(cursorPosition);

		cursorPosition.z = positions[0].z;

		positions.Add(cursorPosition);

		lineRenderer.positionCount = positions.Count;

		lineRenderer.startColor = lineRenderer.endColor = activeLine[0].dotSprite.color;

		lineRenderer.SetPositions(positions.ToArray());
	}

	public void ValidateLine(bool boosterMode = false)
	{
		lineRenderer.positionCount = 0;

		GameManager.Instance.fieldVisuals.ProcessVisuals(0, false, Color.white);

		bool dotHelped = false;

		if (activeLine.Count > 1 || (activeLine.Count > 0 && boosterMode))
		{
			if (activeSquare)
			{
				if (!boosterMode)
					GameManager.Instance.fieldCounter.UseMove(1);
				activeSquare = false;
				GameManager.Instance.fieldBoosters.UseDestroyColor(activeLine[0], true);
				return;
			}

			GameManager.Instance.fieldCounter.AddPoints(activeLine.Count);

			if (!boosterMode)
				GameManager.Instance.fieldCounter.UseMove(1);

			activeLine.Sort(delegate (FieldHole a, FieldHole b) { return (b.row).CompareTo(a.row); });

			foreach (var hole in activeLine)
			{
				hole.dropPositions++;

				for (int i = hole.row - 1; i >= 0; i--)
				{
					GameManager.Instance.fieldSpawn.GetByPos(hole.column, i).dropPositions++;
				}

				for (int i = 1; i < hole.dropPositions; i++)
				{
					FieldHole subdropHole = GameManager.Instance.fieldSpawn.GetByPos(hole.column, hole.row + i);

					if (subdropHole != null && subdropHole.dropPositions > 0)
					{
						subdropHole.dropPositions++;
					}
				}
			}

			dotHelped = true;
			foreach (var dot in activeLine)
			{
				dot.HelperFlash();
			}

			DropField();

			GameManager.Instance.fieldSpawn.ValidateField();
		}

		if (!dotHelped)
			foreach (var dot in activeLine)
			{
				dot.HelperOff();
			}

		activeLine.Clear();

		GameManager.Instance.gameState = GameManager.GameState.IDLE;
	}

	private void DropField()
	{
		for (int i = 0; i < GameManager.Instance.fieldSpawn.fieldHoles.Length; i++)
		{
			for (int j = GameManager.Instance.fieldSpawn.fieldHoles[i].Length - 1; j >= 0; j--)
			{
				GameManager.Instance.fieldSpawn.fieldHoles[i][j].DropPositions();
			}
		}

	}
}
