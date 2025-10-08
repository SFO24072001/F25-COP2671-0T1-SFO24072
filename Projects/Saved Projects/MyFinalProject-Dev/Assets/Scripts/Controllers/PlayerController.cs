using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : SingletonMonobehaviour<PlayerController>
{
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private InputActionReference _inputInteractReference;
    [SerializeField] private InputActionReference _inputActionReference;


    private GameObject _playerCharacter;
    private Animator _animator;    

    private Rigidbody2D _rigidbody;
    private float _lastHorizontal, _lastVertical;
    private bool _isBusy = false;


    private Vector2 _playerInput => _inputActionReference.action.ReadValue<Vector2>().normalized;

    protected override void Initialize()
    {
        _playerCharacter = transform.GetChild(0).gameObject;
        _animator = _playerCharacter.GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (_isBusy) return;

        _lastHorizontal = _playerInput.x == 0 ? 0 : _playerInput.x < 0 ? -1 : 1;
        _lastVertical = _playerInput.y == 0 ? 0 : _playerInput.y < 0 ? -1 : 1;

        if (_lastHorizontal != 0)
        {
            _playerCharacter.transform.localScale = new Vector3(_lastHorizontal, 1, 1);
        }


        if (_animator == null) return;

        _animator.SetFloat(Constants.AnimatorLastHorizontal, _lastHorizontal);
        _animator.SetFloat(Constants.AnimatorLastVertical, _lastVertical);
        _animator.SetFloat(Constants.AnimatorSpeed, _rigidbody.linearVelocity.magnitude);

        if (_inputInteractReference == null) return;
        if (_inputInteractReference.action.WasPressedThisFrame())
        {
            StartCoroutine(UseWaterRoutine());
        }
        
    }
    void FixedUpdate()
    {
        _rigidbody.linearVelocity = _moveSpeed * _playerInput;        
    }

    private IEnumerator UseWaterRoutine()
    {
        _isBusy = true;
        _animator.SetFloat(Constants.AnimatorLastHorizontal, _lastHorizontal);
        _animator.SetFloat(Constants.AnimatorLastVertical, _lastVertical);
        _animator.SetBool(Constants.AnimatorUseWater, true);
        yield return new WaitForSeconds(1);
        _isBusy = false;
        _animator.SetBool(Constants.AnimatorUseWater, false);
    }

}
