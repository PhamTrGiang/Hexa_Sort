using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StrackSpawner : MonoBehaviour
{
    [Header("Elemetns")]
    [SerializeField] private Transform stackPositionsParent;
    [SerializeField] private Hexagon hexagonPrefab;
    [SerializeField] private HexStack hexagonStackPrefab;

    private Vector2Int minMaxHexCount;
    private Color[] colors;

    private bool thirdColor;
    private int stackCounter;

    public static StrackSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        StackController.onStackPlaced += StackPlacedCallback;

    }

    private void OnDestroy()
    {
        StackController.onStackPlaced -= StackPlacedCallback;
    }

    private void StackPlacedCallback(GridCell gridCell)
    {
        stackCounter++;

        if (stackCounter >= 3)
        {
            stackCounter = 0;
            GenerateStacks();
        }

    }

    public void StartGenarateStacks(Vector2Int newHexCount, Color[] newColors)
    {
        minMaxHexCount = newHexCount;
        colors = newColors;
        stackCounter = 0;
        GenerateStacks();
    }

    public void StopGenarateStacks()
    {
        for (int i = 0; i < stackPositionsParent.childCount; i++)
        {
            while (stackPositionsParent.GetChild(i).childCount > 0)
            {
                Transform child = stackPositionsParent.GetChild(i).GetChild(0);
                child.SetParent(null);
                Destroy(child.gameObject);
            }
        }
    }

    private void GenerateStacks()
    {
        for (int i = 0; i < stackPositionsParent.childCount; i++)
            GenerateStack(stackPositionsParent.GetChild(i), thirdColor);

    }

    private void GenerateStack(Transform parent, bool thirdColor = false)
    {
        HexStack hexStack = GetGenerateStack(parent, thirdColor);
    }

    public HexStack GetGenerateStack(Transform parent, bool thirdColor = false)
    {
        HexStack hexStack = Instantiate(hexagonStackPrefab, parent.position, Quaternion.identity, parent);
        hexStack.name = $"Stack {parent.GetSiblingIndex()}";

        int amount = Random.Range(minMaxHexCount.x, minMaxHexCount.y);
        int firstColorHexagonCount = Random.Range(0, amount);
        //int secondColorHexagonCount = Random.Range(firstColorHexagonCount, amount);

        Color[] colorArray = GetRandomColors(thirdColor);

        for (int i = 0; i < amount; i++)
        {
            Vector3 hexagonLocalPos = Vector3.up * i * .1f;
            Vector3 spawnPosition = hexStack.transform.TransformPoint(hexagonLocalPos);

            Hexagon hexagonInstance = Instantiate(hexagonPrefab, spawnPosition, Quaternion.identity, hexStack.transform);

            // Color newColor;
            // if (i < firstColorHexagonCount)
            //     newColor = colorArray[0];
            // else if (firstColorHexagonCount <= i && i < secondColorHexagonCount)
            //     newColor = colorArray[1];
            // else
            //     newColor = colorArray[2];
            // hexagonInstance.Color = newColor;

            hexagonInstance.Color = i < firstColorHexagonCount ? colorArray[0] : colorArray[1];
            hexagonInstance.Configure(hexStack);

            hexStack.Add(hexagonInstance);
        }
        return hexStack;
    }

    private Color[] GetRandomColors(bool needThird)
    {
        List<Color> colorList = new List<Color>();
        colorList.AddRange(colors);

        if (colorList.Count <= 0)
        {
            Debug.Log("No color found");
            return null;
        }

        Color firstColor = colorList.OrderBy(x => Random.value).First();
        colorList.Remove(firstColor);

        if (colorList.Count <= 0)
        {
            Debug.Log("Only one color was found");
            return null;
        }

        Color secondColor = colorList.OrderBy(x => Random.value).First();
        colorList.Remove(secondColor);
        if (!needThird)
            return new Color[] { firstColor, secondColor };

        if (colorList.Count <= 0)
        {
            Debug.Log("Only two color was found");
            return null;
        }
        Color thirdColor = colorList.OrderBy(x => Random.value).First();
        colorList.Remove(thirdColor);

        return new Color[] { firstColor, secondColor, thirdColor };
    }
}
