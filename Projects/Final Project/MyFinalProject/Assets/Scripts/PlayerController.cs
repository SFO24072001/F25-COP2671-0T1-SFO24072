using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float _playerSpeed = 1f;

    private Rigidbody2D _rigidbody2D;
    private Vector2 _movement;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = _movement * _playerSpeed;
    }
}
