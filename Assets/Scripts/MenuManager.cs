using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
    // SerializeFields
    [Tooltip("Fade time.")]
    [SerializeField]
    float fadeTime;
    [Tooltip("Exit panel.")]
    [SerializeField]
    GameObject exitPanel;
    [Tooltip("Start transition panel.")]
    [SerializeField]
    Image fadePanel;
    [Tooltip("Next scene.")]
    [SerializeField]
    string nextScene;

    // public fields
    public static event Action GameStarting;


    private void OnEnable()
    {
        MenuTrigger.OnEntered += HandleEvents;
    }

    private void OnDisable()
    {
        MenuTrigger.OnEntered -= HandleEvents;
    }

    void HandleEvents(string type)
    {
        switch(type)
        {
            case "Start":
                StartCoroutine(StartGame());
                break;
            case "Exit":
                exitPanel.SetActive(true);
                break;
        }
    }

    private IEnumerator StartGame()
    {
        if (GameStarting != null)
            GameStarting.Invoke();
        Color newColor = fadePanel.color;
        while(fadePanel.color != Color.black)
        {
            newColor.a = Mathf.Lerp(fadePanel.color.a, 1, fadeTime * Time.deltaTime);
            fadePanel.color = newColor;
            yield return null;
        }
        SceneManager.LoadScene(nextScene);
    }

    public void ExitButtonYes()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ExitButtonNo()
    {
        exitPanel.SetActive(false);
    }
}
