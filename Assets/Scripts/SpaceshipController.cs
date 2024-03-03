using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SpaceshipController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private Rigidbody2D _rigidbody2D;
    public float acceleration;
    public float rotateSpeed;
    public float linearDrag;
    private float _viewOffset = 8;

    public event Action HasStoppedMoving = delegate { };

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.Move += OnAccelerateDirectional;
        _playerInput.Rotate += OnRotate;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.angularDrag = 5;
        _rigidbody2D.drag = linearDrag; 
    }

    public void OnDisable()
    {
        _playerInput.Move -= OnAccelerateDirectional;
        _playerInput.Rotate -= OnRotate;
    }
    private void OnRotate(Vector2 mousePos)
    {
        Vector3 mouseInGame = Camera.main.ScreenToWorldPoint(
            new Vector3(
                mousePos.x,
                mousePos.y,
                _viewOffset // View offset
            )
        );

        Vector2 direction = new Vector2(
            mouseInGame.x - transform.position.x,
            mouseInGame.y - transform.position.y
        );

        float _currentRotateSpeed;
        _currentRotateSpeed = (rotateSpeed / _viewOffset) / Mathf.Max(_rigidbody2D.velocity.magnitude, 1);

        transform.right = Vector3.Lerp(transform.right, direction, _currentRotateSpeed);
    }

    private void OnAccelerateDirectional(Vector2 direction)
    {
        if (direction.x != 0)
        {
            _rigidbody2D.AddForce(-transform.up * acceleration * direction.x);
            
        }
        if (direction.y != 0)
        {
            _rigidbody2D.AddForce(transform.right * acceleration * direction.y);
        }
    }
}
