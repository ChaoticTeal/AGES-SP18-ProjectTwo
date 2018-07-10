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
    [Tooltip("The object with the BGM.")]
    [SerializeField]
    GameObject musicHolder;
    [Tooltip("Start transition panel.")]
    [SerializeField]
    Image fadePanel;
    [Tooltip("Next scene.")]
    [SerializeField]
    string nextScene;

    // public fields
    public static event Action GameStarting;
    public static event Action EndPanelUp;
    public static event Action EndPanelDown;


    private void OnEnable()
    {
        MenuTrigger.OnEntered += HandleEvents;
    }

    private void OnDisable()
    {
        MenuTrigger.OnEntered -= HandleEvents;
    }

    private void Start()
    {
        GameObject music = GameObject.Find("MusicObject");
        if(music == null)
        {
            music = Instantiate(musicHolder);
            music.name = "MusicObject";
            DontDestroyOnLoad(music);
        }
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
                if (EndPanelUp != null)
                    EndPanelUp.Invoke();
                exitPanel.transform.GetComponentInChildren<Button>().Select();
                break;
        }
    }

    private IEnumerator StartGame()
    {
        if (GameStarting != null)
            GameStarting.Invoke();
        Color newColor = fadePanel.color;
        while(1 - fadePanel.color.a > .25)
        {
            newColor.a = Mathf.Lerp(fadePanel.color.a, 1, fadeTime * Time.deltaTime);
            fadePanel.color = newColor;
            yield return null;
        }
        newColor.a = 1;
        fadePanel.color = newColor;
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
        if (EndPanelDown != null)
            EndPanelDown.Invoke();
    }
}
