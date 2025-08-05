using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject lostPanel;

    [Header("Elements")]
    [SerializeField] private Slider pointSlider;
    [SerializeField] private TextMeshProUGUI pointsText;
    [SerializeField] private TextMeshProUGUI levelText;


    private void OnEnable()
    {
        CloseAllPanel();
    }

    public void UpdatePointUI(int currentPoint, int maxPoint)
    {
        pointsText.text = currentPoint + "/" + maxPoint;
        float pointSliderValue = (float)currentPoint / maxPoint;
        pointSlider.value = pointSliderValue;
    }

    public void UpdateLevelText(int level)
    {
        levelText.text = "LEVEL " + level;
    }

    public void EnablePausePanel(bool enable) => pausePanel.SetActive(enable);
    public void EnableWinPanel(bool enable) => winPanel.SetActive(enable);
    public void EnableLostPanel(bool enable) => lostPanel.SetActive(enable);

    public void CloseAllPanel()
    {
        EnablePausePanel(false);
        EnableWinPanel(false);
        EnableLostPanel(false);
    }
}
