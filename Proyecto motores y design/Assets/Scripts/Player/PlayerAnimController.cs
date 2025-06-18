using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    private Animator animator;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null) Debug.LogError("¡No se encontró Animator en “" + name + "”!");
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool isMoving = (h != 0f || v != 0f);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);

        // 1) Crouch tiene máxima prioridad
        if (isCrouching)
        {
            animator.SetBool("isCrouching", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isWalking", isMoving);
        }
        // 2) Si no crouch, chequeamos run + movimiento
        else if (isRunning && isMoving)
        {
            animator.SetBool("isRunning", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isCrouching", false);
        }
        // 3) Si no run, chequeamos walk
        else if (isMoving)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isCrouching", false);
        }
        // 4) Si nada, volvemos a Idle
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isCrouching", false);
        }
    }
}
