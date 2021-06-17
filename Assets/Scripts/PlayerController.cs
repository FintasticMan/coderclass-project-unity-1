using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum State
    {
        Walking,
        Running,
        Crouching
    }

    private GameController gameController;

    private Rigidbody rb;
    public float speed;
    public float jumpHeight;
    public float jumpSpeed;
    public float counterFactor;

    private float effectiveSpeed;
    private float effectiveJumpHeight;
    private float effectiveJumpSpeed;

    private float inputForward = 0f;
    private float inputStrafe = 0f;
    private bool inputRun = false;
    private bool inputCrouch = false;
    private bool inputJump = false;

    private bool isGrounded;

    private State currentState;

    private void Start()
    {
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }

        rb = GetComponent<Rigidbody>();
        
        currentState = State.Walking;
        effectiveSpeed = speed;
        effectiveJumpHeight = 0f;
        effectiveJumpSpeed = 0f;
    }

    private void Update()
    {
        inputForward = Input.GetAxis("Vertical");
        inputStrafe = Input.GetAxis("Horizontal");
        inputRun = Input.GetButton("Run");
        inputCrouch = Input.GetButton("Crouch");
        inputJump = Input.GetButton("Jump");

        if (inputJump && isGrounded)
        {
            if (inputRun && inputCrouch)
            {
                effectiveJumpHeight = jumpHeight / 2f;
                effectiveJumpSpeed = jumpSpeed;
            } else if (inputCrouch)
            {
                effectiveJumpHeight = jumpHeight * 2f;
                effectiveJumpSpeed = 0f;
            } else
            {
                effectiveJumpHeight = jumpHeight;
                effectiveJumpSpeed = 0f;
            }
        } else
        {
            effectiveJumpHeight = 0f;
            effectiveJumpSpeed = 0f;
        }

        switch (currentState)
        {
            case State.Walking:
                if (inputRun)
                {
                    currentState = State.Running;
                } else if (inputCrouch)
                {
                    currentState = State.Crouching;
                }

                transform.localScale = new Vector3(1f, 1f, 1f);
                effectiveSpeed = speed;

                break;

            case State.Running:
                if (!inputRun)
                {
                    currentState = State.Walking;
                }

                effectiveSpeed = speed * 2f;

                break;

            case State.Crouching:
                if (!inputCrouch)
                {
                    currentState = State.Walking;
                }

                transform.localScale = new Vector3(1f, 0.5f, 1f);
                effectiveSpeed = speed / 2f;

                break;
        }
    }

    private void FixedUpdate()
    {
        Vector3 movement = (transform.forward * inputForward + transform.right * inputStrafe) * effectiveSpeed;
        Vector3 jump = transform.up * effectiveJumpHeight + transform.forward * effectiveJumpSpeed;
        rb.AddForce(movement + jump, ForceMode.Impulse);

        rb.AddForce(new Vector3(-rb.velocity.x, 0, -rb.velocity.z) * counterFactor);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
