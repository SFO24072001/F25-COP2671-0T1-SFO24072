using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    [SerializeField] private float _playerMovementSpeed = 1f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody.linearVelocity = _playerMovementSpeed * Time.deltaTime * Vector2.right;
    }
}
