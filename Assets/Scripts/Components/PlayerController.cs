using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterController;

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _jumpPower;

    private Vector2 _playerMove;

    private Vector3 _direction;

    private float _velocity;

    private float _gravity = -5f;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void OnMove(InputValue inputValue)
    {
        _playerMove = inputValue.Get<Vector2>();
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
            _velocity = 1f;
        }
        else
        {
            _velocity += _gravity * 2 * Time.deltaTime;
        }

        _direction.y = _velocity;
    }

    private void Move()
    {
        Run();
    }

    private void Jump()
    {
        Debug.Log("Jump");

        _velocity += _jumpPower;
    }

    private void Run()
    {
        //// Calculate the movement direction based on input
        //var moveDirection = new Vector3(_playerMove.x, 0f, _playerMove.y);

        //// Convert the movement direction from local to world space
        //moveDirection = transform.TransformDirection(moveDirection);

        //// Apply movement speed
        //moveDirection *= _moveSpeed * Time.deltaTime;

        //// Move the character
        //_characterController.Move(moveDirection);

        // Calculate movement direction based on input
        var movementDirection = new Vector3(_playerMove.x, 0.0f, _playerMove.y).normalized;

        // Move the character based on input and speed
        _characterController.Move(movementDirection * _moveSpeed * Time.deltaTime);
    }

    //private void Strafe()
    //{
    //    var movement = new Vector3(_playerMove.y, 0f, -_playerMove.x); // Adjusted for left-right movement
    //    movement = transform.TransformDirection(movement * _moveSpeed * Time.deltaTime);
    //    Debug.Log(movement);
    //    _characterController.Move(movement);
    //}
}
