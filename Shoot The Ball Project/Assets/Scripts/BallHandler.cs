using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D currentBallRigidbody;
    [SerializeField] private SpringJoint2D currentBallSpringJoint;
    [SerializeField] private float detachDelay;
    private Camera mainCamera;
    private bool isDragging;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidbody == null)
        {
            return;
        }

        if (!Touchscreen.current.primaryTouch.press.isPressed)
        {
            if (isDragging)
            {
                LaunchBall();
            }

            isDragging = false;
            return;
        }

        isDragging = true;
        currentBallRigidbody.isKinematic = true; //Physics disabled on the object

        Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue(); //Screen pixel wise location of touch

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(touchPos); //Game scene wise location of touch

        // Debug.Log("Touch pos:" + touchPos);
        // Debug.Log("World pos:" + worldPos);
        currentBallRigidbody.position = worldPos;
    }

    private void LaunchBall()
    {
        currentBallRigidbody.isKinematic = false; //Physics enabled on the object
        currentBallRigidbody = null;
        Invoke(nameof(DetachBall), detachDelay);
    }

    private void DetachBall()
    {
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
    }
}