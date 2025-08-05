using UnityEngine;
using UnityEngine.SceneManagement;

public enum State
{
    GAME,
    MENU,
}
public class GameManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameUI gameUI;
    [SerializeField] private MenuUI menuUI;

    [Header("Level settings")]
    [SerializeField] private int maxPoint = 200;
    [SerializeField] private int currentPoint = 0;

    public int maxLevel;
    public int currentLevelUnlock;
    private int currentLevel = 0;

    [Space]
    public State currentState;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        currentLevelUnlock = PlayerPrefs.GetInt("Level_Unlock", 1);
        maxLevel = SceneManager.sceneCountInBuildSettings;

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        SwitchToMenu();
        // SwitchToGame();
    }

    public void AdjustPoint(int adjustPoint)
    {
        int pointToAdd = Mathf.Min(adjustPoint, maxPoint - currentPoint);
        currentPoint += pointToAdd;
        gameUI.UpdatePointUI(currentPoint, maxPoint);
        if (currentPoint >= maxPoint)
            WinLevel();
    }

    public int GetCurrentPoint() => currentPoint;

    private void WinLevel()
    {
        gameUI.EnableWinPanel(true);
        if (currentLevelUnlock >= maxLevel - 1)
            return;
        currentLevelUnlock++;
        PlayerPrefs.SetInt("Level_Unlock", currentLevelUnlock);
    }

    public void LostLevel()
    {
        gameUI.EnableLostPanel(true);
    }

    public void SwitchToMenu()
    {
        menuUI.gameObject.SetActive(true);
        gameUI.gameObject.SetActive(false);
        currentState = State.MENU;
        StrackSpawner.Instance.StopGenarateStacks();
        SceneManager.UnloadSceneAsync(currentLevel);
        currentLevel = 0;
    }

    public void SwitchToGame()
    {
        LoadLevel(currentLevelUnlock);
    }

    public void LoadLevel(int level)
    {
        menuUI.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(true);
        gameUI.CloseAllPanel();
        StrackSpawner.Instance.StopGenarateStacks();
        currentState = State.GAME;

        if (currentLevel != 0)
            SceneManager.UnloadSceneAsync(currentLevel);
        currentLevel = level;
        currentPoint = 0;
        gameUI.UpdateLevelText(level);
        SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
    }

    public void ReloadLevel()
    {
        LoadLevel(currentLevel);
    }

    public void LoadNextLevel()
    {
        int nextLevel = currentLevelUnlock >= maxLevel - 1 ? currentLevel : currentLevel + 1;
        LoadLevel(nextLevel);
    }

    public void StartLevel(Vector2Int hexCount, Color[] colors, int newMaxPoint)
    {
        StrackSpawner.Instance.StartGenarateStacks(hexCount, colors);
        maxPoint = newMaxPoint;
        gameUI.UpdatePointUI(currentPoint, maxPoint);
    }

}