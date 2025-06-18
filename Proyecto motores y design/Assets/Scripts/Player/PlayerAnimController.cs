using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement playerMovement;

    private float velocity = 0f;
    [SerializeField] private float acceleration = 0.5f;
    []

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("¡No se encontró Animator en “" + name + "”!");
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool input = (h != 0f || v != 0f);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);

        if (isRunning && input)
        {
            animator.SetBool("isRunning", input);
            velocity += Time.deltaTime * acceleration;
            
        }
        else if (isCrouching && input)
        {
            animator.SetBool("isCrouching", input);
            velocity += Time.deltaTime * acceleration;
            
        }
        else
        {
            animator.SetBool("isWalking", input);
            velocity += Time.deltaTime * acceleration;
            
        }
        if (input)
        {
            animator.SetFloat("velocity", velocity);
        }

        if (!isRunning) animator.SetBool("isRunning", false);
        if (!isCrouching) animator.SetBool("isCrouching", false);


    }
}
