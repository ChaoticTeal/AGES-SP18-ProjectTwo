using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour 
{
    // Public fields
    public static event System.Action DoorUnlocked;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Key")
        {
            if (FindObjectOfType<PlayerMovement>().KeyCount > 0)
            {
                if (DoorUnlocked != null)
                    DoorUnlocked.Invoke();
                gameObject.SetActive(false);
            }
        }
    }
}
