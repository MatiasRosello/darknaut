using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float walkSpeed = 15f;
    [SerializeField] private float crouchMult = 0.8f;
    [SerializeField] private float runningMult = 1.3f;

    [SerializeField] private float JumpForce = 6f;

    public bool IsGrounded;
    private Rigidbody rb;

    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float rotationThreshhold = 0.1f; //esto es un parametro que evita rotaciones muy pequeñas, cuando el cursor esta muy cerca del jugador
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float runStepInterval = 0.4f;  //espera entre ruidos al moverse de diferentes formas
    [SerializeField] private float walkStepInterval = 0.6f;
    [SerializeField] private float crouchStepInterval = 1.2f;

    [SerializeField] private float runNoiseIntensity = 1.5f;
    [SerializeField] private float walkNoiseIntensity = 1f;
    [SerializeField] private float crouchNoiseIntensity = 0.3f;

    private float nextStepTime = 0f;

    void Start()
    {
         rb = GetComponent<Rigidbody>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogWarning("No se encontró ninguna cámara principal asignada ni en el Inspector ni via Camera.main.");
            }
        }

    }

    void Update()
    {
        PlayerMove();
        PlayerRotate();
    }

    private void PlayerMove()
    {
        //Movimiento
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isCrouching = Input.GetKey(KeyCode.LeftControl);

        if (isRunning)  //La velocidad de correr lo modifica multiplicando por "runningMult"
        {
            playerSpeed = walkSpeed * runningMult;
        }
        else if (isCrouching) //La velocidad de agacharse lo modifica multiplicando por "crouchMult"
        {
            playerSpeed = walkSpeed * crouchMult;
        }
        else    //se asigna la velocidad base de caminar
        {
            playerSpeed = walkSpeed;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        Vector3 direction = Vector3.forward * y + Vector3.right * x;

        if (direction.magnitude > 1f)  // Si la magnitud supera 1(moverse en diagonal), normalizamos
        {
            direction.Normalize();
        }
         
        transform.Translate(direction * playerSpeed * Time.deltaTime, Space.World);   //esto traduce los vectores locales del player a vectores globales para que el player
                                                                                      //siempre se mueva de manera absoluta para adelante sin importar su rotacion
        if (IsGrounded && direction.magnitude > 0f)
        {
            float now = Time.time;

            float interval = walkStepInterval;
            float intensity = walkNoiseIntensity;

            if (isRunning)
            {
                interval = runStepInterval;
                intensity = runNoiseIntensity;
            }
            else if (isCrouching)
            {
                interval = crouchStepInterval;
                intensity = crouchNoiseIntensity;
            }

            if (now >= nextStepTime)
            {
                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.MakeNoise(transform.position, intensity);
                }
                nextStepTime = now + interval;
            }
        }

    }

    private void PlayerRotate()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 mouseWorldPos = ray.GetPoint(enter);
            Vector3 direction = mouseWorldPos - transform.position;
            direction.y = 0f;

            if (direction.magnitude < rotationThreshhold) return;
            
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
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
