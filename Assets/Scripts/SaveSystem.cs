using UnityEngine;

public class SaveSystem
{
	private static SaveSystem instance;

	public static SaveSystem Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new SaveSystem();
				instance.Load();
			}
			return instance;
		}
	}

	protected SaveSystem() { }

	// ########
	public int pointsBank = 0;
	public int pointsMax = 0;
	public int boosterTimer = 5;
	public int boosterDestroyOne = 5;
	public int boosterDestroyColor = 5;

	void OnApplicationQuit()
	{
		PlayerPrefs.Save();
	}

	public void Save()
	{
		PlayerPrefs.SetInt("PointsBank", pointsBank);
		PlayerPrefs.SetInt("PointsMax", pointsMax);
		PlayerPrefs.SetInt("BoosterTimer", boosterTimer);
		PlayerPrefs.SetInt("BoosterDestroyOne", boosterDestroyOne);
		PlayerPrefs.SetInt("BoosterDestroyColor", boosterDestroyColor);
		PlayerPrefs.Save();
	}

	public void Load()
	{
		pointsBank = PlayerPrefs.GetInt("PointsBank", 0);
		pointsMax = PlayerPrefs.GetInt("PointsMax", 0);
		boosterTimer = PlayerPrefs.GetInt("BoosterTimer", 5);
		boosterDestroyOne = PlayerPrefs.GetInt("BoosterDestroyOne", 5);
		boosterDestroyColor = PlayerPrefs.GetInt("BoosterDestroyColor", 5);
	}
}
