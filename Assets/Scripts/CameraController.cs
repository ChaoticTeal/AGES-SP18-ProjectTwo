using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    // SerializeFields
    [Tooltip("Camera speed.")]
    [SerializeField]
    float cameraSpeed = 5f;
    [Tooltip("Vertical height of a single screen.")]
    [SerializeField]
    float screenHeight;
    [Tooltip("Horizontal width of a single screen.")]
    [SerializeField]
    float screenWidth;
    [Tooltip("Margin of eror for movement.")]
    [SerializeField]
    float positionMargin = .05f;

    // Public fields
    /// <summary>
    /// Notify when the camera finishes moving
    /// </summary>
    public static event Action OnMoveFinished;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

    private void OnEnable()
    {
        PlayerMovement.ScreenChangeTrigger += OnPlayerScreenChange;
    }

    private void OnDisable()
    {
        PlayerMovement.ScreenChangeTrigger -= OnPlayerScreenChange;
    }

    /// <summary>
    /// When the player hits a screen change trigger, move the camera
    /// </summary>
    /// <param name="direction"></param>
    void OnPlayerScreenChange(string direction)
    {
        Vector3 newPosition = transform.position;
        switch(direction)
        {
            case "Top":
                newPosition += new Vector3(0, screenHeight);
                break;
            case "Right":
                newPosition += new Vector3(screenWidth, 0);
                break;
            case "Bottom":
                newPosition += new Vector3(0, -screenHeight);
                break;
            case "Left":
                newPosition += new Vector3(-screenWidth, 0);
                break;
        }
        if (newPosition != transform.position)
            StartCoroutine(CameraMove(newPosition));
    }

    /// <summary>
    /// Lerp the camera position to the new position
    /// </summary>
    /// <param name="newPosition"></param>
    /// <returns></returns>
    IEnumerator CameraMove(Vector3 newPosition)
    {
        while((transform.position - newPosition).magnitude > positionMargin)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, cameraSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = newPosition;
        if (OnMoveFinished != null)
            OnMoveFinished.Invoke();
    }
}
