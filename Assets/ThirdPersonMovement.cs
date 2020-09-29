using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ThirdPersonMovement : MonoBehaviour


{
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };
    public event Action StartJumping = delegate { };
    [SerializeField] AudioClip AbilitySound;
    [SerializeField] AudioClip HurtSound;
    AudioSource audioSource;

    public CharacterController controller;
    public Transform cam;

    
    public float gravity = -9.81f;
    Vector3 velocity;
    bool isGrounded;
    public float jumpHeight = 3;
    private const float walkSpeed = 5.0f;
    private const float runSpeed = 10.0f;
    public float speed = walkSpeed;
    public int maxHealth = 100;
    public int currentHealth;

    public healthbar _healthbar;

    public Transform groundCheck;
    public float groundDistance = 0;
    public LayerMask groundMask;

    public float turnSmoothTime = 0.1f;
    float turnsmoothvelocity;
    bool _ismoving = false;
    public bool grav = true;
    

    private void Start()
    {
        currentHealth = maxHealth;
        _healthbar.SetMaxHealth(maxHealth);
        Idle?.Invoke();
    }

    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        if (other.gameObject.tag == "enemy")
        {
            TakeDamage(1);
        }
    }


    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f;
        }

         

        if(Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20);
            Debug.Log("taking damage");
        }


        if (currentHealth == 0)
        {
            SceneManager.LoadScene("SampleScene");
        }

        //gravity
        if (grav == true)
        {
            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

        }


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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jumping();
        }

       if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
       else
        {
            speed = walkSpeed;
        }
       
       
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (AbilitySound != null)
            {
                AudioHelper.PlayClip2D(AbilitySound, 1f);
            }
            jumpHeight = 10;
            StartCoroutine(levelup());

        }
            IEnumerator levelup()
              {
            yield return new WaitForSeconds(3);
            jumpHeight = 3;
               
            }

           /* if (AbilitySound != null)
            {
                AudioHelper.PlayClip2D(AbilitySound, 1f);
            }
            if (gravity == -9.81f)
            {
                gravity = 10;
            }
            else
            {
                gravity = -9.81f;
            }
           */
        
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
    
        private void Jumping()
    {
        if (isGrounded == true)
        {
            StartJumping?.Invoke();
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            Debug.Log("Jumping");
            isGrounded = false;
        }
        isGrounded = false;
    }

    [Obsolete]
    void TakeDamage(int damage)
    {
        if (AbilitySound != null && AbilitySound.isReadyToPlay)
        {
            AudioHelper.PlayClip2D(HurtSound, 1f);
        }
        currentHealth -= damage;
        _healthbar.SetHealth(currentHealth);
    }


}
