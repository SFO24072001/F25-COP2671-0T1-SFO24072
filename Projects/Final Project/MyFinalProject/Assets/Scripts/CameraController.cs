using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform _target;
    private Transform _clampMin, _clampMax;
    private Camera _camera;
    private float _halfWidth, _halfHeight;

    private void Start()
    {
        _target = FindAnyObjectByType<PlayerController>().transform;
        _clampMin = transform.GetChild(0);
        _clampMax = transform.GetChild(1);

        _clampMin.SetParent(null);
        _clampMax.SetParent(null);

        _camera = GetComponent<Camera>();
        _halfHeight = _camera.orthographicSize;
        _halfWidth = _camera.orthographicSize * _camera.aspect;
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(_target.position.x, _target.position.y, transform.position.z);

        var clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, _clampMin.position.x +_halfWidth , _clampMax.position.x - _halfWidth);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, _clampMin.position.y +_halfHeight , _clampMax.position.y - _halfHeight);

        transform.position = clampedPosition;

    }
}
