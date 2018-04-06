using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement: MonoBehaviour 
{
    // SerializeFields
    [Tooltip("Player move speed.")]
    [SerializeField]
    float moveSpeed = 5f;

    // Private fields
    /// <summary>
    /// Horizontal input check
    /// </summary>
    float horizontalInput;
    /// <summary>
    /// Vertical input check
    /// </summary>
    float verticalInput;
    /// <summary>
    /// Player Rigidbody2D
    /// </summary>
    Rigidbody2D rigidbody2D;

	// Use this for initialization
	void Start () 
	{
        rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
        GetMoveInput();
	}

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rigidbody2D.velocity = new Vector2(horizontalInput, verticalInput) * moveSpeed;
    }

    private void GetMoveInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }
}
