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
    [Tooltip("Projectile layer.")]
    [SerializeField]
    int projectileLayer;
    [Tooltip("Target index.")]
    [SerializeField]
    int targetIndex_UseProperty;
    [Tooltip("Target type.\n0 for single target/door opens.\n1 for double target/door opens.")]
    [SerializeField]
    int targetType_UseProperty;

    // Properties
    bool Hit
    {
        get
        {
            return hit_UseProperty;
        }
        set
        {
            hit_UseProperty = true;
            if (hit_UseProperty)
                if (OnHit != null)
                    // Notify that the target was hit, with its type and index
                    OnHit.Invoke(TargetType, TargetIndex);
        }
    }
    public int TargetType
    {
        get
        {
            return targetType_UseProperty;
        }
    }
    public int TargetIndex
    {
        get
        {
            return targetIndex_UseProperty;
        }
    }

    // Private fields
    /// <summary>
    /// The animator attached to the target
    /// </summary>
    Animator animator;
    /// <summary>
    /// Has the target been hit?
    /// </summary>
    bool hit_UseProperty;

    // Public fields
    /// <summary>
    /// Notify on hit
    /// </summary>
    public static event System.Action<int, int> OnHit;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(ShineAnim());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == projectileLayer)
        {
            Hit = true;
            animator.SetBool("hit", true);
        }
    }

    IEnumerator ShineAnim()
    {
        while(!Hit)
        {
            animator.SetBool("shouldShine", true);
            yield return null;
            animator.SetBool("shouldShine", false);
            yield return new WaitForSeconds(Random.Range(shineDelayMin, shineDelayMax));
        }
    }
}
