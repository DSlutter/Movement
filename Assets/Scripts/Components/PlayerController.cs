using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _jumpPower;

    [SerializeField]
    private float _gravity;


    private CharacterController _characterController;

    private Vector2 _playerMove;

    private Vector3 _direction = new();

    private float _velocity;



    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        ApplyGravity();
        ApplyMovement();
    }

    private void OnMove(InputValue inputValue)
    {
        _playerMove = inputValue.Get<Vector2>();
        Move();
    }

    private void OnJump()
    {
        if(!_characterController.isGrounded)
        {
            return;
        }

        Jump();
    }

    private void ApplyGravity()
    {
        if( _characterController.isGrounded && _velocity <= 0f) 
        {
            _velocity = -1f;
        }
        else
        {
            _velocity += _gravity * 2 * Time.deltaTime;
        }

        _direction.y = _velocity;
    }

    private void ApplyMovement()
    {
        _characterController.Move(_direction * _moveSpeed * Time.deltaTime);
    }
    
    private void Move()
    {
        _direction = new Vector3(_playerMove.x, 0.0f, _playerMove.y);
    }

    private void Jump()
    {
        Debug.Log("Jump");

        _velocity += _jumpPower;
    }
}
