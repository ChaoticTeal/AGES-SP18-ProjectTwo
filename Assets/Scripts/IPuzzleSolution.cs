using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPuzzleSolution 
{
    // Properties
    bool IsSolved { get; set; }
    int PuzzleType { get; }
    int PuzzleIndex { get; }
    int TargetsHit { get; set; }

    // Methods
    void DoSolution();
}
