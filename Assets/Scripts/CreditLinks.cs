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

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == projectileLayer)
        {
            if (audioSource != null)
                audioSource.Play();
            Application.OpenURL(link);
        }
    }
}
