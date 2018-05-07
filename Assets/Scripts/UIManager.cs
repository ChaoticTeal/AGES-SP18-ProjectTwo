using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour 
{
    // SerializeFields
    [Tooltip("Key number display.")]
    [SerializeField]
    Text keyText;

    private void OnEnable()
    {
        PlayerMovement.KeyCollected += UpdateKeyText;
    }

    private void OnDisable()
    {
        PlayerMovement.KeyCollected -= UpdateKeyText;
    }

    private void UpdateKeyText(int newKeyCount)
    {
        keyText.text = "Keys: " + newKeyCount;
    }


    // Use this for initialization
    void Start () 
	{
        UpdateKeyText(0);
	}
}
