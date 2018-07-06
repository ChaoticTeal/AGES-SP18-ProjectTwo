using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IPuzzleSolution
{
    // Private fields
    /// <summary>
    /// Audio source attached to the door
    /// </summary>
    AudioSource audioSource;
    /// <summary>
    /// Is the door open?
    /// </summary>
    bool isSolved_UseProperty;
    /// <summary>
    /// Is the collider overlapping the player?
    /// </summary>
    bool overlapping;
    /// <summary>
    /// How many corresponding targets have been hit?
    /// </summary>
    int targetsHit_UseProperty;
    /// <summary>
    /// The renderer attached to the door
    /// </summary>
    Renderer renderer;
    /// <summary>
    /// The collider attached to the door
    /// </summary>
    Collider2D collider;
    /// <summary>
    /// The composite collider
    /// </summary>
    CompositeCollider2D compCollider;

    // SerializeFields
    [Tooltip("Door index.")]
    [SerializeField]
    int puzzleIndex_UseProperty;
    [Tooltip("What type of puzzle will open this door?\n0 for single target/door opens.\n1 for double target/door opens.")]
    [SerializeField]
    int puzzleType_UseProperty;
    [Tooltip("Point effector to resolve overlap.")]
    [SerializeField]
    PointEffector2D pointEffector;

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
            if (isSolved_UseProperty)
            {
                if (audioSource != null)
                    audioSource.Play();
                renderer.enabled = false;
                collider.enabled = false;
            }
            else
                StartCoroutine(Reenable());
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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider2D>();
        compCollider = GetComponent<CompositeCollider2D>();
    }

    public void DoSolution()
    {
        IsSolved = true;
    }

    public void UndoSolution()
    {
        IsSolved = false;
    }

    IEnumerator Reenable()
    {
        collider.enabled = true;
        if (compCollider != null)
        {
            compCollider.isTrigger = true;
            yield return new WaitForSeconds(.1f);
            if (overlapping && pointEffector != null)
            {
                pointEffector.enabled = true;
                yield return new WaitForSeconds(.25f);
                pointEffector.enabled = false;
            }
            overlapping = false;
            if(!IsSolved)
                compCollider.isTrigger = false;
        }
        if(!IsSolved)
            renderer.enabled = true;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        overlapping = true;
    }
}
