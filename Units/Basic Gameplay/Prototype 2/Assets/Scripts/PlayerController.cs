using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    GameObject projectilePrefab;
    float Horizontal = 0f;
    float speed = 20.0f;
    float xRange = 17f;

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

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            Instantiate(projectilePrefab, transform.position, projectilePrefab.transform.rotation);
        }
    }
}
