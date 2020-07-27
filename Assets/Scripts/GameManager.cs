using UnityEngine.SceneManagement;

public class GameManager
{
	private static readonly GameManager instance = new GameManager();

	public static GameManager Instance
	{
		get { return instance; }
	}

	protected GameManager() { }

	// ########
	public FieldDrag fieldDrag;
	public FieldVisuals fieldVisuals;
	public FieldSpawn fieldSpawn;
	public FieldCounter fieldCounter;
	public FieldBoosters fieldBoosters;
	public FieldWinScreen fieldWinScreen;

	public enum GameMode
	{
		MENU,
		TIMER,
		MOVES
	}

	public GameMode gameMode = GameMode.MENU;

	public enum GameState
	{
		IDLE,
		WINSCREEN,
		BOOSTERDESTROYONE,
		BOOSTERDESTROYCOLOR
	}

	public GameState gameState = GameState.IDLE;

	public void ChangeScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}
