using UnityEngine;

public class MoveForward : MonoBehaviour
{
    public float speed = 40.0f;

    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
