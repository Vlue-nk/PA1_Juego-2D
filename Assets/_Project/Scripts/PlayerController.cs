using UnityEngine;

// Movimiento y salto
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float jumpForce = 5.5f;
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private float move;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Input horizontal instantáneo (sin inercia)
        move = Input.GetAxisRaw("Horizontal");

        // 2. Giro de sprite izquierda/derecha
        if (move != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);
        }

        // 3. Chequeo de suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // 4. Salto (bloquea salto infinito)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        // Físicas horizontales manteniendo caída natural
        rb.linearVelocity = new Vector2(move * speed, rb.linearVelocity.y);
    }

    void OnDrawGizmosSelected()
    {
        // Guía visual en escena
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}