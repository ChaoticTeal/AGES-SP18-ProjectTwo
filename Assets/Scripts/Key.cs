using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IPuzzleSolution
{
    // Private fields
    /// <summary>
    /// Is the key active?
    /// </summary>
    bool isSolved_UseProperty;
    /// <summary>
    /// How many corresponding targets have been hit?
    /// </summary>
    int targetsHit_UseProperty;

    // SerializeFields
    /// <summary>
    /// What type of puzzle will spawn this key?
    /// 0 for single target/key spawn
    /// 1 for double target/key spawn
    /// </summary>
    [Tooltip("What type of puzzle will spawn this key?\n0 for single target/key spawns.\n1 for double target/key spawns.")]
    [SerializeField]
    int puzzleType_UseProperty;
    [Tooltip("Key index.")]
    [SerializeField]
    int puzzleIndex_UseProperty;

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
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
