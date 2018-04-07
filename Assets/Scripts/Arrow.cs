﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour 
{
    // SerializeFields
    [Tooltip("Speed of arrow.")]
    [SerializeField]
    float arrowSpeed = 5f;
    [Tooltip("Arrow lifetime on collision.")]
    [SerializeField]
    float arrowLifeTime = 1f;

    // Private fields
    /// <summary>
    /// Rigidbody2D component of arrow
    /// </summary>
    Rigidbody2D rigidbody2D;

	// Use this for initialization
	void Start () 
	{
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.velocity = transform.up * arrowSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        Destroy(gameObject, arrowLifeTime);
    }
}
