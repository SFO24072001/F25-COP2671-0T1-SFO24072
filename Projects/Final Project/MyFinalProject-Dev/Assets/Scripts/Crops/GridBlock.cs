using UnityEngine;

public class GridBlock : MonoBehaviour
{   
    public Vector2Int Location { get; protected set; }
    public bool IsUsable { get; protected set; } = true;

    public void Initialize(Vector2Int location)
    {
        Location = location;
        name = $"GridBlock [{location.x},{location.y}]";
    }
    public void PreventUse() => IsUsable = false;
}
