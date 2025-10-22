using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController_Dev : MonoBehaviour
{
    [SerializeField] private InputActionReference _inputActionReference;
    [SerializeField] private float _moveSpeed = 5f;
    public Vector2 PlayerInput
    {
        get
        {
            return _inputActionReference.action.ReadValue<Vector2>().normalized;
        }
    }
    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }    
    private void FixedUpdate()
    {
        _rigidbody2D.linearVelocity = PlayerInput * _moveSpeed;
    }
}

