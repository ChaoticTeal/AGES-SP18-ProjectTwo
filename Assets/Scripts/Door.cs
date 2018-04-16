using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IPuzzleSolution
{
    // Private fields
    /// <summary>
    /// Is the door open?
    /// </summary>
    bool isSolved_UseProperty;
    /// <summary>
    /// How many corresponding targets have been hit?
    /// </summary>
    int targetsHit_UseProperty;

    // SerializeFields
    /// <summary>
    /// What type of puzzle will open this door?
    /// 0 for single target/door open
    /// 1 for double target/door open
    /// </summary>
    [Tooltip("What type of puzzle will open this door?\n0 for single target/door opens.\n1 for double target/door opens.")]
    [SerializeField]
    int puzzleType_UseProperty;
    [Tooltip("Door index.")]
    [SerializeField]
    int puzzleIndex_UseProperty;

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
                gameObject.SetActive(false);
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
        IsSolved = true;
    }
}
