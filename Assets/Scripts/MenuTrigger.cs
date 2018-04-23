using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTrigger : MonoBehaviour 
{
    // SerializeFields
    [Tooltip("What type of trigger is this?")]
    [SerializeField]
    string triggerType;

    // public fields
    public static event System.Action<string> OnEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            if (OnEntered != null)
                OnEntered.Invoke(triggerType);
    }
}
