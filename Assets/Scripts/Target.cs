using System;
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
    [Tooltip("Time target remains active.\nIf 0, timer remains active indefinitely.")]
    [SerializeField]
    float timerLength;

    // Properties
    bool Activated
    {
        get
        {
            return activated_UseProperty;
        }
        set
        {
            activated_UseProperty = value;
            if (activated_UseProperty)
            {
                if (OnActivated != null)
                    // Notify that the target was hit, with its type and index
                    OnActivated.Invoke(TriggerType, TriggerIndex);
            }
            else if(OnDeactivated != null)
            {
                OnDeactivated.Invoke(TriggerType, TriggerIndex);
                StartCoroutine(ShineAnim());
            }
                    
        }
    }
    public int TriggerType
    {
        get
        {
            return targetType_UseProperty;
        }
    }
    public int TriggerIndex
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
    /// Audio source on the target
    /// </summary>
    AudioSource audioSource;
    /// <summary>
    /// Has the target been hit?
    /// </summary>
    bool activated_UseProperty;

    // Public fields
    /// <summary>
    /// Notify on hit
    /// </summary>
    public static event Action<int, int> OnActivated;
    /// <summary>
    /// Notify deactivation
    /// </summary>
    public static event Action<int, int> OnDeactivated;
    
    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ShineAnim());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == projectileLayer)
        {
            Activated = true;
            animator.SetBool("hit", true);
            audioSource.Play();
            if (timerLength > 0)
                StartCoroutine(TargetTimer());
        }
    }

    private IEnumerator TargetTimer()
    {
        yield return new WaitForSeconds(timerLength);
        Activated = false;
        animator.SetBool("hit", false);
    }

    IEnumerator ShineAnim()
    {
        while(!Activated)
        {
            animator.SetBool("shouldShine", true);
            yield return null;
            animator.SetBool("shouldShine", false);
            yield return new WaitForSeconds(UnityEngine.Random.Range(shineDelayMin, shineDelayMax));
        }
    }
}
