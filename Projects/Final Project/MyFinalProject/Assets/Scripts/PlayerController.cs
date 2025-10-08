using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionReference _inputActionReference;
    [SerializeField] private float _playerSpeed = 5f;
    public Vector2 PlayerInput => _inputActionReference.action.ReadValue<Vector2>();

    private Vector2 _lastPlayerInput;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        _lastPlayerInput = PlayerInput.normalized;
    }
    private void FixedUpdate()
    {
        _rigidbody.linearVelocity = _lastPlayerInput * _playerSpeed;
    }
}
