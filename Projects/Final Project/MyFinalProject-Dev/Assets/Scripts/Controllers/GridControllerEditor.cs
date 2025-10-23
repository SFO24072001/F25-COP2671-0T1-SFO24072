using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridController))]
public class GridControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridController grid = (GridController)target;

        if (GUILayout.Button("Log Crop Grid"))
        {
            if (grid.GridBlocks != null)
            {
                for (int y = 0; y < grid.GridSize.y; y++)
                {
                    string row = "";
                    for (int x = 0; x < grid.GridSize.x; x++)
                    {
                        var block = grid.GridBlocks[x, y];
                        row += block == null ? "[ ]" : block.IsUsable ? "[O]" : "[X]";
                    }
                    Debug.Log(row);
                }
            }
        }
    }
}
