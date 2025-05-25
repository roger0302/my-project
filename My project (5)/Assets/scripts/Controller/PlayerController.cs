using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxMoveSpeed = 5;
    public float moveSpeed = 0;
    public float jumpSpeed = 10;
    public float gravity = 20f;
    public bool isGrounded = true;
    private float verticalSpeed = 0;
    
    private CharacterController characterController;
    
    private PlayerInput playerInput;

    private Vector3 move;
    
    public Transform renderCamera;

    public float angleSpeed = 400;
    
    public float acceleratedSpeed = 5;
    
    private Animator animator;
    private void Awake()
    {
        characterController = transform.GetComponent<CharacterController>();
        playerInput = transform.GetComponent<PlayerInput>();
        animator = transform.GetComponent<Animator>();
    }

    private void Update()
    {
        CaculateMove();
        CaculateVerticalSpeed();
        CalculateForwardSpeed();
    }



    private void CaculateMove()
    {
        //float h = Input.GetAxis("Horizontal");
        //float v = Input.GetAxis("Vertical");

        //Vector3 move = new Vector3(h, 0, v);

       
        
        move.Set(playerInput.Move.x, 0, playerInput.Move.y);

        move *= Time.deltaTime * moveSpeed;
        move = renderCamera.TransformDirection(move);
        
        if (move.x != 0 || move.z != 0)
        {
            move.y = 0;
            //transform.rotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(move), angleSpeed * Time.deltaTime);
        }

        move += Vector3.up * verticalSpeed * Time.deltaTime;
        
        

        characterController.Move(move);
        isGrounded = characterController.isGrounded;
        
        animator.SetBool("isGround", isGrounded);
    }

    private void CaculateVerticalSpeed()
    {
        if (isGrounded)
        {
            verticalSpeed = -gravity * 0.3f;
            if (playerInput.Jump)
            {
                verticalSpeed = jumpSpeed;
                isGrounded = false;
            }
        }
        else
        {

            if (!Input.GetKeyDown(KeyCode.Space) && verticalSpeed > 0)
            {
                verticalSpeed -= gravity * Time.deltaTime;
            }
            verticalSpeed -= gravity * Time.deltaTime;
        }
        animator.SetFloat("verticalSpeed", verticalSpeed);
    }

    private void CalculateForwardSpeed()
    {
        moveSpeed = Mathf.MoveTowards(moveSpeed, maxMoveSpeed * playerInput.Move.normalized.magnitude, acceleratedSpeed * Time.deltaTime);
        animator.SetFloat("forwardSpeed",moveSpeed);
    }
}
