using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float movementSpeed = 5f;
    public float jumpForce = 5f;
    private bool isGrounded;
    public LayerMask groundLayer;
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        Move();
        Jump();
    }

    private void Move() {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * movementSpeed, rb.velocity.y);
    }

    private void Jump() {
        isGrounded = Physics2D.OverlapCircle(transform.position, 0.1f, groundLayer);
        if (isGrounded && Input.GetButtonDown("Jump")) {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}