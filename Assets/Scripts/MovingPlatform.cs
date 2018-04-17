using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour 
{
    // SerializeFields
    [Tooltip("Platform movement speed.")]
    [SerializeField]
    float moveSpeed = 5f;
    [Tooltip("Move threshold.")]
    [SerializeField]
    float moveThreshold = .05f;
    [Tooltip("Pause time.")]
    [SerializeField]
    float pauseTime = 1f;
    [Tooltip("Positions to move between.\nFirst should be <0,0,0>")]
    [SerializeField]
    List<Vector3> positions;

	// Use this for initialization
	void Start () 
	{
        StartCoroutine(Move());
	}

    private IEnumerator Move()
    {
        int i = 1;
        while(true)
        {
            Debug.Log("i: " + i);
            while((transform.position - positions[i]).magnitude > moveThreshold)
            {
                transform.position = Vector3.Lerp(transform.position, positions[i], moveSpeed * Time.deltaTime);
                yield return null;
            }
            if (++i > positions.Count - 1)
                i = 0;
            yield return new WaitForSeconds(pauseTime);
        }
    }

    // Update is called once per frame
    void Update () 
	{
		
	}
}
