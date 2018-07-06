using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IPuzzleSolution
{
    // Private fields
    /// <summary>
    /// AudioSource attached to the key
    /// </summary>
    AudioSource audioSource;
    /// <summary>
    /// Has the key been collected?
    /// </summary>
    bool isCollected;
    /// <summary>
    /// Is the key active?
    /// </summary>
    bool isSolved_UseProperty;
    /// <summary>
    /// How many corresponding targets have been hit?
    /// </summary>
    int targetsHit_UseProperty;
    /// <summary>
    /// Renderer on the object
    /// </summary>
    Renderer renderer;
    /// <summary>
    /// Collider on the object
    /// </summary>
    BoxCollider2D boxCollider;

    // SerializeFields
    [Tooltip("Key index.")]
    [SerializeField]
    int puzzleIndex_UseProperty;
    /// <summary>
    /// What type of puzzle will spawn this key?
    /// 0 for single target/key spawn
    /// 1 for double target/key spawn
    /// </summary>
    [Tooltip("What type of puzzle will spawn this key?\n0 for single target/key spawns.\n1 for double target/key spawns.")]
    [SerializeField]
    int puzzleType_UseProperty;

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

    public void DoSolution()
    {
        if (!isCollected)
        {
            renderer.enabled = true;
            boxCollider.enabled = true;
        }
    }

    public void UndoSolution()
    {
        if (!isCollected)
        {
            boxCollider.enabled = false;
            renderer.enabled = false;
        }
    }

    // Use this for initialization
    void Start () 
	{
        audioSource = GetComponent<AudioSource>();
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.enabled = false;
        renderer = GetComponent<Renderer>();
        renderer.enabled = false;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            isCollected = true;
            if (audioSource != null)
                audioSource.Play();
            boxCollider.enabled = false;
            renderer.enabled = false;
        }
    }
}
