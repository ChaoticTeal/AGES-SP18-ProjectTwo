using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IPuzzleSolution 
{
    // SerializeFields
    [Tooltip("Platform movement speed.")]
    [SerializeField]
    float moveSpeed = 5f;
    [Tooltip("Move threshold.")]
    [SerializeField]
    float moveThreshold = .05f;
    [Tooltip("Pause time.")]
    [SerializeField]
    float pauseTime = 1f;
    [Tooltip("Platform index.")]
    [SerializeField]
    int puzzleIndex_UseProperty;
    /// <summary>
    /// What type of puzzle will affect the platform?
    /// 0 for single target/stop movement
    /// 1 for double target/stop movement
    /// </summary>
    [Tooltip("What type of puzzle will affect the platform?\n0 for single target/stop movement.\n1 for double target/stop movement.")]
    [SerializeField]
    int puzzleType_UseProperty;
    [Tooltip("Positions to move between.\nFirst should be <0,0,0>")]
    [SerializeField]
    List<Vector3> positions;

    // Private fields
    /// <summary>
    /// Can the platform move?
    /// </summary>
    bool canMove = true;
    /// <summary>
    /// Is the platform stopped?
    /// </summary>
    bool isSolved_UseProperty;
    /// <summary>
    /// How many corresponding targets have been hit?
    /// </summary>
    int targetsHit_UseProperty;

    // Properties
    public bool IsSolved
    {
        get
        {
            return isSolved_UseProperty;
        }
        set
        {
            isSolved_UseProperty = value;
        }
    }
    public int PuzzleType
    {
        get
        {
            return puzzleType_UseProperty;
        }
    }
    public int PuzzleIndex
    {
        get
        {
            return puzzleIndex_UseProperty;
        }
    }
    public int TargetsHit
    {
        get
        {
            return targetsHit_UseProperty;
        }
        set
        {
            targetsHit_UseProperty = value;
        }
    }

    // Use this for initialization
    void Start () 
	{
        StartCoroutine(Move());
	}

    private IEnumerator Move()
    {
        int i = 1;
        while(canMove)
        {
            while((transform.position - positions[i]).magnitude > moveThreshold)
            {
                transform.position = Vector3.Lerp(transform.position, positions[i], moveSpeed * Time.deltaTime);
                yield return null;
            }
            if (++i > positions.Count - 1)
                i = 0;
            yield return new WaitForSeconds(pauseTime);
        }
    }

    // Update is called once per frame
    void Update () 
	{
		
	}

    public void DoSolution()
    {
        canMove = false;
        IsSolved = true;
    }

    public void UndoSolution()
    {
        canMove = true;
        IsSolved = false;
        StartCoroutine(Move());
    }
}
