using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour 
{
    // SerializeFields
    [Tooltip("Maximum time between shine animations.")]
    [SerializeField]
    float shineDelayMax;
    [Tooltip("Minimum time between shine animations.")]
    [SerializeField]
    float shineDelayMin;
    [Tooltip("Door to open.")]
    [SerializeField]
    GameObject door;
    [Tooltip("Projectile layer.")]
    [SerializeField]
    int projectileLayer;

    // Private fields
    /// <summary>
    /// The animator attached to the target
    /// </summary>
    Animator animator;
    /// <summary>
    /// Has the target been hit?
    /// </summary>
    bool hit;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ShineAnim());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == projectileLayer)
        {
            hit = true;
            animator.SetBool("hit", true);
            door.SetActive(false);
        }
    }

    IEnumerator ShineAnim()
    {
        while(!hit)
        {
            animator.SetBool("shouldShine", true);
            yield return null;
            animator.SetBool("shouldShine", false);
            yield return new WaitForSeconds(Random.Range(shineDelayMin, shineDelayMax));
        }
    }
}
