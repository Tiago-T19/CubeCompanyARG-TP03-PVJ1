using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 5f;

    // Esta propiedad es la que usa tu EnemigoEspecial para ralentizarte
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private bool isStop = false;
    public bool IsStop
    {
        get { return isStop; }
        set { isStop = value; }
    }

    private float moveHorizontal;
    private float moveVertical;
    private Rigidbody rb;

    [Header("Raycast Settings")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float rayDistance = 1.1f;
    [SerializeField] private LayerMask groundLayer;

    // Opcional TP3 - descomenta cuando crees el PlayerStats.asset
    // public PlayerStats stats;
    // private float SpeedReal => stats != null ? stats.velocidad : speed;
    // private float JumpReal => stats != null ? stats.fuerzaSalto : jumpForce;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // para que no se caiga de costado
    }

    void Update()
    {
        // --- GAME MANAGER TP3 - DESCOMENTAR CUANDO LO TENGAS ---
        // if (GameManager.Instance != null && GameManager.Instance.currentState != GameManager.GameState.Playing)
        // {
        //     moveHorizontal = 0;
        //     moveVertical = 0;
        //     return;
        // }

        if (isStop) return;

        moveHorizontal = Input.GetAxis("Horizontal"); // A-D
        moveVertical = Input.GetAxis("Vertical");     // W-S
        CheckGround();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // --- GAME MANAGER TP3 - DESCOMENTAR CUANDO LO TENGAS ---
        // if (GameManager.Instance != null && GameManager.Instance.currentState != GameManager.GameState.Playing)
        // {
        //     rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        //     return;
        // }

        if (isStop)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        Vector3 movement = (transform.forward * moveVertical + transform.right * moveHorizontal).normalized;
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed);
    }

    private void CheckGround()
    {
        Vector3 origin = transform.position;
        isGrounded = Physics.Raycast(origin, Vector3.down, rayDistance, groundLayer);

        Color colorRay = isGrounded ? Color.green : Color.red;
        Debug.DrawRay(origin, Vector3.down * rayDistance, colorRay);
    }

    public void RotateCharacter(float rotateY)
    {
        transform.rotation = Quaternion.Euler(0f, rotateY, 0f);
    }
}