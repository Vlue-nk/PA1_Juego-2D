using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float jumpForce = 5.5f;

    [Header("Detección Suelo")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.15f;
    [SerializeField] private LayerMask groundLayer;

    [Header("UI")]
    [SerializeField] private TMP_Text textCoins;

    private Rigidbody2D rb2D;
    private Animator animator;
    private float move;
    private bool isGrounded;
    private int coins;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 1. Input horizontal y velocidad
        move = Input.GetAxisRaw("Horizontal");
        rb2D.linearVelocity = new Vector2(move * speed, rb2D.linearVelocity.y);

        // 2. Giro visual
        if (move != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(move), 1, 1);
        }

        // 3. Salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb2D.linearVelocity = new Vector2(rb2D.linearVelocity.x, jumpForce);
        }

        // 4. Parámetros del Animator
        if (animator != null)
        {
            animator.SetFloat("Speed", Mathf.Abs(move));
            animator.SetFloat("VerticalVelocity", rb2D.linearVelocity.y);
            animator.SetBool("IsGrounded", isGrounded);
        }
    }

    void FixedUpdate()
    {
        // Chequeo físico de suelo
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Monedas: suma y destruye
        if (collision.transform.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            coins++;
            if (textCoins != null)
            {
                textCoins.text = coins.ToString();
            }
        }

        // Trampa: reinicia nivel
        if (collision.transform.CompareTag("Spikes"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}