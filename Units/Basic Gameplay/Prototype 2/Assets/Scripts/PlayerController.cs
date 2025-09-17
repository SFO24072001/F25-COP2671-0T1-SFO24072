using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float Horizontal = 0f;
    float speed = 10.0f;
    float xRange = 10f;

    void Update()
    {
        Horizontal = Input.GetAxis(nameof(Horizontal));
        transform.Translate(Vector3.right * Horizontal * Time.deltaTime * speed);
        if (transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }
        if (transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
    }
}
