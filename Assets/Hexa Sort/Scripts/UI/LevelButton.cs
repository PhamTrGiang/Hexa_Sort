using TMPro;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject iconLock;

    private int level;

    private bool isLock = true;

    private void Awake()
    {
        level = transform.GetSiblingIndex() + 1;
    }

    private void OnEnable()
    {
        CheckLock();
    }

    private void CheckLock()
    {
        int levelUnlock = PlayerPrefs.GetInt("Level_Unlock", 1);
        if (level <= levelUnlock)
            isLock = false;
        UpdateUI(isLock);
    }

    public void PlayLevel()
    {
        if (isLock)
            return;
        GameManager.Instance.LoadLevel(level);
    }

    private void UpdateUI(bool status)
    {
        levelText.text = level.ToString();
        iconLock.SetActive(status);
    }
}
