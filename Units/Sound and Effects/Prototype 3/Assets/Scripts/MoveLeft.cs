using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float speed = 30f;
        
    void Update()
    {
        transform.Translate(Vector3.left * Time.deltaTime * speed);
    }
}
