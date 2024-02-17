using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField] 
    private float moveSpeed = 5f;

    private Rigidbody2D _rigidBody;
    private Vector2 _moveInput; 
    private bool _canMove = true;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(_canMove == false)
            return;

        // Get input axes
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        _moveInput = new Vector2(moveHorizontal, moveVertical).normalized;        

        Vector2 movement = _moveInput * moveSpeed;
        _rigidBody.velocity = movement;
    }

    public void SetCanMove(bool canMove)
    {
        _canMove = canMove;
        _moveInput = Vector2.zero;
        _rigidBody.velocity = Vector2.zero;
    }
}
