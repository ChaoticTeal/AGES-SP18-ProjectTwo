using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    // SerializeFields
    [Tooltip("List of all puzzle solutions in the game.")]
    [SerializeField]
    List<GameObject> solutions;
    [Tooltip("Start transition panel.")]
    [SerializeField]
    Image fadePanel;
    [Tooltip("Fade time.")]
    [SerializeField]
    float fadeTime;
    [Tooltip("Next scene.")]
    [SerializeField]
    string nextScene;
    
    // Use this for initialization
    void Start () 
	{
        StartCoroutine(FadeIn());
	}

    private IEnumerator FadeIn()
    {
        Color newColor = fadePanel.color;
        while (fadePanel.color.a - 0 > .25)
        {
            newColor.a = Mathf.Lerp(fadePanel.color.a, 0, fadeTime * Time.deltaTime);
            fadePanel.color = newColor;
            yield return null;
        }
        newColor.a = 0;
        fadePanel.color = newColor;
    }

    private void OnEnable()
    {
        Target.OnActivated += TriggerHandler;
        Target.OnDeactivated += UnTriggerHandler;
        PressurePlate.OnActivated += TriggerHandler;
        PressurePlate.OnDeactivated += UnTriggerHandler;
        PlayerMovement.EndGame += EndGame;
    }

    private void OnDisable()
    {
        Target.OnActivated -= TriggerHandler;
        Target.OnDeactivated -= UnTriggerHandler;
        PressurePlate.OnActivated -= TriggerHandler;
        PressurePlate.OnDeactivated -= UnTriggerHandler;
        PlayerMovement.EndGame -= EndGame;
    }

    /// <summary>
    /// Handle a puzzle trigger deactivating.
    /// </summary>
    /// <param name="type">The puzzle type associated with the trigger.</param>
    /// <param name="index">The index of the trigger.</param>
    private void UnTriggerHandler(int type, int index)
    {
        // Loop through solutions
        foreach (GameObject s in solutions)
        {
            IPuzzleSolution solution = s.GetComponent<IPuzzleSolution>();
            // If the door is open, skip it
            if (!solution.IsSolved)
                continue;
            if (solution.PuzzleType == type)
            {
                switch (type)
                {
                    // Single switch/door puzzle
                    case 0:
                        if (solution.PuzzleIndex == index)
                            solution.UndoSolution();
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
                                solution.UndoSolution();
                        }
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Handle a puzzle trigger.
    /// </summary>
    /// <param name="type">The puzzle type associated with the trigger.</param>
    /// <param name="index">The index of the trigger.</param>
    private void TriggerHandler(int type, int index)
    {
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

    void EndGame()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        Color newColor = fadePanel.color;
        while (1 - fadePanel.color.a > .25)
        {
            newColor.a = Mathf.Lerp(fadePanel.color.a, 1, fadeTime * Time.deltaTime);
            fadePanel.color = newColor;
            yield return null;
        }
        newColor.a = 1;
        fadePanel.color = newColor;
        SceneManager.LoadScene(nextScene);
    }
}
