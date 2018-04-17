using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    // SerializeFields
    [Tooltip("List of all puzzle solutions in the game.")]
    [SerializeField]
    List<GameObject> solutions;
    /*
    [Tooltip("List of all targets in the game.")]
    [SerializeField]
    List<GameObject> targets;
    */

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    private void OnEnable()
    {
        Target.OnHit += TargetHandler;
    }

    private void OnDisable()
    {
        Target.OnHit -= TargetHandler;
    }

    /// <summary>
    /// Handle a target hit.
    /// </summary>
    /// <param name="type">The puzzle type associated with the target.</param>
    /// <param name="index">The index of the target.</param>
    private void TargetHandler(int type, int index)
    {
        Debug.Log(type + " Target " + index + " hit.");
        // Loop through solutions
        foreach(GameObject s in solutions)
        {
            IPuzzleSolution solution = s.GetComponent<IPuzzleSolution>();
            // If the door is open, skip it
            if (solution.IsSolved)
                continue;
            if(solution.PuzzleType == type)
            {
                switch(type)
                {
                    // Single switch/door puzzle
                    case 0:
                        if (solution.PuzzleIndex == index)
                            solution.DoSolution();
                        break;
                    // Double switch/door puzzle
                    case 1:
                        // If the target lines up with one of the solution's targets, make a note of it
                        // Solution 0 will use targets 0 and 1, solution 1 will use 2 and 3, etc.
                        if ((solution.PuzzleIndex * 2 == index) || ((solution.PuzzleIndex * 2) + 1) == index)
                        {
                            // Add to the number of targets hit
                            solution.TargetsHit++;
                            // If both targets have been hit, act on the solution
                            if (solution.TargetsHit == 2)
                                solution.DoSolution();
                        }
                        break;
                }
            }
        }
    }
}
