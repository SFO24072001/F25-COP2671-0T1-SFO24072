using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    [SerializeField] private float speed = 30f;
    private PlayerController playerControllerScript;
    private Animator playerAnim;
    private float leftBound = -15f;

    private void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        playerAnim =  playerControllerScript.GetComponent<Animator>();
    }
    void Update()
    {
        if (playerControllerScript.gameOver == false)
            transform.Translate(Vector3.left * Time.deltaTime * speed);

        if (transform.position.x < leftBound && gameObject.CompareTag("Obstacle"))
            Destroy(gameObject);

        playerAnim.SetFloat("Speed_f", speed);
    }
}
