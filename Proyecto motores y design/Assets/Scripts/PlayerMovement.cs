using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private float speed = 15f;
    public float MinSpeed = 7f;
    public float MaxSpeed = 25f;

    public float JumpForce = 6f;

    
    public bool IsGrounded;
    private Rigidbody rb;
    void Start()
    {
         rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //Movimiento
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = MaxSpeed;
        }
        else
        {
            speed = MinSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        transform.Translate(new Vector3(x, 0, y) * Time.deltaTime * speed);
    }

    
     public void Jump()
    {
        if (IsGrounded)
        {
                rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
            }
            

        

    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGrounded = true;
        }

        
    }
    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            IsGrounded = false;
        }
    }
     

   
}
