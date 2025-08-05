using TMPro;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject iconLock;
    [SerializeField] private TextMeshPro lockValueText;
    [Header("Cell Settings")]
    [SerializeField] private int lockValue;
    [SerializeField] public bool isHaveStack;

    private string lockLayer = "Lock Grid";
    private int defaultLayer;

    public HexStack Stack { get; private set; }
    public bool IsLock { get; private set; }

    private void Start()
    {
        defaultLayer = gameObject.layer;
        if (isHaveStack)
        {
            HexStack hexStack = StrackSpawner.Instance.GetGenerateStack(transform);
            hexStack.Place();
            AssignStack(hexStack);
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance == null)
        {
            UpdateGridCell(0);
            return;
        }

        UpdateGridCell(GameManager.Instance.GetCurrentPoint());
    }

    public bool IsOccupied
    {
        get => Stack != null;
        private set { }
    }

    public void AssignStack(HexStack stack)
    {
        Stack = stack;
    }

    public void UpdateGridCell(int currentValue)
    {
        IsLock = currentValue >= lockValue ? false : true;

        iconLock.SetActive(IsLock);
        lockValueText.text = lockValue.ToString();
        gameObject.layer = IsLock ? LayerMask.NameToLayer(lockLayer) : defaultLayer;
    }
}
