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
    [Tooltip("Target index.")]
    [SerializeField]
    int targetIndex;
    [Tooltip("Target type. 0 for single-target, door opens.")]
    [SerializeField]
    int targetType;
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

    // Public fields
    /// <summary>
    /// Notify on hit
    /// </summary>
    //public static event ;
    
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
            //if (OnHit != null)
            //    OnHit.Invoke();
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
