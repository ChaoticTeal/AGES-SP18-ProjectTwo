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
    [Tooltip("Arrow prefab.")]
    [SerializeField]
    GameObject arrow;
    [Tooltip("Origin points for arrows. T, R, B, L.")]
    [SerializeField]
    List<Transform> shootPoints;
    [Tooltip("Horizontal movement axis name.")]
    [SerializeField]
    string horizontalAxis;
    [Tooltip("Vertical movement axis name.")]
    [SerializeField]
    string verticalAxis;
    [Tooltip("Fire button name.")]
    [SerializeField]
    string fireButton;

    // Private fields
    /// <summary>
    /// Can the player move?
    /// </summary>
    bool canMove = true;
    /// <summary>
    /// Should the player fire?
    /// </summary>
    bool shouldFire;
    /// <summary>
    /// Player direction enum
    /// </summary>
    enum DIRECTIONS { Top, Right, Bottom, Left };
    /// <summary>
    /// Current facing direction
    /// </summary>
    DIRECTIONS currentDirection = DIRECTIONS.Bottom;
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
        GetFireInput();
	}

    private void FixedUpdate()
    {
        if(canMove)
        {
            SetDirection();
            Move();
            Fire();
        }
    }

    /// <summary>
    /// Get input to determine movement
    /// </summary>
    private void GetMoveInput()
    {
        horizontalInput = Input.GetAxis(horizontalAxis);
        verticalInput = Input.GetAxis(verticalAxis);
    }

    /// <summary>
    /// Move player character
    /// </summary>
    private void Move()
    {
        rigidbody2D.velocity = new Vector2(horizontalInput, verticalInput) * moveSpeed;
    }

    /// <summary>
    /// Get input to determine shooting
    /// </summary>
    private void GetFireInput()
    {
        shouldFire = Input.GetButtonDown(fireButton);
    }

    /// <summary>
    /// Shoot an arrow
    /// </summary>
    private void Fire()
    {
        if (shouldFire)
        {
            GameObject temp =  Instantiate(arrow);
            temp.transform.position = shootPoints[(int)currentDirection].position;
            temp.transform.rotation = shootPoints[(int)currentDirection].rotation;
        }
    }

    /// <summary>
    /// Based on movement input, determine facing direction
    /// </summary>
    private void SetDirection()
    {
        if (horizontalInput > 0)
            currentDirection = DIRECTIONS.Right;
        else if (horizontalInput < 0)
            currentDirection = DIRECTIONS.Left;
        else if (verticalInput > 0)
            currentDirection = DIRECTIONS.Top;
        else if (verticalInput < 0)
            currentDirection = DIRECTIONS.Bottom;
    }
}
