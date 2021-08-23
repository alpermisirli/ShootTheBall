using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot_rb;
    [SerializeField] private float detachDelay;
    [SerializeField] private float respawnDelay;

    private Rigidbody2D currentBallRigidbody;
    private SpringJoint2D currentBallSpringJoint;
    private Camera mainCamera;
    private bool isDragging;

    private void Start()
    {
        mainCamera = Camera.main;

        SpawnBall();
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

    private void SpawnBall()
    {
        GameObject ballInstance = Instantiate(ballPrefab, pivot_rb.position, Quaternion.identity);
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();

        currentBallSpringJoint.connectedBody = pivot_rb;
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

        Invoke(nameof(SpawnBall), respawnDelay);
    }
}