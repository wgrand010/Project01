using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ThirdPersonMovement : MonoBehaviour


{
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };
   

    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    float turnsmoothvelocity;
    bool _ismoving = false;


    private void Start()
    {
        Idle?.Invoke();
    }


    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float Vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, Vertical).normalized;

        if (direction.magnitude >= 0.2f)
        {
            CheckIfStartedMoving();
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnsmoothvelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        else
        {
            CheckIfStoppedMoving();
        }
    }
        private void CheckIfStartedMoving()
        {
            if(_ismoving == false)
            {
                StartRunning?.Invoke();
                Debug.Log("Started Running");
            }
            _ismoving = true;
        }

        private void CheckIfStoppedMoving()
        {
            if (_ismoving == true)
            {
                Idle?.Invoke();
                Debug.Log("Stopped");
            }
            _ismoving = false;
        }
    
}
