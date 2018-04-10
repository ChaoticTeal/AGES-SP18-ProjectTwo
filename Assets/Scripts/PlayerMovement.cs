using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement: MonoBehaviour 
{
    // SerializeFields
    [Tooltip("Time to wait after an arrow is fired.")]
    [SerializeField]
    float arrowCooldown = 1f;
    [Tooltip("Time to wait before an arrow is fired.")]
    [SerializeField]
    float arrowDelay;
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
    /// Animator attached to the player
    /// </summary>
    Animator animator;
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
        animator = GetComponent<Animator>();
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
        animator.SetFloat("speed", Mathf.Max(Mathf.Abs(horizontalInput), Mathf.Abs(verticalInput)));
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
        if(shouldFire)
            animator.SetBool("shouldAttack", true);
    }

    /// <summary>
    /// Shoot an arrow
    /// </summary>
    private void Fire()
    {
        if (shouldFire)
        {
            StartCoroutine(FireArrow());
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
        animator.SetInteger("direction", (int)currentDirection);
    }

    /// <summary>
    /// Fire an arrow and pause
    /// </summary>
    /// <returns></returns>
    IEnumerator FireArrow()
    {
        canMove = false;
        yield return new WaitForSeconds(arrowDelay);
        GameObject temp = Instantiate(arrow);
        temp.transform.position = shootPoints[(int)currentDirection].position;
        temp.transform.rotation = shootPoints[(int)currentDirection].rotation;
        yield return new WaitForSeconds(arrowCooldown);
        shouldFire = false;
        canMove = true;
        animator.SetBool("shouldAttack", false);
    }
}
