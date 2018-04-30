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
    [Tooltip("Key prefab.")]
    [SerializeField]
    GameObject projectileKey;
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
    /// Can the player change screens?
    /// </summary>
    bool canChangeScreens = true;
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
    /// Keys collected
    /// </summary>
    int keyCount_UseProperty;
    /// <summary>
    /// Player Rigidbody2D
    /// </summary>
    Rigidbody2D rigidbody2D;
    /// <summary>
    /// Player scale
    /// </summary>
    Vector3 scale;

    // Public fields
    /// <summary>
    /// Notify when entering a screen edge trigger
    /// </summary>
    public static event Action<string> ScreenChangeTrigger;
    /// <summary>
    /// Notify when collecting a key
    /// </summary>
    public static event Action<int> KeyCollected;

    // Properties
    int KeyCount
    {
        get
        {
            return keyCount_UseProperty;
        }
        set
        {
            keyCount_UseProperty = value;
            if (KeyCollected != null)
                KeyCollected.Invoke(keyCount_UseProperty);
        }
    }

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
        if (shouldFire && canMove)
            StartCoroutine(FireArrow());
        if (!canMove)
            rigidbody2D.velocity = Vector3.zero;
	}

    private void FixedUpdate()
    {
        if(canMove)
        {
            SetDirection();
            Move();
        }
    }

    private void OnEnable()
    {
        CameraController.OnMoveFinished += EndScreenTransition;
        LockedDoor.DoorUnlocked += UseKey;
        MenuManager.GameStarting += StopMove;
    }

    private void OnDisable()
    {
        CameraController.OnMoveFinished -= EndScreenTransition;
        LockedDoor.DoorUnlocked -= UseKey;
        MenuManager.GameStarting -= StopMove;
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
        animator.SetBool("shouldAttack", true);
        canMove = false;
        yield return new WaitForSeconds(arrowDelay);
        if (KeyCount < 1)
        {
            GameObject temp = Instantiate(arrow);
            temp.transform.position = shootPoints[(int)currentDirection].position;
            temp.transform.rotation = shootPoints[(int)currentDirection].rotation;
        }
        else
        {
            GameObject temp = Instantiate(projectileKey);
            temp.transform.position = shootPoints[(int)currentDirection].position;
            temp.transform.rotation = shootPoints[(int)currentDirection].rotation;
        }
        yield return new WaitForSeconds(arrowCooldown);
        shouldFire = false;
        canMove = true;
        animator.SetBool("shouldAttack", false);
    }

    IEnumerator ScreenChangeCooldown()
    {
        yield return new WaitForSeconds(1.0f);
        canChangeScreens = true;
    }

    void StopMove()
    {
        canMove = false;
    }

    void EndScreenTransition()
    {
        canMove = true;
        animator.enabled = true;
        GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(ScreenChangeCooldown());
    }

    void UseKey()
    {
        KeyCount--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Key")
            KeyCount++;
        else if (canChangeScreens)
        {
            // Loop through each of the four directions checking for screen shift tags
            foreach (string s in Enum.GetNames(typeof(DIRECTIONS)))
            {
                if (collision.tag == s)
                {
                    // Send the event with the direction that matches
                    if (ScreenChangeTrigger != null)
                    {
                        ScreenChangeTrigger.Invoke(s);
                        // Disable movement and screen changing
                        canMove = false;
                        animator.enabled = false;
                        canChangeScreens = false;
                        // Move to the exit of the screen trigger
                        transform.position = collision.GetComponent<ScreenChangeTrigger>().exit.position;
                        // Make the player invisible temporarily
                        GetComponent<SpriteRenderer>().enabled = false;
                        animator.enabled = false;
                    }
                }
            }
        }
    }
}
