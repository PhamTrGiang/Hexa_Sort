using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjectsToDestroy;

    [Header("Level Settings")]
    [NaughtyAttributes.MinMaxSlider(2, 8)]
    [SerializeField] private Vector2Int minMaxHexCount;
    [SerializeField] private Color[] colors;
    [SerializeField] private int maxPoint;

    private GridGenerator gridGenerator;
    private GridCell[] allGridCell;

    private void Awake()
    {
        if (GameManager.Instance == null)
            return;

        if (gameObjectsToDestroy.Length > 0)
            foreach (GameObject obj in gameObjectsToDestroy)
                Destroy(obj);

        gridGenerator = FindFirstObjectByType<GridGenerator>();
        allGridCell = gridGenerator.GetComponentsInChildren<GridCell>();

        GameManager.Instance.StartLevel(minMaxHexCount, colors, maxPoint);

        StackController.onStackPlaced += StackPlacedCallback;

    }

    private void OnDestroy()
    {
        StackController.onStackPlaced -= StackPlacedCallback;
    }

    private void StackPlacedCallback(GridCell cell)
    {
        if (CheckLost())
            LeanTween.delayedCall(1, DelayCall);
    }

    private void DelayCall()
    {
        if (CheckLost())
            GameManager.Instance.LostLevel();
    }

    private bool CheckLost()
    {
        foreach (GridCell gridCell in allGridCell)
        {
            if (!gridCell.IsOccupied && !gridCell.IsLock)
                return false;
        }
        return true;
    }


    private void Start()
    {
        StartCoroutine(CheckMerge());
    }

    private IEnumerator CheckMerge()
    {
        foreach (GridCell gridCell in allGridCell)
        {
            if (!gridCell.isHaveStack || gridCell.IsLock)
                continue;

            yield return MergeManager.Instance.StackPlacedCoroutine(gridCell);
        }
    }

    public Vector2Int GetHexCount() => minMaxHexCount;
    public Color[] GetColors() => colors;
}
