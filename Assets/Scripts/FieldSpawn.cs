using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FieldSpawn : MonoBehaviour
{
	public List<Transform> fieldColumns;

	public FieldHole[][] fieldHoles;

	public List<Color> dotsTypes;

	private void Awake()
	{
		GameManager.Instance.fieldSpawn = this;

		fieldHoles = new FieldHole[fieldColumns.Count][];

		for (int i = 0; i < fieldColumns.Count; i++)
		{
			fieldHoles[i] = new FieldHole[fieldColumns.Count];

			for (int j = 0; j < fieldColumns[i].childCount; j++)
			{
				FieldHole hole = fieldColumns[i].GetChild(j).GetComponent<FieldHole>();

				hole.column = i;
				hole.row = j;

				fieldHoles[i][j] = hole;
			}
		}

		FullSpawn();
	}

	public void ValidateField()
	{
		for (int i = 0; i < fieldHoles.Length; i++)
		{
			for (int j = 0; j < fieldHoles[i].Length; j++)
			{
				foreach (var neighbour in GetNeighbours(fieldHoles[i][j]))
				{
					if (fieldHoles[i][j].dotType == neighbour.dotType)
						return;
				}
			}
		}

		FullSpawn();
	}

	public void FullSpawn()
	{
		for (int i = 0; i < fieldHoles.Length; i++)
		{
			for (int j = 0; j < fieldHoles[i].Length; j++)
			{
				fieldHoles[i][j].dropPositions = -1;
				fieldHoles[i][j].DropPositions();
			}
		}

		ValidateField();
	}

	public void DotSpawn(FieldHole target)
	{
		int rnd = Random.Range(0, dotsTypes.Count);
		target.dotType = rnd;
		target.dotSprite.color = dotsTypes[rnd];
	}

	public void DotsHelper(bool state)
	{
		for (int i = 0; i < fieldHoles.Length; i++)
		{
			for (int j = 0; j < fieldHoles[i].Length; j++)
			{
				if (state)
					fieldHoles[i][j].HelperOn();
				else
					fieldHoles[i][j].HelperOff();
			}
		}
	}

	public bool IsNeighbours(FieldHole first, FieldHole second)
	{
		return GetNeighbours(first).Contains(second);
	}

	public List<FieldHole> GetByColor(int type)
	{
		List<FieldHole> result = new List<FieldHole>();

		for (int i = 0; i < fieldHoles.Length; i++)
		{
			for (int j = 0; j < fieldHoles[i].Length; j++)
			{
				if (fieldHoles[i][j].dotType == type)
					result.Add(fieldHoles[i][j]);
			}
		}

		return result;
	}

	public FieldHole GetByPos(int column, int row)
	{
		if (column >= 0 && column < fieldHoles.Length)
			if (row >= 0 && row < fieldHoles[column].Length)
				return fieldHoles[column][row];

		return null;
	}

	public List<FieldHole> GetNeighbours(FieldHole target)
	{
		List<FieldHole> result = new List<FieldHole>();

		// top
		if (target.row > 0)
			result.Add(fieldHoles[target.column][target.row - 1]);

		// right
		if (target.column < fieldHoles.Length - 1)
			result.Add(fieldHoles[target.column + 1][target.row]);

		// bottom
		if (target.row < fieldHoles[target.column].Length - 1)
			result.Add(fieldHoles[target.column][target.row + 1]);

		// left
		if (target.column > 0)
			result.Add(fieldHoles[target.column - 1][target.row]);

		return result;
	}
}
