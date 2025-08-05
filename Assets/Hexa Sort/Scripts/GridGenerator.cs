using NaughtyAttributes;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Grid grid;
    [SerializeField] private GameObject hexagon;

    [Header("Settings")]
    [OnValueChanged(nameof(GenerateGrid))]
    [SerializeField] private int girdSize;

    private void GenerateGrid()
    {
        while (transform.childCount > 0)
        {
            Transform child = transform.transform.GetChild(0);
            child.SetParent(null);
            Object.DestroyImmediate(child.gameObject);
        }

        for (int x = -girdSize; x <= girdSize; x++)
        {
            for (int y = -girdSize; y <= girdSize; y++)
            {
                Vector3 spawnPos = grid.CellToWorld(new Vector3Int(x, y, 0));

                if (spawnPos.magnitude > grid.CellToWorld(new Vector3Int(1, 0, 0)).magnitude * girdSize)
                    continue;

                Instantiate(hexagon, spawnPos, Quaternion.identity, transform);
            }
        }
    }
}
