using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditLinks : MonoBehaviour 
{
    [Tooltip("Link to open.")]
    [SerializeField]
    string link;
    [Tooltip("Projectile layer.")]
    [SerializeField]
    int projectileLayer;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == projectileLayer)
            Application.OpenURL(link);
    }
}
