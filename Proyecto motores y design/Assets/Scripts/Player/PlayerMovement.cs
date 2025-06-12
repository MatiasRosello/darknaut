using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Velocidades")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float walkSpeed = 15f;
    [SerializeField] private float crouchMult = 0.8f;
    [SerializeField] private float runningMult = 1.3f;
    [SerializeField] private float JumpForce = 6f;

    [Header("Rotación")]
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float rotationThreshhold = 0.1f; //esto es un parametro que evita rotaciones muy peque�as, cuando el cursor esta muy cerca del jugador

    [Header("Estados")]
    public bool IsGrounded;
    public bool playerOnGlass;

    [Header("Referencias")]
    private Rigidbody rb;
    private BrokenGlassCollider glass;
    [SerializeField] private Camera mainCamera;

    //VARIABLE DE PANTALLA DE VICTORIA CREADA POR TOMAS
    private bool IsWin;

    private PlayerSounds source;

    void Start()
    {
        source = GetComponent<PlayerSounds>();
        rb = GetComponent<Rigidbody>();
        glass = GameObject.FindGameObjectWithTag("Glass").GetComponent<BrokenGlassCollider>();

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogWarning("No se encontr� ninguna c�mara principal asignada ni en el Inspector ni via Camera.main.");
            }
        }

    }

    void Update()
    {
        PlayerMove();
        PlayerRotate();
        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIManager.inst.ShowPauseScreen();
        }

        if (!UIManager.inst.Pause) // Solo permite movimiento y rotaci�n si no est� en pausa
        {
            
        }
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
                                                                                      //siempre se mueva de manera absoluta para adelante sin importar su rotacio

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
            source.PlayJumpNoise();
            rb.AddForce(new Vector3(0, JumpForce, 0), ForceMode.Impulse);
        }

    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }

        if (collision.gameObject.tag == "End" && !IsWin)
        {
            UIManager.inst.ShowWinScreen();
            IsWin = true;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Glass"))
        {
            playerOnGlass = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Glass"))
        {
            playerOnGlass = false;
        }
    }
}
