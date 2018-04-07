using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour 
{
    // SerializeFields
    [Tooltip("Door to open.")]
    [SerializeField]
    GameObject door;
    [Tooltip("Projectile layer.")]
    [SerializeField]
    int projectileLayer;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == projectileLayer)
        {
            Debug.Log("Arrow hit.");
            door.SetActive(false);
        }
    }
}
